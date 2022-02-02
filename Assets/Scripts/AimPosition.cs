using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using StarterAssets;

public class AimPosition : MonoBehaviour
{
    ThirdPersonController thirdPersonController;

    private void Awake() {
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
