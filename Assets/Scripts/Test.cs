using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 test = new Vector2(3, 9);
        Vector2 test2 = new Vector2(2, 1);
        Vector2 test3 = Vector2.Scale(test, test2);

        //Debug.Log("X: " + test3.x + " Y: " + test3.y);

    }
}
