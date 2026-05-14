using System.Collections.Generic;
using UnityEngine;

public class PathFindingList
{

    private List<NodeRecord> nodeRecords = new List<NodeRecord>();

    public PathFindingList()
    {
    }

    public void AddNodeRecord(NodeRecord nodeRecord)
    {
        nodeRecords.Add(nodeRecord);
    }

    public void RemoveNodeRecord(NodeRecord nodeRecord)
    {
        nodeRecords.Remove(nodeRecord);
    }

    public int GetSize()
    {
        return nodeRecords.Count;
    }
    public NodeRecord GetSmallest()
    {
        NodeRecord nodeRecord = new NodeRecord
        {
            EstimatedTotalCost = float.MaxValue
        };

        foreach (NodeRecord node in nodeRecords)
        {
            if(node.EstimatedTotalCost < nodeRecord.EstimatedTotalCost)
            {
                nodeRecord = node;
            }
            
        }
        return nodeRecord;
    }

    public bool Contains(GameObject node)
    {
        foreach (NodeRecord nodeRecord in nodeRecords)
        {
            if (nodeRecord.Node != null && nodeRecord.Node.Equals(node))
            {
                return true;
            }
        }
        return false;
    }

    public NodeRecord Find(GameObject nodeRecord)
    {
        foreach (NodeRecord node in nodeRecords)
        {
            if (node.Node.Equals(nodeRecord.gameObject))
            {
                return node;
            }
        }
        return null;
    }
}
