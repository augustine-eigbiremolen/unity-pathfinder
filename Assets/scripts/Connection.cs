using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    private float cost = 0;
    private float costSoFar = 0;
    private GameObject fromNode;
    private GameObject toNode;

    public float Cost
    {
        get
        {
            if(cost == 0)
            {
                cost = Vector3.Distance(fromNode.transform.position, toNode.transform.position);
            }
            return cost;
            
        }
        set { cost = value ;}
    }

     public float CostSoFar
    {
        get
        {
            return costSoFar;
            
        }
        set { costSoFar = value ;}
    }

    public GameObject FromNode
    {
        get { return fromNode; }
        set
        {
            fromNode = value;
            cost = 0;
        }
    }

    public GameObject ToNode
    {
        get { return toNode; }
        set
        {
            toNode = value;
            cost = 0;
        }
    }
    

    public Connection()
    {
        
    }
    
}
