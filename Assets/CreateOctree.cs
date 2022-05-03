using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOctree : MonoBehaviour
{
    public GameObject[] worldObjects;
    Octree ot;

    // Start is called before the first frame update
    void Start()
    {
        ot = new Octree(worldObjects, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(0, 1, 0);
            Gizmos.DrawWireCube(ot.boundsD.center, ot.boundsD.size);
        }
    }
}
