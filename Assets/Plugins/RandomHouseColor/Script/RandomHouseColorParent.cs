using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomHouseColorParent : MonoBehaviour {

    public Material[] m_colors;


    

    public void NoName()
    {
        GameObject[] houses = new GameObject[transform.childCount];

        for (int i = 0; i < houses.Length; i++)
        {

            houses[i] = transform.GetChild(i).gameObject;
        }

        foreach (var house in houses)
        {
            Material color = m_colors[Random.Range(0, m_colors.Length)];
            

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
    

}
