using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexGrid))]
public class EditorButtonLayout :  Editor
{


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        HexGrid hexGrid = (HexGrid)target;
            if (GUILayout.Button("Layout"))
            {
            hexGrid.Layout();
            }

        }
    }
