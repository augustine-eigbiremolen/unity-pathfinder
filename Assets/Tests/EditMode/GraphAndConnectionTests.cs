using NUnit.Framework;
using UnityEngine;

public class GraphAndConnectionTests
{
    [Test]
    public void Graph_AddConnection_GetConnections_ReturnsOnlyMatchingFromNode()
    {
        var graph = new Graph();
        var a = new GameObject("A");
        var b = new GameObject("B");
        var c = new GameObject("C");
        a.transform.position = Vector3.zero;
        b.transform.position = Vector3.right * 3f;
        c.transform.position = Vector3.right * 6f;

        graph.AddConnection(new Connection { FromNode = a, ToNode = b, Cost = 1f });
        graph.AddConnection(new Connection { FromNode = b, ToNode = c, Cost = 2f });

        var fromA = graph.GetConnections(a);
        var fromB = graph.GetConnections(b);

        Assert.AreEqual(1, fromA.Count);
        Assert.AreEqual(b, fromA[0].ToNode);
        Assert.AreEqual(1, fromB.Count);
        Assert.AreEqual(c, fromB[0].ToNode);

        Object.DestroyImmediate(a);
        Object.DestroyImmediate(b);
        Object.DestroyImmediate(c);
    }

    [Test]
    public void Connection_CostGetter_UsesSerializedCostWhenNonZero()
    {
        var from = new GameObject("From");
        var to = new GameObject("To");
        from.transform.position = Vector3.zero;
        to.transform.position = new Vector3(3f, 4f, 0f);

        var connection = new Connection { FromNode = from, ToNode = to, Cost = 99f };
        Assert.AreEqual(99f, connection.Cost);

        Object.DestroyImmediate(from);
        Object.DestroyImmediate(to);
    }

    [Test]
    public void Connection_CostGetter_FallsBackToDistanceWhenUnset()
    {
        var from = new GameObject("From");
        var to = new GameObject("To");
        from.transform.position = Vector3.zero;
        to.transform.position = new Vector3(0f, 3f, 4f);

        var connection = new Connection { FromNode = from, ToNode = to };
        Assert.AreEqual(5f, connection.Cost, 0.0001f);

        Object.DestroyImmediate(from);
        Object.DestroyImmediate(to);
    }
}
