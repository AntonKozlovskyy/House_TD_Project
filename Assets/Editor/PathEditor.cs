using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathEditor : EditorWindow 
{
    List<GameObject> wayPoints = new List<GameObject>();
    GameObject wayPoint;
    GameObject lastWayPoint;

    [MenuItem("Game Editors/Path Editor")]
    static void Init()
    {
        PathEditor window = (PathEditor)EditorWindow.GetWindow(typeof(PathEditor));
    }

    void Update()
    {
        wayPoints = GameObject.FindGameObjectsWithTag("WayPoint").ToList();
        if(wayPoint == null)
            wayPoint = (GameObject)Resources.LoadAssetAtPath("Assets/Prefabs/Waypoint.prefab", typeof(GameObject));

        Repaint();
    }
    void OnGUI()
    {
        GUILayout.Label("Waypoints on level: " + wayPoints.Count);
        if (GUILayout.Button("Add Waypoint"))
        {
            lastWayPoint = (GameObject)Instantiate(wayPoint);
        }
        if (Selection.activeGameObject != null)
        {
            if (GUILayout.Button("Connect last Waypoint to: " + Selection.activeObject.name))
            {
                Selection.activeGameObject.GetComponent<Monster>().path.Add(lastWayPoint);
            }
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Delete all Waypoints"))
        {
            for (int i = 0; i < wayPoints.Count; i++)
                DestroyImmediate(wayPoints[i]);
        }
    }
}
