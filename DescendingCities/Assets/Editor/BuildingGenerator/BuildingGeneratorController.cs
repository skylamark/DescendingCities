using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*[CustomEditor(typeof(BuildingGeneratorz))]
public class BuildingGeneratorController : Editor
{
    public bool showHouses;

    public override void OnInspectorGUI()
    {
        BuildingGenerator buildingGenerator = (BuildingGenerator)target;
        EditorGUILayout.LabelField("Cubes Collected:", buildingGenerator.cubeHouses.Length.ToString());

        if (GUILayout.Button("Collect CubeHouses"))
        {
            buildingGenerator.CollectAllCubes();
        }
        if (GUILayout.Button("Generate Houses"))
        {
            buildingGenerator.ConvertToBuilding();
        }

        if (showHouses)
        {
            if (GUILayout.Button("Show Cubes"))
            {
                showHouses = false;
            }
        } else {
            if (GUILayout.Button("Show Houses"))
            {
                showHouses = true;
            }
        }

        //base.OnInspectorGUI();
    }


}*/
