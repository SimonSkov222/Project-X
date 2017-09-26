using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class HealthBar : MonoBehaviour {

    //public GameObject healthBar;
    public Slider slider;
    Vector3 pos = Vector3.zero;
    public float height;

    BoxCollider boxCollider;

    Health hp;

	// Use this for initialization
	void Start ()
    {
        hp = GetComponent<Health>();
        slider.maxValue = hp.HP;
        //boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update ()
    {
        
        slider.transform.LookAt(Camera.main.transform);
        //if (InCombat)
        //{
        HealthLeft();
        if (hp == null || slider == null)
        {
            //Debug.Log("null");
        }
        //}
        
    }
    
    private void HealthLeft()
    {
        pos = gameObject.transform.position;
        //pos.y += (boxCollider.size.y * gameObject.transform.localScale.y) + (slider.GetComponent<RectTransform>().sizeDelta.y * 2 / 100);
        //Debug.Log("bogstav " + slider.GetComponent<RectTransform>().sizeDelta.y);
        //Debug.Log("1 " + pos);
        //Debug.Log("2 " + slider.transform.position);

        pos.y = height;

        slider.transform.position = pos;

        slider.value = hp.currentHP;
    }


}
