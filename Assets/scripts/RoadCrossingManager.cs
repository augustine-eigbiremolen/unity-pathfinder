using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCrossingManager : MonoBehaviour
{
    private const float CLOSE_DISTANCE = 1;
    private List<GameObject> roadCrossingWaypoints = new List<GameObject>();
    private GameObject curTarget = null;
    private float speed = 2;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] crossingObjects = GameObject.FindGameObjectsWithTag("crossingObjectWaypoint");
        foreach (GameObject waypoint in crossingObjects)
        {
            VisWaypointManager visWaypointManager = waypoint.GetComponent<VisWaypointManager>();
            if(visWaypointManager) roadCrossingWaypoints.Add(waypoint);
        }
        curTarget = findClosest();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(curTarget != null)
        {
            Vector3 direction = curTarget.transform.position - transform.position;
            direction.y = 0;
            float distance = direction.magnitude;
            if(direction.magnitude > 0)
            {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
            Vector3 normDir = direction/distance;
            transform.position = transform.position + normDir * speed * Time.deltaTime;

            if(distance < CLOSE_DISTANCE)
            {
                VisWaypointManager visWaypointManager = curTarget.GetComponent<VisWaypointManager>();
                if (visWaypointManager)
                {
                    if(visWaypointManager.Connections.Count == 0)
                    {
                        curTarget = null;
                    }
                    if(visWaypointManager.Connections.Count == 1)
                    {
                        curTarget = visWaypointManager.Connections[0].ToNode;
                    }
                    if(visWaypointManager.Connections.Count > 1)
                    {
                        int rndIndex = Random.Range(0, visWaypointManager.Connections.Count);
                        curTarget = visWaypointManager.Connections[rndIndex].ToNode;
                    }
                }
            }
        }
        
    }

    private GameObject findClosest()
    {
        GameObject closest = null;
        float maxDistSqrt = Mathf.Infinity;
        foreach (GameObject waypoint in roadCrossingWaypoints)
        {
            if(waypoint != null)
            {
                Vector3 dir = waypoint.transform.position - transform.position;
                if(dir.sqrMagnitude < maxDistSqrt)
                {
                    maxDistSqrt = dir.sqrMagnitude;
                    closest = waypoint;
                }
            }
        }
        return closest;
    }
}
