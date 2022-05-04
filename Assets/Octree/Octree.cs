using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    public OctreeNode rootNode;
    public List<OctreeNode> emptyLeaves = new List<OctreeNode>();
    public Graph navigationGraph;

    public Octree(GameObject[] worldObjects, float minNodeSize, Graph navgraph)
    {
        Bounds bounds = new Bounds();
        navigationGraph = navgraph;

        foreach (GameObject go in worldObjects)
        {
            bounds.Encapsulate(go.GetComponent<Collider>().bounds);
        }

        float maxSize = Mathf.Max(new float[] { bounds.size.x, bounds.size.y, bounds.size.z });
        Vector3 sizeVector = new Vector3(maxSize, maxSize, maxSize) * 0.5f;
        bounds.SetMinMax(bounds.center - sizeVector, bounds.center + sizeVector);

        // boundsD = bounds;

        rootNode = new OctreeNode(bounds, minNodeSize);
        AddObjects(worldObjects);
        GetEmptyLeaves(rootNode);
    }

    public void AddObjects(GameObject[] worldObjects)
    {
        foreach (GameObject go in worldObjects)
        {
            rootNode.AddObject(go);
        }
    
    }

    public void GetEmptyLeaves(OctreeNode node)
    {
        if (node == null) return;
        if (node.children == null)
        {
            if (node.containedObjects.Count == 0)
            {
                emptyLeaves.Add(node);
                navigationGraph.AddNode(node);
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                GetEmptyLeaves(node.children[i]);
            }
        }
    }
}
