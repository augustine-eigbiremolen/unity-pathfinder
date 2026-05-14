using NUnit.Framework;
using UnityEngine;

public class AStarCoreTests
{
    private static Heuristic DeterministicHeuristic()
    {
        return new Heuristic();
    }

    [Test]
    public void AStar_LineGraph_FindsShortestPathInOrder()
    {
        var a = new GameObject("N0");
        var b = new GameObject("N1");
        var c = new GameObject("N2");
        a.transform.position = Vector3.zero;
        b.transform.position = Vector3.right * 5f;
        c.transform.position = Vector3.right * 10f;

        var graph = new Graph();
        graph.AddConnection(new Connection { FromNode = a, ToNode = b, Cost = 5f });
        graph.AddConnection(new Connection { FromNode = b, ToNode = c, Cost = 5f });

        var astar = new AStar();
        var result = astar.PathFindAStar(graph, a, c, DeterministicHeuristic(), simulateTime: false);

        Assert.AreEqual(2, result.Connections.Count);
        Assert.AreEqual(10f, result.TotalCost, 0.001f);
        Assert.AreEqual(a, result.Connections[0].FromNode);
        Assert.AreEqual(b, result.Connections[0].ToNode);
        Assert.AreEqual(b, result.Connections[1].FromNode);
        Assert.AreEqual(c, result.Connections[1].ToNode);

        Object.DestroyImmediate(a);
        Object.DestroyImmediate(b);
        Object.DestroyImmediate(c);
    }

    [Test]
    public void AStar_PrefersCheaperParallelRoute()
    {
        var start = new GameObject("Start");
        var cheap = new GameObject("Cheap");
        var expensive = new GameObject("Expensive");
        var goal = new GameObject("Goal");

        start.transform.position = Vector3.zero;
        cheap.transform.position = Vector3.right * 2f;
        expensive.transform.position = Vector3.up * 2f;
        goal.transform.position = Vector3.right * 4f;

        var graph = new Graph();
        graph.AddConnection(new Connection { FromNode = start, ToNode = cheap, Cost = 2f });
        graph.AddConnection(new Connection { FromNode = start, ToNode = expensive, Cost = 20f });
        graph.AddConnection(new Connection { FromNode = cheap, ToNode = goal, Cost = 2f });
        graph.AddConnection(new Connection { FromNode = expensive, ToNode = goal, Cost = 2f });

        var astar = new AStar();
        var result = astar.PathFindAStar(graph, start, goal, DeterministicHeuristic(), simulateTime: false);

        Assert.AreEqual(2, result.Connections.Count);
        Assert.AreEqual(4f, result.TotalCost, 0.001f);
        Assert.AreEqual(cheap, result.Connections[0].ToNode);

        Object.DestroyImmediate(start);
        Object.DestroyImmediate(cheap);
        Object.DestroyImmediate(expensive);
        Object.DestroyImmediate(goal);
    }

    [Test]
    public void AStar_NoPath_ReturnsEmptyConnections()
    {
        var start = new GameObject("Start");
        var orphan = new GameObject("Orphan");
        var goal = new GameObject("Goal");
        start.transform.position = Vector3.zero;
        orphan.transform.position = Vector3.one;
        goal.transform.position = Vector3.right * 5f;

        var graph = new Graph();
        graph.AddConnection(new Connection { FromNode = start, ToNode = orphan, Cost = 1f });

        var astar = new AStar();
        var result = astar.PathFindAStar(graph, start, goal, DeterministicHeuristic(), simulateTime: false);

        Assert.IsNotNull(result.Connections);
        Assert.AreEqual(0, result.Connections.Count);
        Assert.AreEqual(0f, result.TotalCost);

        Object.DestroyImmediate(start);
        Object.DestroyImmediate(orphan);
        Object.DestroyImmediate(goal);
    }

    [Test]
    public void AStar_StartEqualsGoal_ReturnsEmptyPathWithZeroCost()
    {
        var node = new GameObject("Lonely");
        node.transform.position = Vector3.zero;

        var graph = new Graph();
        var astar = new AStar();
        var result = astar.PathFindAStar(graph, node, node, DeterministicHeuristic(), simulateTime: false);

        Assert.IsNotNull(result.Connections);
        Assert.AreEqual(0, result.Connections.Count);
        Assert.AreEqual(0f, result.TotalCost);

        Object.DestroyImmediate(node);
    }
}
