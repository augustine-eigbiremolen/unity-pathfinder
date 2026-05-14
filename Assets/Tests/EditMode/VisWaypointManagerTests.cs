using NUnit.Framework;
using UnityEngine;

public class VisWaypointManagerTests
{
    [Test]
    public void VisWaypointManager_Connections_IsInitializedAndMutable()
    {
        var go = new GameObject("Waypoint");
        var waypoint = go.AddComponent<VisWaypointManager>();
        var other = new GameObject("Other");
        other.transform.position = Vector3.right;

        Assert.IsNotNull(waypoint.Connections);
        Assert.AreEqual(0, waypoint.Connections.Count);

        waypoint.Connections.Add(new VisGraphConnection { ToNode = other });
        Assert.AreEqual(1, waypoint.Connections.Count);
        Assert.AreEqual(other, waypoint.Connections[0].ToNode);

        Object.DestroyImmediate(other);
        Object.DestroyImmediate(go);
    }
}
