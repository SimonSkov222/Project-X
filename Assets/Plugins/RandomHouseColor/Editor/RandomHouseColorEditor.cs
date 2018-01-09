using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomHouseColorParent))]
public class RandomHouseColorEditor : Editor {

    public override void OnInspectorGUI()
    {
        var controller = target as RandomHouseColorParent;
        EditorGUIUtility.LookLikeInspector();
        SerializedProperty tps = serializedObject.FindProperty("m_colors");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(tps, true);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
        EditorGUIUtility.LookLikeControls();


        //((RandomHouseColorParent)target).m_colors = (Material[])EditorGUILayout.ObjectField("", ((RandomHouseColorParent)target).m_colors, typeof(Material[]), false);

        if (GUILayout.Button("I am a button"))
        {

            Debug.Log("hej");
            ((RandomHouseColorParent)target).NoName();

        }
        
    }




}
