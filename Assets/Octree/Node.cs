using System.Collections.Generic;

public class Node
{
    public List<Edge> edgeList = new List<Edge>();
    public Node path = null;
    public OctreeNode octreeNode;
    public float g; // movement cost
    public float h; // heuristic (best intelligent guess)
    public float f; // h + g
    public Node cameFrom;

    // Update is called once per frame
    public Node(OctreeNode n)
    {
        octreeNode = n;
        path = null;
    }

    public OctreeNode getNode()
    {
        return octreeNode;
    }
}
