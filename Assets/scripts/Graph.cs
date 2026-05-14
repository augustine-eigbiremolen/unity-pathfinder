using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph 
{
    private List<Connection> connections = new List<Connection>();

    public Graph()
    {
        
    }

    public void AddConnection(Connection con)
    {
        connections.Add(con);
    }
    public List<Connection> GetConnections(GameObject fromNode)
    {
        List<Connection> cons = new List<Connection>();

        foreach (Connection connection in connections)
        {
            if (connection.FromNode.Equals(fromNode))
            {
                cons.Add(connection);
            }
        }
        return cons;
    }
}
