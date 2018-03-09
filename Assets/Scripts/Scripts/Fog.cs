using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour {

    void OnEnable()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.HSVToRGB(0.5556f, 0.7021f, 0.9216f);
        RenderSettings.fogDensity = 0.2f;
    }
    void OnDisable()
    {
        RenderSettings.fog = false;
    }
    
    


}
