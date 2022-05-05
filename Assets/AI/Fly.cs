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
        graph.AStar(graph.nodes[currentWP].octreeNode, graph.nodes[randnode].octreeNode);
        currentWP = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (graph == null) return;
        if (graph.getPathLength() == 0 || currentWP == graph.getPathLength())
        {
            GetRandomDestination();
            return;
        }

        if (Vector3.Distance(graph.getPathPoint(currentWP).nodeBounds.center,
            this.transform.position) <= accuracy)
        {
            currentWP++;
        }

        if (currentWP < graph.getPathLength())
        {
            goal = graph.getPathPoint(currentWP).nodeBounds.center;
            currentNode = graph.getPathPoint(currentWP);

            Vector3 lookAtGoal = new Vector3(goal.x, goal.y, goal.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction),
                Time.deltaTime * rotSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            GetRandomDestination();
            if (graph.getPathLength() == 0)
            {
                Debug.Log("No Path");
            }
        }
    }
}
