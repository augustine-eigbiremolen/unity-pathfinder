using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RoadCrossingManagerPlayModeTests
{
    private static object GetPrivateField(object target, string name)
    {
        FieldInfo field = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(field, $"Missing field {name} on {target.GetType().Name}");
        return field.GetValue(target);
    }

    [UnityTest]
    public IEnumerator RoadCrossingManager_Start_SelectsClosestTaggedWaypoint()
    {
        var root = new GameObject("CrossingIntegrationRoot");

        var pedestrian = new GameObject("Pedestrian");
        pedestrian.transform.SetParent(root.transform, false);
        pedestrian.transform.position = Vector3.zero;
        var manager = pedestrian.AddComponent<RoadCrossingManager>();

        var near = new GameObject("NearCrossing");
        near.transform.SetParent(root.transform, false);
        near.transform.position = new Vector3(4f, 0f, 0f);
        near.tag = "crossingObjectWaypoint";
        near.AddComponent<VisWaypointManager>();

        var far = new GameObject("FarCrossing");
        far.transform.SetParent(root.transform, false);
        far.transform.position = new Vector3(40f, 0f, 0f);
        far.tag = "crossingObjectWaypoint";
        far.AddComponent<VisWaypointManager>();

        yield return null;

        var curTarget = GetPrivateField(manager, "curTarget") as GameObject;
        Assert.IsNotNull(curTarget);
        Assert.AreEqual(near, curTarget);

        Object.Destroy(root);
    }
}
