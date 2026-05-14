using UnityEngine;

public class NodeRecord 
{
    private GameObject node;
    private float costSoFar;
    private float estimatedCost;
    private Connection connection;

    public GameObject Node
    {
        get{ return node; }
        set{ node = value; }
    }

    public float CostSoFar
    {
        get { return costSoFar; }
        set { costSoFar = value; }
    }

    public float EstimatedTotalCost
    {
        get{ return estimatedCost;}
        set{ estimatedCost = value;}
    }

    public Connection Connection
    {
        get { return connection;}
        set{ connection = value;}
    }

    public NodeRecord()
    {
        
    }
    
}
