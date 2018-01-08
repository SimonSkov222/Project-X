using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtMaterialReplacer : EditorWindow {

    List<Material> m_find;
    List<Material> m_replace;

    List<int> m_id;

    Vector2 scrollPos = new Vector2(0,0);

    bool child;
    int count = 1;

    [MenuItem("Window/Ext Material Replacer")]
    public static void ShownWindow()
    {
        GetWindow<ExtMaterialReplacer>().Show();
    }


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
                    ReplaceMaterial(go, m_find[a], m_replace[a]);
                    if (child)
                    {
                        for (int i = 0; i < go.transform.childCount; i++)
                        {
                            GameObject child = go.transform.GetChild(i).gameObject;
                            ReplaceMaterial(child, m_find[a], m_replace[a]);
                        }
                    }

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


    private Material GetMaterial(GameObject gameObject)
    {
        if (gameObject.GetComponent<Renderer>() != null)
        {
            return gameObject.GetComponent<Renderer>().sharedMaterial;
        }
        return null;
    }

    private bool HasMaterial(GameObject go, Material material)
    {
        return GetMaterial(go) == material;
    }

    private void ReplaceMaterial(GameObject go, Material find, Material replace)
    {
        if (HasMaterial(go, find))
        {
            go.GetComponent<Renderer>().sharedMaterial = replace;
        }
    }


    




}
