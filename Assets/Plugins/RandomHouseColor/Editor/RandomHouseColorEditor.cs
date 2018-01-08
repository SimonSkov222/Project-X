using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomHouseColorParent))]
public class RandomHouseColorEditor : Editor {

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("I am a button"))
        {

            Debug.Log("hej");
            ((RandomHouseColorParent)target).NoName();

        }
        
    }




}
