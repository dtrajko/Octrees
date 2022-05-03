using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    public OctreeNode rootNode;
    public Bounds boundsD;

    public Octree(GameObject[] worldObjects, float minNodeSize)
    {
        Bounds bounds = new Bounds();

        foreach (GameObject go in worldObjects)
        {
            bounds.Encapsulate(go.GetComponent<Collider>().bounds);
        }

        boundsD = bounds;
    }

}
