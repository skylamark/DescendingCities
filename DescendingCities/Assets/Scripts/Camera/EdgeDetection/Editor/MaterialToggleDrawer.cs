using UnityEngine;
using UnityEditor;
using System;

public class ToggleDrawer : MaterialPropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
        // Setup
        bool value = (Math.Abs(prop.floatValue) > float.Epsilon);

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = prop.hasMixedValue;

        // Show the toggle control
        value = EditorGUI.Toggle(position, label, value);

        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            // Set the new value if it has changed
            prop.floatValue = value ? 1.0f : 0.0f;
        }
    }
}