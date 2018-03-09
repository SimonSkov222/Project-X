using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public string[] MapNames;

    private int characterID = 0;
    private int mapID = 0;
    

    public void LoadScene()
    {
        switch (mapID)
        {
            default: SceneManager.LoadScene(MapNames[mapID]);  break;
        }
    }
}
