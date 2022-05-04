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

    public void AddEdge(OctreeNode fromNode, OctreeNode toNode)
    {
        Node from = findNode(fromNode.id);
        Node to = findNode(toNode.id);

        if (from != null && to != null)
        {
            Edge e = new Edge(from, to);
            edges.Add(e);
            from.edgeList.Add(e);
            Edge f = new Edge(to, from);
            edges.Add(f);
            to.edgeList.Add(f);
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

    public int getPathLength()
    {
        return pathList.Count;
    }

    public OctreeNode getPathPoint(int index)
    {
        return pathList[index].octreeNode;
    }

    public void Draw()
    {
        for (int i = 0; i < edges.Count; i++)
        {
            Debug.DrawLine(
                edges[i].startNode.octreeNode.nodeBounds.center,
                edges[i].endNode.octreeNode.nodeBounds.center,
                Color.red);
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Gizmos.color = new Color(1, 1, 0);
            // Gizmos.DrawWireSphere(nodes[i].octreeNode.nodeBounds.center, nodes[i].octreeNode.nodeBounds.size.y / 2f);
            Gizmos.DrawWireSphere(nodes[i].octreeNode.nodeBounds.center, 0.1f);
        }
    }

}
