﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OctreeObject
{
    public Bounds bounds;
    public GameObject gameObject;

    public OctreeObject(GameObject go)
    {
        bounds = go.GetComponent<Collider>().bounds;
        gameObject = go;
    }
}

public class OctreeNode
{
    Bounds nodeBounds;
    Bounds[] childBounds;
    public OctreeNode[] children = null;
    float minSize;
    List<OctreeObject> containedObjects = new List<OctreeObject>();

    public OctreeNode(Bounds b, float minNodeSize)
    {
        nodeBounds = b;
        minSize = minNodeSize;

        float quarter = nodeBounds.size.y / 4f;
        float childLength = nodeBounds.size.y / 2f;
        Vector3 childSize = new Vector3(childLength, childLength, childLength);
        childBounds = new Bounds[8];
        childBounds[0] = new Bounds(nodeBounds.center + new Vector3(-quarter,  quarter, -quarter), childSize);
        childBounds[1] = new Bounds(nodeBounds.center + new Vector3( quarter,  quarter, -quarter), childSize);
        childBounds[2] = new Bounds(nodeBounds.center + new Vector3(-quarter,  quarter,  quarter), childSize);
        childBounds[3] = new Bounds(nodeBounds.center + new Vector3( quarter,  quarter,  quarter), childSize);
        childBounds[4] = new Bounds(nodeBounds.center + new Vector3(-quarter, -quarter, -quarter), childSize);
        childBounds[5] = new Bounds(nodeBounds.center + new Vector3( quarter, -quarter, -quarter), childSize);
        childBounds[6] = new Bounds(nodeBounds.center + new Vector3(-quarter, -quarter,  quarter), childSize);
        childBounds[7] = new Bounds(nodeBounds.center + new Vector3( quarter, -quarter,  quarter), childSize);

        Divide();
    }

    public void Divide()
    {
        if (nodeBounds.size.y <= minSize) return;

        children = new OctreeNode[8];
        for (int i = 0; i < 8; i++)
        {
            children[i] = new OctreeNode(childBounds[i], minSize);
        }
    }

    public void Draw()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireCube(nodeBounds.center, nodeBounds.size);

        if (children != null)
        {
            for (int i = 0; i < 8; i++)
            {
                children[i].Draw();
            }
        }

        //  Gizmos.color = new Color(1, 0, 0);
        //  foreach (Bounds b in childBounds)
        //  {
        //      Gizmos.DrawWireCube(b.center, b.size);
        //  }
    }
}
