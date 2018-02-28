using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour {


    public Rigidbody cube;

	// Use this for initialization
	void Start () {
        cube = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        cube.AddForce(transform.forward * 1);
	}
}
