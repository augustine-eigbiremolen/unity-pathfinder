using NUnit.Framework;
using UnityEngine;

public class AStarManagerHeuristicTests
{
    [Test]
    public void AStarManager_FindPath_DelegatesToGraph()
    {
        var a = new GameObject("A");
        var b = new GameObject("B");
        a.transform.position = Vector3.zero;
        b.transform.position = new Vector3(0f, 0f, 7f);

        var manager = new AStarManager();
        manager.AddConnection(new Connection { FromNode = a, ToNode = b, Cost = 7f });

        var path = manager.FindPath(a, b, simulateTimeCost: false);

        Assert.AreEqual(1, path.Connections.Count);
        Assert.AreEqual(7f, path.TotalCost, 0.001f);

        Object.DestroyImmediate(a);
        Object.DestroyImmediate(b);
    }

    [Test]
    public void Heuristic_WithoutSimulatedTime_MatchesWorldDistance()
    {
        var start = new GameObject("S");
        var goal = new GameObject("G");
        start.transform.position = new Vector3(1f, 2f, 3f);
        goal.transform.position = new Vector3(4f, 6f, 8f);

        var heuristic = new Heuristic();
        float estimate = heuristic.Estimate(start, goal, simulateTime: false);

        Assert.AreEqual(Vector3.Distance(start.transform.position, goal.transform.position), estimate, 0.0001f);

        Object.DestroyImmediate(start);
        Object.DestroyImmediate(goal);
    }
}
