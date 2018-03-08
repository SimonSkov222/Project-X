using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            NetworkManager.singleton.GetComponent<MultiplayerManager>().selectedCharacter = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NetworkManager.singleton.GetComponent<MultiplayerManager>().selectedCharacter = 1;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            NetworkManager.singleton.GetComponent<MultiplayerManager>().StartHost();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            NetworkManager.singleton.GetComponent<MultiplayerManager>().StartClient();
        }
    }
}
