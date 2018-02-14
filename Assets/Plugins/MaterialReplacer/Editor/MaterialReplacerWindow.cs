using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Den her klasse gør det muligt for os at
//  ændre materialer på mange forskellige objecter
//  på samme tid og på en nem måde.
//  
//////////////////////////////////////////////////////
public class MaterialReplacerWindow : EditorWindow
{

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private List<Material> m_find;
    private List<Material> m_replace;
    private List<int> m_id;
    private Vector2 scrollPos = new Vector2(0, 0);
    bool child;
    int count = 1;


    /// <summary>
    /// Viser vinduet.
    /// </summary>
    [MenuItem("Window/Material Replacer")]
    public static void ShownWindow()
    {
        GetWindow<MaterialReplacerWindow>().Show();
    }

    /// <summary>
    /// Her laver vi layout på vores vindue
    /// og fortæller hvad de forskellige ting skal gøre
    /// </summary>
    void OnGUI()
    {

        // Laver et lille mellemrum
        EditorGUILayout.Separator();

        // Er en checkbox som checker om vi skal have children med
        child = GUILayout.Toggle(child, "Child");


        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int a = 0; a < m_find.Count; a++)
        {
            //if (a == m_find.Count)
            //{
            //    m_find.Add(null);
            //    m_replace.Add(null);
            //}

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            m_find[a] = (Material)EditorGUILayout.ObjectField("Find", m_find[a], typeof(Material), false);
            m_replace[a] = (Material)EditorGUILayout.ObjectField("Replace", m_replace[a], typeof(Material), false);

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (GUILayout.Button("Switch"))
            {
                Material temp = m_find[a];
                m_find[a] = m_replace[a];
                m_replace[a] = temp;
            }

            if (GUILayout.Button("Apply") && m_find[a] != null && m_replace[a] != null)
            {
                foreach (var go in Selection.gameObjects)
                {
                    ReplaceMaterial(go, m_find[a], m_replace[a], child);

                }
            }

            EditorGUILayout.Separator();

            if (GUILayout.Button("Remove"))
            {
                count--;
                m_find.RemoveAt(a);
                m_replace.RemoveAt(a);
                a--;
                continue;
            }


        }


        EditorGUILayout.EndScrollView();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (GUILayout.Button("Add"))
        {
            m_find.Add(null);
            m_replace.Add(null);
        }




    }
    /// <summary>
    /// Leder efter et material og udskifter den med noget andet material.
    /// Man kan vælge at den også skal tjekke child igennem.
    /// </summary>
    public static void ReplaceMaterial(GameObject go, Material find, Material replace, bool includeChild = true)
    {

        SetReplaceMaterial(go, find, replace);
        if (includeChild)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                ReplaceMaterial(child, find, replace, includeChild);

            }
        }

    }

    /// <summary>
    /// Henter material fra gameobject.
    /// </summary>
    private static Material GetMaterial(GameObject gameObject)
    {
        if (gameObject.GetComponent<Renderer>() != null)
        {
            return gameObject.GetComponent<Renderer>().sharedMaterial;
        }
        return null;
    }

    /// <summary>
    /// Tjekker om gameobjectet har et material.
    /// </summary>
    private static bool HasMaterial(GameObject go, Material material)
    {
        return GetMaterial(go) == material;
    }

    /// <summary>
    /// Leder efter et material og udskifter den med noget andet material.
    /// </summary>
    private static void SetReplaceMaterial(GameObject go, Material find, Material replace)
    {
        if (HasMaterial(go, find))
        {
            go.GetComponent<Renderer>().sharedMaterial = replace;
        }
    }

}
