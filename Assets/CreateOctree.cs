using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOctree : MonoBehaviour
{
    public GameObject[] worldObjects;
    public int nodeMinSize = 5;
    public Octree ot;
    public Graph waypoints;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new Graph();
        ot = new Octree(worldObjects, nodeMinSize, waypoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            ot.rootNode.Draw();
            ot.navigationGraph.Draw();
        }
    }
}
