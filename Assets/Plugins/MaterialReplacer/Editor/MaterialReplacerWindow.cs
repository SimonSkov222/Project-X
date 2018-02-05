using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Denne klasse bruges til at flytte "Bullet"(gameobject).
//  Når "Bullet"(gamepbject) rammer et andet object
//   kalder vi metoden OnGameObjectEnter() på det object
//   vi ramte.
//  
////////////////////////////////////////////////////////////////////
public class MaterialReplacerWindow : EditorWindow {

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region

    private List<Material> m_find;
    private List<Material> m_replace;

    private List<int> m_id;

    private Vector2 scrollPos = new Vector2(0,0);

    private bool child;
    private int count = 1;

    #endregion

    ///////////////////////////////
    //      Public Static Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Åbner vores vindue
    /// </summary>
    [MenuItem("Window/Material Replacer")]
    public static void ShownWindow()
    {
        GetWindow<MaterialReplacerWindow>().Show();
    }

    /// <summary>
    /// 
    /// </summary>
    public static void ReplaceMaterial(GameObject go, Material find, Material replace, bool includeChild = true)
    {

        SetReplaceMaterial(go, find, replace);
        if (includeChild)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                ReplaceMaterial(child, find, replace);
            }
        }

    }
    #endregion

    ///////////////////////////////
    //      Private Static Methods
    ///////////////////////////////
    #region


    private static Material GetMaterial(GameObject gameObject)
    {
        if (gameObject.GetComponent<Renderer>() != null)
        {
            return gameObject.GetComponent<Renderer>().sharedMaterial;
        }
        return null;
    }

    private static bool HasMaterial(GameObject go, Material material)
    {
        return GetMaterial(go) == material;
    }

    private static void SetReplaceMaterial(GameObject go, Material find, Material replace)
    {
        if (HasMaterial(go, find))
        {
            go.GetComponent<Renderer>().sharedMaterial = replace;
        }
    }

    #endregion

    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////
    #region

    void OnGUI()
    {
        

        EditorGUILayout.Separator();

        child = GUILayout.Toggle(child, "Child");
        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int a = 0; a < count; a++)
        {
            if (a == m_find.Count)
            {
                m_find.Add(null);
                m_replace.Add(null);
            }

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
            count++;
        }




    }

    #endregion

}
