using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PathFinderPlayModeTests
{
    private static void SetPrivateField(object target, string name, object value)
    {
        FieldInfo field = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(field, $"Missing field {name} on {target.GetType().Name}");
        field.SetValue(target, value);
    }

    private static SelectedPathInfo GetSelectedPath(PathFinder pathFinder)
    {
        FieldInfo field = typeof(PathFinder).GetField("selectedPathInfo", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(field);
        return (SelectedPathInfo)field.GetValue(pathFinder);
    }

    [UnityTest]
    public IEnumerator PathFinder_Start_BuildsPathFromTaggedWaypointGraph()
    {
        var root = new GameObject("IntegrationRoot");

        var goStart = new GameObject("WpStart");
        var goMid = new GameObject("WpMid");
        var goEnd = new GameObject("WpEnd");
        goStart.transform.SetParent(root.transform, false);
        goMid.transform.SetParent(root.transform, false);
        goEnd.transform.SetParent(root.transform, false);

        goStart.tag = "waypoint";
        goMid.tag = "waypoint";
        goEnd.tag = "waypoint";

        goStart.transform.position = Vector3.zero;
        goMid.transform.position = Vector3.right * 10f;
        goEnd.transform.position = Vector3.right * 20f;

        var startWp = goStart.AddComponent<VisWaypointManager>();
        var midWp = goMid.AddComponent<VisWaypointManager>();
        var endWp = goEnd.AddComponent<VisWaypointManager>();

        startWp.Connections.Add(new VisGraphConnection { ToNode = goMid });
        midWp.Connections.Add(new VisGraphConnection { ToNode = goEnd });
        endWp.Connections.Clear();

        var agent = new GameObject("Agent");
        agent.transform.SetParent(root.transform, false);
        var pathFinder = agent.AddComponent<PathFinder>();

        SetPrivateField(pathFinder, "start", goStart);
        SetPrivateField(pathFinder, "end", goEnd);
        SetPrivateField(pathFinder, "simulateTimeCost", false);
        SetPrivateField(pathFinder, "activateRuntimeReroute", false);

        yield return null;

        SelectedPathInfo path = GetSelectedPath(pathFinder);
        Assert.IsNotNull(path);
        Assert.AreEqual(2, path.Connections.Count);
        Assert.Greater(path.TotalCost, 0f);
        Assert.AreEqual(goStart, path.Connections[0].FromNode);
        Assert.AreEqual(goEnd, path.Connections[1].ToNode);

        Object.Destroy(root);
    }
}
