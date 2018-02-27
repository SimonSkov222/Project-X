using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour {


    public Swords sword;


	// Use this for initialization
	void Start () {

        var iSword = Instantiate(sword.sword);


        iSword.transform.parent = transform;
        iSword.transform.position = transform.position;
        iSword.transform.rotation = transform.rotation;
        


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
