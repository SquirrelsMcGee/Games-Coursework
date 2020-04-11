using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
        else if (GUILayout.Button("Remove Waypoint"))
        {
            RemoveWaypoint();
        }
        else if (GUILayout.Button("Complete Loop"))
        {
            CompleteLoop();
        }
    }

    private void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        if (waypointRoot.childCount > 1)
        {
            waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
            waypoint.previousWaypoint.nextWaypoint = waypoint;
            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }

        Selection.activeGameObject = waypoint.gameObject;
    }

    private void RemoveWaypoint()
    {
        
        if (waypointRoot.childCount == 0) return;

        GameObject waypointObject = waypointRoot.GetChild(waypointRoot.childCount - 1).gameObject;

        if (waypointObject != null)
        {
            DestroyImmediate(waypointObject);
        }
    }

    private void CompleteLoop()
    {
        if (waypointRoot.childCount == 0) return;
        GameObject waypointObject = waypointRoot.GetChild(waypointRoot.childCount - 1).gameObject;

        if (waypointObject != null)
        {
            waypointObject.GetComponent<Waypoint>().nextWaypoint = waypointRoot.GetChild(0).GetComponent<Waypoint>();
        }
    }
}
