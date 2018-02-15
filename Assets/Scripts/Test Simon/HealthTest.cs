using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTest : MonoBehaviour, HealthNew {

    public int m_HealthMax;
    public int m_Health;


    public int HealthMax { get; set; }
    public int Health { get; set; }

    public void OnDeath()
    {
       
    }
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
