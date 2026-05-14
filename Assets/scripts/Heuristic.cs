using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristic
{

    public Heuristic(){}

    public float Estimate(GameObject startNode, GameObject goal, bool simulateTime)
    {
        Vector3 costStart = startNode.transform.position;
        Vector3 costGoal = goal.transform.position;
        if (simulateTime)
        {
            int timeCostStart = Random.Range(2, 2000);
            int timeCostGoal = Random.Range(2, 2000);
            costStart = costStart * timeCostStart / Random.Range(4, 20);
            costGoal = costGoal * timeCostGoal / Random.Range(4, 6);
        }
        return Vector3.Distance(costStart, costGoal);
    }
  
}
