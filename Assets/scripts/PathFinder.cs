using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    private AStarManager aStarManager = new AStarManager();
    private List<GameObject> waypoints = new List<GameObject>();
    private SelectedPathInfo selectedPathInfo;

    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject end;

     [SerializeField]
    List<GameObject> crossingObects = new List<GameObject>();
    GameObject crossingObject = null;

    private Heuristic heuristic = new Heuristic();

    /**
    To simulate traffic or other obstacle/delays on the road dynamically, 
    a random time weight is generated for every run, and the quickest path is selected.
    This can be disabled in the Unity editor
    */
    [SerializeField]
    bool simulateTimeCost = true;

    /**
    To simulate runtime rerouting, agent periodically find a "better path", and transition is made when found.
    This can be disabled
    */
    [SerializeField]
    bool activateRuntimeReroute = true;

    private bool rerouting = false;

    private int maxRerouteCount = 3; //change route max 3 times

    Vector3 offset = new Vector3(0, 0.3f, 0);

    private float curSpeed = 8;
    private int curTarget = 0;
    private Vector3 curTargetPos;
    private int moveDir = 1;
    private bool isAgentMove = true;


    // Start is called before the first frame update
    void Start()
    {
        if(start == null || end == null)
        {
            Debug.Log("No start or end waypoint");
            return;
        }
        if(start.GetComponent<VisWaypointManager>() == null)
        {
            Debug.Log("Start is not a waypoint");
            return;
            
        }
        if(end.GetComponent<VisWaypointManager>() == null)
        {
            Debug.Log("end is not a waypoint");
            return;
            
        }

        GameObject[] waypointsWithTags = GameObject.FindGameObjectsWithTag("waypoint");
        foreach (GameObject waypoint in waypointsWithTags)
        {
            if (waypoint.GetComponent<VisWaypointManager>())
            {
                waypoints.Add(waypoint);
            }
        }

        // create connections
        foreach (GameObject waypoint in waypoints)
        {
            VisWaypointManager visWaypointManager = waypoint.GetComponent<VisWaypointManager>();

            foreach (VisGraphConnection visGraphConnection  in visWaypointManager.Connections)
            {
                if(visGraphConnection.ToNode != null)
                {
                    aStarManager.AddConnection(new Connection
                    {
                        FromNode = waypoint,
                        ToNode = visGraphConnection.ToNode,
                        Cost = heuristic.Estimate(waypoint, visGraphConnection.ToNode, simulateTimeCost)
                    });
                }
                else
                {
                    Debug.LogWarning("Missing node for "+ waypoint.name);
                }
            } 
        }

        selectedPathInfo = aStarManager.FindPath(start, end, simulateTimeCost);
        if(selectedPathInfo.Connections.Count == 0)
        {
            Debug.LogWarning("No connection found");
            return;
        }
        // move the delivery van to selected start position
        transform.position = selectedPathInfo.Connections[curTarget].FromNode.transform.position;

        if (activateRuntimeReroute)
        {
            StartCoroutine(StartRuntimeReroute());
        }
        
    }

    void OnDrawGizmos()
    {
        if(selectedPathInfo != null)
        {
             foreach (Connection connection in selectedPathInfo.Connections)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(connection.FromNode.transform.position + offset, connection.ToNode.transform.position);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAgentMove || rerouting || shouldPause())
        {
           return;
        }

        Move();
       
    }

    private IEnumerator StartRuntimeReroute()
    {
        yield return new WaitForSeconds(3f); // wait for first 3 secs
        while (isAgentMove && moveDir > 0 && maxRerouteCount > 0)
        {
            // find alternative path from my current node to target node
             GameObject curNode = selectedPathInfo.Connections[curTarget].FromNode;
             if(curNode != null)
            {
             SelectedPathInfo newPath = aStarManager.FindPath(curNode, end, simulateTimeCost);
             if(newPath.TotalCost > 0 && newPath.TotalCost < selectedPathInfo.TotalCost - selectedPathInfo.Connections[curTarget].CostSoFar)
                {
                    Debug.Log("Found better path " + newPath.TotalCost + " < " + (selectedPathInfo.TotalCost - selectedPathInfo.Connections[curTarget].Cost));
                    Debug.Log("Rerouting...");
                    rerouting = true;

                    // copy path
                    for(int i=0; i<newPath.Connections.Count; i++)
                    {
                        if( selectedPathInfo.Connections.Count > i + curTarget)
                        {
                            selectedPathInfo.Connections[i+curTarget] = newPath.Connections[i];
                        }
                        else selectedPathInfo.Connections.Add(newPath.Connections[i]);
                    }
                    selectedPathInfo.TotalCost = newPath.TotalCost;
                    maxRerouteCount--;
                    
                    // move van to new start node, will mostly be same as current
                    //FIXME: make transiction a bit more smooth
                    transform.position = curNode.transform.position;
                    rerouting = false;
                }
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private void Move()
    {
        curTargetPos = moveDir > 0 ? selectedPathInfo.Connections[curTarget].ToNode.transform.position : selectedPathInfo.Connections[curTarget].FromNode.transform.position;

        curTargetPos.y = transform.position.y;
        Vector3 direction = curTargetPos - transform.position;
        float distance = direction.magnitude;
        direction.y = 0;

        if(direction.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        Vector3 normDirection = direction / distance;
        transform.position = transform.position + normDirection * curSpeed * Time.deltaTime;

        if(distance < 1)
        {
            curTarget+=moveDir;
            if(curTarget == selectedPathInfo.Connections.Count)
            {
                // No new target, change direction and undo target change
                moveDir = -1;
                curTarget += moveDir;
            }
            if(curTarget == -1)
            {
                // No new target, change direction and undo target change
                moveDir = 1;
                curTarget += moveDir;
                isAgentMove = false; // only one trip.
            }
        }
    }

    private bool shouldPause()
    {
        float closestObstacle = float.MaxValue;

        foreach (GameObject crossingObj in crossingObects)
        {
            float objDistance = Vector3.Distance(transform.position, crossingObj.transform.position);
            if(objDistance < closestObstacle)
            {
                closestObstacle = objDistance;
                crossingObject = crossingObj;
            }
        }
        // hold break for any obstacles like people crossing, to avoid collision.
        return crossingObject != null ? Vector3.Distance(crossingObject.transform.position, transform.position) < 7 : false;
        
    }
}
