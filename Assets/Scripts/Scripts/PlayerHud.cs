using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHud : NetworkBehaviour {

    private Canvas canvas;

	// Use this for initialization
	void Start () {
        canvas = GetComponent<Canvas>();
        if (isLocalPlayer)
        {
            canvas.enabled = true;

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
