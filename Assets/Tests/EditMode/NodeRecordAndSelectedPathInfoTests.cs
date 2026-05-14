using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class NodeRecordAndSelectedPathInfoTests
{
    [Test]
    public void NodeRecord_Properties_RoundTrip()
    {
        var go = new GameObject("Node");
        var connection = new Connection { FromNode = go, ToNode = go, Cost = 1f };

        var record = new NodeRecord
        {
            Node = go,
            CostSoFar = 2.5f,
            EstimatedTotalCost = 9f,
            Connection = connection
        };

        Assert.AreSame(go, record.Node);
        Assert.AreEqual(2.5f, record.CostSoFar);
        Assert.AreEqual(9f, record.EstimatedTotalCost);
        Assert.AreSame(connection, record.Connection);

        Object.DestroyImmediate(go);
    }

    [Test]
    public void SelectedPathInfo_ConnectionsAndTotalCost_RoundTrip()
    {
        var from = new GameObject("From");
        var to = new GameObject("To");
        from.transform.position = Vector3.zero;
        to.transform.position = Vector3.one;

        var info = new SelectedPathInfo
        {
            Connections = new List<Connection>
            {
                new Connection { FromNode = from, ToNode = to, Cost = 3f }
            },
            TotalCost = 3f
        };

        Assert.AreEqual(1, info.Connections.Count);
        Assert.AreEqual(3f, info.TotalCost);

        Object.DestroyImmediate(from);
        Object.DestroyImmediate(to);
    }
}
