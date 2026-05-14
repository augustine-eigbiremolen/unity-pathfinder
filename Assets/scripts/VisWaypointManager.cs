using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VisWaypointManager : MonoBehaviour
{
    private enum WaypointTextColor
    {
        Blue,
        Cyan,
        Yellow
    }

    private enum WaypointPropList
    {
        Standard,
        Start,
        Goal
    }

    [SerializeField] private WaypointTextColor waypointTextColor = WaypointTextColor.Yellow;
    [SerializeField] private WaypointPropList waypointType = WaypointPropList.Standard;
    [SerializeField] private List<VisGraphConnection> connections = new List<VisGraphConnection>();

    [Header("Debug Display")]
    [SerializeField] private bool displayText = true;
    [SerializeField] private float labelHeight = 3f;
    [SerializeField] private int labelFontSize = 9;

    public List<VisGraphConnection> Connections
    {
        get { return connections; }
    }


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        bool selected = Selection.activeGameObject == gameObject;
#else
        const bool selected = false;
#endif

        DrawWaypointAndConnections(selected);

#if UNITY_EDITOR
        if (displayText)
        {
            DrawCleanLabel(selected);
        }
#endif
    }

#if UNITY_EDITOR
    private void DrawCleanLabel(bool selected)
    {
        string infoText =
            $"{waypointType}\n" +
            $"{gameObject.name}\n" +
            $"{connections.Count}";

        GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
        style.normal.textColor = selected ? Color.white : GetTextColor();
        style.fontSize = labelFontSize;
        style.fontStyle = FontStyle.Normal;
        style.alignment = TextAnchor.MiddleCenter;

        Vector3 labelPosition = transform.position + Vector3.up * labelHeight;

        Handles.color = selected ? Color.white : GetTextColor();
        Handles.DrawLine(transform.position, labelPosition);
        Handles.Label(labelPosition, infoText, style);
    }
#endif

    private void DrawWaypointAndConnections(bool selected)
    {
        Color waypointColor = selected ? Color.red : GetWaypointColor();
        Color arrowHeadColor = selected ? Color.magenta : Color.blue;

        Gizmos.color = waypointColor;
        Gizmos.DrawSphere(transform.position, selected ? 0.45f : 0.01f);

        for (int i = 0; i < connections.Count; i++)
        {
            VisGraphConnection connection = connections[i];

            if (connection == null || connection.ToNode == null)
            {
                continue;
            }

            if (connection.ToNode == gameObject)
            {
                continue;
            }

            Vector3 direction = connection.ToNode.transform.position - transform.position;

            if (direction.sqrMagnitude < 0.001f)
            {
                continue;
            }

            DrawConnection(i, transform.position, direction, arrowHeadColor);

            if (selected)
            {
                Gizmos.color = arrowHeadColor;

                float dist = direction.magnitude;
                Vector3 dir = direction.normalized;

                Gizmos.DrawSphere(transform.position + dir * (dist * 0.1f), 0.3f);
                Gizmos.DrawSphere(transform.position + dir * (dist * 0.2f), 0.3f);
                Gizmos.DrawSphere(transform.position + dir * (dist * 0.3f), 0.3f);
            }
        }
    }

    private Color GetWaypointColor()
    {
        switch (waypointType)
        {
            case WaypointPropList.Start:
                return Color.green;

            case WaypointPropList.Goal:
                return Color.red;

            case WaypointPropList.Standard:
            default:
                return Color.yellow;
        }
    }

    private Color GetTextColor()
    {
        switch (waypointTextColor)
        {
            case WaypointTextColor.Blue:
                return Color.blue;

            case WaypointTextColor.Cyan:
                return Color.cyan;

            case WaypointTextColor.Yellow:
                return Color.yellow;

            default:
                return Color.white;
        }
    }

    private void DrawConnection(
        int connectionIndex,
        Vector3 position,
        Vector3 direction,
        Color arrowHeadColor,
        float arrowHeadLength = 0.5f,
        float arrowHeadAngle = 40.0f)
    {
        Debug.DrawRay(position, direction, Color.blue);

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 right =
            lookRotation *
            Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
            Vector3.forward;

        Vector3 left =
            lookRotation *
            Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
            Vector3.forward;

        Vector3 arrowPosition =
            position +
            direction.normalized +
            direction.normalized * (0.1f * connectionIndex);

        Debug.DrawRay(arrowPosition, right * arrowHeadLength, arrowHeadColor);
        Debug.DrawRay(arrowPosition, left * arrowHeadLength, arrowHeadColor);
    }
}