using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingGenerator))]
[CanEditMultipleObjects]
public class BuildingGeneratorController : Editor
{
    public override void OnInspectorGUI()
    {
        BuildingGenerator buildingGenerator = (BuildingGenerator)target;
        EditorGUILayout.LabelField("Cubes Collected:", buildingGenerator.cubeHouses.Length.ToString());
        EditorGUILayout.LabelField("Cubes Stored:", buildingGenerator.storedCubes.Length.ToString());
        EditorGUILayout.LabelField("Homes Stored:", buildingGenerator.storedHomes.Length.ToString());

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Collect CubeHouses", GUILayout.Width(130f)))
        {
            buildingGenerator.CollectAllCubes();
        }
        if (GUILayout.Button("Clear Selection", GUILayout.Width(130f)))
        {
            buildingGenerator.ClearSelectedCubes();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Houses", GUILayout.Width(130f)))
        {
            buildingGenerator.ConvertToBuilding();
        }
        
        if (GUILayout.Button("Swith Visual", GUILayout.Width(130f)))
        {
            buildingGenerator.SwitchVisible();
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Destroy Generated Homes", GUILayout.Width(265f)))
        {
            buildingGenerator.DeleteGeneratedHouses();
        }
        base.OnInspectorGUI();
    }

}
