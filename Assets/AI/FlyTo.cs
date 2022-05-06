using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTo : MonoBehaviour
{
    float speed = 10.0f;
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

    void LateUpdate()
    {
        if (graph == null) return;
        if (getPathLength() == 0 || currentWP == getPathLength())
        {
            return;
        }

        if (Vector3.Distance(getPathPoint(currentWP).nodeBounds.center,
            this.transform.position) <= accuracy)
        {
            currentWP++;
        }

        if (currentWP < getPathLength())
        {
            goal = getPathPoint(currentWP).nodeBounds.center;
            Vector3 lookAtGoal = new Vector3(goal.x, goal.y, goal.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            this.transform.rotation = Quaternion.Slerp(
                this.transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * rotSpeed);

            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            if (getPathLength() == 0)
            {
                return;
            }
        }
    }
}
