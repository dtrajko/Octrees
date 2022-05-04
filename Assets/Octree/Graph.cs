using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public List<Edge> edges = new List<Edge>();
    public List<Node> nodes = new List<Node>();
    List<Node> pathList = new List<Node>();

    public Graph()
    {
        
    }

    public void AddNode(OctreeNode otn)
    {
        if (findNode(otn.id) == null)
        {
            Node node = new Node(otn);
            nodes.Add(node);
        }
    }

    Node findNode(int otn_id)
    {
        foreach (Node n in nodes)
        {
            if (n.getNode().id == otn_id)
            {
                return n;
            }
        }

        return null;
    }
}
