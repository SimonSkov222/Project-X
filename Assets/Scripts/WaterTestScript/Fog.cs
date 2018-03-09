using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour {

    private Water water;

    // Use this for initialization
    void Start () {
		
        


	}
	
	// Update is called once per frame
	void Update () {

        water = GetComponentInParent<Water>();

        if (water.isUnderWater == true)
        {
            BlueFog();
        }
        else
        {
            RenderSettings.fog = false;
        }
    }


    private void BlueFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.blue;
        RenderSettings.fogDensity = 0.2f;
    }


}
