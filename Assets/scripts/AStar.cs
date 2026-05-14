using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar 
{
    public AStar(){}

    public SelectedPathInfo PathFindAStar(Graph graph, GameObject start, GameObject end, Heuristic heuristic, bool simulateTime)
    {
        NodeRecord startNode = new NodeRecord
        {
            Node = start,
            Connection = null,
            CostSoFar = 0,
            EstimatedTotalCost = heuristic.Estimate(start, end, simulateTime)
        };

        PathFindingList openList = new PathFindingList();
        PathFindingList closedList = new PathFindingList();


        openList.AddNodeRecord(startNode);

        NodeRecord curRec = null;
        List<Connection> connections;

        while(openList.GetSize() > 0)
        {
            curRec = openList.GetSmallest();
            if(curRec.Node.Equals(end)) break;

            connections = graph.GetConnections(curRec.Node);

            GameObject endNode;
            float endNodeCost;
            NodeRecord endNodeRecord;
            float endNodeHeuristic;

            foreach (Connection connection in connections)
            {
                endNode = connection.ToNode;
                endNodeCost = curRec.CostSoFar + connection.Cost;

                if (closedList.Contains(endNode))
                {
                    endNodeRecord = closedList.Find(endNode);

                    if(endNodeRecord.CostSoFar <= endNodeCost) continue;

                    closedList.RemoveNodeRecord(endNodeRecord);
                    endNodeHeuristic = endNodeRecord.EstimatedTotalCost - endNodeRecord.CostSoFar;
                }
                else if (openList.Contains(endNode))
                {
                    endNodeRecord = openList.Find(endNode);
                    if(endNodeRecord.CostSoFar <= endNodeCost) continue;
                    endNodeHeuristic = endNodeRecord.EstimatedTotalCost - endNodeRecord.CostSoFar;
                }
                else
                {
                    endNodeRecord = new NodeRecord
                    {
                        Node = endNode
                    };
                    endNodeHeuristic = heuristic.Estimate(endNode, end, simulateTime);
                }

                endNodeRecord.CostSoFar = endNodeCost;
                endNodeRecord.Connection = connection;
                endNodeRecord.EstimatedTotalCost = endNodeCost + endNodeHeuristic;

                if(!openList.Contains(endNode))
                {
                    openList.AddNodeRecord(endNodeRecord);
                }
            } // # END foreach loop

            openList.RemoveNodeRecord(curRec);
            closedList.AddNodeRecord(curRec);
        }

        List<Connection> tempConnections = new List<Connection>();
        if(!curRec.Node.Equals(end)) return new SelectedPathInfo {
            Connections = tempConnections,
            TotalCost = 0
        };

        else
        {
            while (curRec != null && !curRec.Node.Equals(start))
            {
                tempConnections.Add(curRec.Connection);
                curRec = closedList.Find(curRec.Connection.FromNode);
            }
            // reverse the path
            List<Connection> reversed = new List<Connection>();
            float total = 0;
            for (int i = tempConnections.Count -1 ; i >=0; i--){
                tempConnections[i].CostSoFar += tempConnections[i].Cost;
                total+=tempConnections[i].Cost;
                reversed.Add(tempConnections[i]);
            }

            return new SelectedPathInfo
            {
                Connections = reversed,
                TotalCost = total
            };
        }
    }
    
}
