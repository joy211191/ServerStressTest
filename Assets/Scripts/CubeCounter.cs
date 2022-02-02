using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CubeCounter : MonoBehaviour
{
    List<GameObject> cubes = new List<GameObject>();
    [SerializeField]
    Text counterText;

    private void Start() {
        InvokeRepeating("CubeCounting", 2f, 0.3f);
    }

    void CubeCounting() {
        cubes.Clear();
        cubes = GameObject.FindGameObjectsWithTag("Cube").ToList();
        counterText.text = cubes.Count.ToString();
    }
}
