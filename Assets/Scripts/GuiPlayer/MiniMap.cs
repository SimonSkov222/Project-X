using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {


    public Transform player;
    
	// Update is called once per frame
	void Update () {

        Vector3 pos = player.position;
        pos.y = transform.position.y;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
	}
}
