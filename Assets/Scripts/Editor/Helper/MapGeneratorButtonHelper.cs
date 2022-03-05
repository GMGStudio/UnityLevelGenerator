using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapObjectGenerator))]
public class MapGeneratorButtonHelper : Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapObjectGenerator myScript = (MapObjectGenerator)target;

        if (GUILayout.Button("Generate Map"))
        {
            myScript.DeleteObjects();
            myScript.Initialize();
            myScript.GenerateMap();
        }
        if (GUILayout.Button("Delete Objects"))
        {
            myScript.DeleteObjects();
        }
        if (GUILayout.Button("Add Objects"))
        {
            myScript.Initialize();
            myScript.GenerateMap();
        }
    }
}