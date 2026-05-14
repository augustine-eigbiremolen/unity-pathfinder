using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager 
{
    private AStar aStar = new AStar();
    private Graph graph = new Graph();
    private Heuristic heuristic = new Heuristic();

    public AStarManager(){}

    public void AddConnection(Connection connection)
    {
        graph.AddConnection(connection);
    }
    public SelectedPathInfo FindPath(GameObject start, GameObject end, bool simulateTimeCost)
    {
        return aStar.PathFindAStar(graph, start, end, heuristic, simulateTimeCost);
    }
    
}
