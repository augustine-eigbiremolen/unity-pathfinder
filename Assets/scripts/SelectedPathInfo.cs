using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPathInfo 
{
    private List<Connection> connections;
   
    private float totalCost;

    public List<Connection> Connections
    {
         
         get
        {
           return connections;
            
        }
        set { connections = value ;}
    
    }

    public float TotalCost
    {
         
         get
        {
           return totalCost;
            
        }
        set { totalCost = value ;}
    
    }

    public SelectedPathInfo()
    {
        
    }

}
