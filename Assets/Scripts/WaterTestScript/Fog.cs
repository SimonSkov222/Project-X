using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour {
    


    // Use this for initialization
    void Start () {}
    void OnEnable()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.blue;
        RenderSettings.fogDensity = 0.2f;
    }
    void OnDisable()
    {
        RenderSettings.fog = false;
    }
    

    private void BlueFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.blue;
        RenderSettings.fogDensity = 0.2f;
    }


}
