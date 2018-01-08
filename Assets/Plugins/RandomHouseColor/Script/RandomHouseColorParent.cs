using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomHouseColorParent : MonoBehaviour {

    public Material[] m_colors;


    

    public void NoName()
    {
        if (m_colors.Length == 0)
        {
            return;
        }

        GameObject[] houses = new GameObject[transform.childCount];

        for (int i = 0; i < houses.Length; i++)
        {

            houses[i] = transform.GetChild(i).gameObject;
        }

        foreach (var house in houses)
        {
            
            Material replace = m_colors[Random.Range(0, m_colors.Length)];

            foreach (var find in m_colors)
            {
                ReplaceMaterial(house, find, replace);
            }

            //Debug.Log(house.transform.position);
            //var hits = Physics.RaycastAll(house.transform.position, -house.transform.right, 2);
            //if (hits.Length > 0)
            //{
            //    Debug.Log("true");

            //    foreach (var item in hits)
            //    {
            //        Debug.Log(item.collider.transform.position);
            //    }
            //    break;
            //}
            
        }

    }

    public static void ReplaceMaterial(GameObject go, Material find, Material replace, bool includeChild = true)
    {

        SetReplaceMaterial(go, find, replace);
        if (includeChild)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                SetReplaceMaterial(child, find, replace);
            }
        }

    }

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


}
