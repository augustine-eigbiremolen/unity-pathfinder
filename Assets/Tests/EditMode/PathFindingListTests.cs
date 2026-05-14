using NUnit.Framework;
using UnityEngine;

public class PathFindingListTests
{
    [Test]
    public void PathFindingList_GetSmallest_ReturnsLowestEstimatedTotalCost()
    {
        var list = new PathFindingList();
        var goA = new GameObject("A");
        var goB = new GameObject("B");

        list.AddNodeRecord(new NodeRecord
        {
            Node = goA,
            EstimatedTotalCost = 10f
        });
        list.AddNodeRecord(new NodeRecord
        {
            Node = goB,
            EstimatedTotalCost = 3f
        });

        var smallest = list.GetSmallest();
        Assert.AreEqual(goB, smallest.Node);
        Assert.AreEqual(3f, smallest.EstimatedTotalCost);

        Object.DestroyImmediate(goA);
        Object.DestroyImmediate(goB);
    }

    [Test]
    public void PathFindingList_ContainsAndFind_WorkForGameObjectNodes()
    {
        var list = new PathFindingList();
        var go = new GameObject("Node");
        var record = new NodeRecord { Node = go, EstimatedTotalCost = 1f };
        list.AddNodeRecord(record);

        Assert.IsTrue(list.Contains(go));
        Assert.AreSame(record, list.Find(go));

        Object.DestroyImmediate(go);
    }
}
