using System.Collections;
using System.Collections.Generic;
using System;
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

        rootNode = new OctreeNode(bounds, minNodeSize, null);
        AddObjects(worldObjects);
        GetEmptyLeaves(rootNode);
        ProcessExtraConnections();
        Debug.Log("Edge: " + navigationGraph.edges.Count);
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

                for (int s = 0; s < 8; s++)
                {
                    if (s != i)
                    {
                        navigationGraph.AddEdge(node.children[i], node.children[s]);
                    }
                }
            }
        }
    }

    void ProcessExtraConnections()
    {
        Dictionary<int, int> subGraphConnections = new Dictionary<int, int>();

        foreach (OctreeNode i in emptyLeaves)
        {
            foreach (OctreeNode j in emptyLeaves)
            {
                if (i.id != j.id && i.parent.id != j.parent.id)
                {
                    RaycastHit hitInfo;
                    Vector3 direction = j.nodeBounds.center - i.nodeBounds.center;
                    float accuracy = 1;
                    if (!Physics.SphereCast(i.nodeBounds.center, accuracy, direction, out hitInfo))
                    {
                        try
                        {
                            subGraphConnections.Add(i.parent.id, j.parent.id);
                            navigationGraph.AddEdge(i, j);
                        }
                        catch (ArgumentException e)
                        {
                            Debug.Log(e.Message);
                            //...
                        }
                    }
                }
            }
        }
    }
}
