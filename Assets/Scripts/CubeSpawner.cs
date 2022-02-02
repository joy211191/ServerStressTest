using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CubeSpawner : NetworkBehaviour {
    public GameObject cube;

    private void Start() {
        //InvokeRepeating("CmdSpawner", 1f, 0.1f);
    }

    private void OnEnable() {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner() {
        while (true) {
            CmdSpawner();
            yield return new WaitForSeconds(0.1f);
        }
    }

    [Command]
    void CmdSpawner() {
        GameObject go = Instantiate(cube, transform.position, Quaternion.identity);
        NetworkServer.Spawn(go);
    }
}