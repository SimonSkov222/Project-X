using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
////////////////////////////////////////////////////////////////////
//                  Beskrivelse
//
//      Tilføjer en knap til gameobject som har RandomHouseColorParent component,
//      når man trykke på knappen skifter den farverne.
//
////////////////////////////////////////////////////////////////////

[CustomEditor(typeof(RandomHouseColorParent))]
public class RandomHouseColorEditor : Editor {

/// <summary>
/// Tilføjer en knap, når man trykker på den skifter den farverne.
/// </summary>
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
        ((RandomHouseColorParent)target).ChangeColorByRandom();

    }
        
}




}
