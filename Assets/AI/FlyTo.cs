using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTo : MonoBehaviour
{
    float speed = 20.0f;
    float accuracy = 1.0f;
    float rotSpeed = 15.0f;

    int currentWP = 0;
    Vector3 goal;

    public GameObject octree;
    Graph graph;
    Octree ot;
    List<Node> pathList = new List<Node>();

    public GameObject goalPosition;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Navigate", 1);
    }

    void Navigate()
    {
        graph = octree.GetComponent<CreateOctree>().waypoints;
        ot = octree.GetComponent<CreateOctree>().ot;
    }

    void NavigateTo(int destination, Node finalGoal)
    {
        Node destinationNode = graph.findNode(destination);
        graph.AStar(graph.nodes[currentWP].octreeNode, destinationNode.octreeNode, pathList);
        currentWP = 0;
        pathList.Add(finalGoal);
    }

    public int getPathLength()
    {
        return pathList.Count;
    }

    public OctreeNode getPathPoint(int index)
    {
        return pathList[index].octreeNode;
    }

    // Update is called once per frame
    void Update()
    {
        if (ot == null) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = goalPosition.transform.position;
            int i = ot.AddDestination(pos);
            if (i == -1)
            {
                Debug.Log("Destination not found in Octree.");
                return;
            }
            Node finalGoal = new Node(new OctreeNode(new Bounds(pos, new Vector3(1, 1, 1)), 1, null));
            NavigateTo(i, finalGoal);
        }
    }
}
