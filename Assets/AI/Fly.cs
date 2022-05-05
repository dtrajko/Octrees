using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    float speed = 5.0f;
    float accuracy = 1.0f;
    float rotSpeed = 5.0f;

    int currentWP = 1;
    Vector3 goal;
    OctreeNode currentNode;

    public GameObject octree;
    Graph graph;
    List<Node> pathList = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Navigate", 1);
    }

    void Navigate()
    {
        graph = octree.GetComponent<CreateOctree>().waypoints;
        currentNode = graph.nodes[currentWP].octreeNode;
        GetRandomDestination();
    }

    void GetRandomDestination()
    {
        int randnode = Random.Range(0, graph.nodes.Count);
        graph.AStar(graph.nodes[currentWP].octreeNode, graph.nodes[randnode].octreeNode, pathList);
        currentWP = 0;
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
    void LateUpdate()
    {
        if (graph == null) return;
        if (getPathLength() == 0 || currentWP == getPathLength())
        {
            GetRandomDestination();
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
            currentNode = getPathPoint(currentWP);

            Vector3 lookAtGoal = new Vector3(goal.x, goal.y, goal.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction),
                Time.deltaTime * rotSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            GetRandomDestination();
            if (getPathLength() == 0)
            {
                Debug.Log("No Path");
            }
        }
    }
}
