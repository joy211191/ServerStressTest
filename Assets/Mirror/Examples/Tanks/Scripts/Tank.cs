using UnityEngine;
using UnityEngine.AI;

namespace Mirror.Examples.Tanks {
    public class Tank : NetworkBehaviour {
        [SerializeField] private bool m_LockCursor = false;

        GameObject camObject;
        Camera cam;

        [Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;
        public TextMesh healthBar;
        public Transform turret;

        public Transform chasis;

        [Header("Movement")]
        public float rotationSpeed = 100;

        [Header("Firing")]
        public KeyCode shootKey = KeyCode.Space;
        public GameObject projectilePrefab;
        public Transform projectileMount;

        [Header("Stats")]
        [SyncVar] public int health = 4;

        bool setupDone;


        private void OnEnable() {
            Invoke("Initialize", 2f);
        }

        void Initialize() {
            if (isLocalPlayer) {
                camObject = FindObjectOfType<FreeLookCam>().gameObject;
                camObject.GetComponent<FreeLookCam>().m_Target = chasis;
                cam = camObject.GetComponentInChildren<Camera>();
                m_LockCursor = true;
                Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !m_LockCursor;
            }
            else
                enabled = false;

            setupDone = true;
        }

        void Update() {
            if (!setupDone)
                return;
            // always update health bar.
            // (SyncVar hook would only update on clients, not on server)
            healthBar.text = new string('-', health);

            // movement for local player
            if (isLocalPlayer) {
                // rotate
                float horizontal = Input.GetAxis("Horizontal");
                transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);

                // move
                float vertical = Input.GetAxis("Vertical");
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                agent.velocity = forward * Mathf.Max(vertical, 0) * agent.speed;
                animator.SetBool("Moving", agent.velocity != Vector3.zero);
                CmdFire();
                // shoot
                /*if (Input.GetKeyDown(shootKey))
                {
                    
                }*/

                RotateTurret();
            }
        }

        // this is called on the server
        [Command]
        void CmdFire() {
            GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation);
            NetworkServer.Spawn(projectile);
            RpcOnFire();
        }

        // this is called on the tank that fired for all observers
        [ClientRpc]
        void RpcOnFire() {
            animator.SetTrigger("Shoot");
        }

        [ServerCallback]
        void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Projectile>() != null) {
                --health;
                if (health == 0)
                    NetworkServer.Destroy(gameObject);
            }
        }

        void RotateTurret() {
            turret.transform.forward = cam.transform.forward;
        }
    }
}