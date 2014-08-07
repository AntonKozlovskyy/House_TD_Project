using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathEditor : EditorWindow 
{
    List<GameObject> wayPoints = new List<GameObject>();
    GameObject wayPoint, wayPointParent;
    GameObject lastWayPoint;
    GameObject[] selectedPoints;
    GameObject enemy;

    [MenuItem("Game Editors/Path Editor")]
    static void Init()
    {
        PathEditor window = (PathEditor)EditorWindow.GetWindow(typeof(PathEditor));
    }

    void Update()
    {
        //все вейпоинты на сцене
        wayPoints = GameObject.FindGameObjectsWithTag("WayPoint").ToList();

        //находим префаб вейпоинта
        if(wayPoint == null)
            wayPoint = (GameObject)Resources.LoadAssetAtPath("Assets/Prefabs/Waypoint.prefab", typeof(GameObject));

        //находим или создаем родителя для вейпоинтов
        if (wayPointParent == null)
        {
            if (GameObject.FindGameObjectWithTag("WaypointParent"))
                wayPointParent = GameObject.FindGameObjectWithTag("WaypointParent");
            else
            {
                wayPointParent = new GameObject("Waypoints");
                wayPointParent.tag = "WaypointParent";
            }
        }

        //все выделенные вейпоинты, для связки с монстром
        selectedPoints = Selection.gameObjects;
        for (int i = 0; i < selectedPoints.Length; i++)
        {
            //находим монстра из выделения
            if (selectedPoints[i].tag == "Enemy")
            {
                enemy = selectedPoints[i];
                break;                   
            }
        }
        
        //если мы нашли монстра среди выделения
        if(enemy != null)
            //сортируем вейпоинты по расстоянию к монстру
            selectedPoints = selectedPoints.OrderBy(x => Vector3.Distance(enemy.transform.position, x.transform.position)).ToArray();

        //обновляем гуи
        Repaint();
    }
    void OnGUI()
    {
        GUILayout.Label("Waypoints on level: " + wayPoints.Count);
        
        //создает новый вейпоинт на уровне
        if (GUILayout.Button("Add Waypoint"))
        {
            lastWayPoint = (GameObject)Instantiate(wayPoint);
            lastWayPoint.transform.parent = wayPointParent.transform;
        }
        
        //если выбран один монстр, тогда коннектим последний вейпоинт к этому монстру
        if (Selection.activeGameObject != null && Selection.gameObjects.Length == 1)
        {
            if (Selection.activeGameObject.tag == "Enemy")
            {
                if (GUILayout.Button("Connect last Waypoint to: " + Selection.activeObject.name))
                {
                    Selection.activeGameObject.GetComponent<Monster>().path.Add(lastWayPoint);
                }
            }
        }

        //если выбрана куча вейпоинтов и монстр, то коннектим все вейпоинты к монстру
        if (Selection.gameObjects.Length > 1 && enemy != null)
        {
            if (GUILayout.Button("Connect all Waypoints to: " + enemy.name))
            {
                for(int i = 0; i < selectedPoints.Length; i++)
                    if(selectedPoints[i].tag != "Enemy")
                        enemy.GetComponent<Monster>().path.Add(selectedPoints[i]);
            }
        }
        GUILayout.FlexibleSpace();

        //удаляет все вейпоинты с уровня, и очищает все связи с монстрами
        if (GUILayout.Button("Delete all Waypoints"))
        {
            for (int i = 0; i < wayPoints.Count; i++)
                DestroyImmediate(wayPoints[i]);
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
                GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Monster>().path.Clear();
        }
    }
}
