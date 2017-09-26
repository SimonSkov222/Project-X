using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextScript : MonoBehaviour {
    
    public Text health_text;
    public Text ammo_text;

    Health health;
    Weapon ammo;

    // Use this for initialization
    void Start ()
    {
        health = GetComponentInParent<Health>();
        ammo = GetComponentInParent<Weapon>();
        
    }
	
	// Update is called once per frame
	void Update ()
    {

        string allHealth = health.currentHP.ToString() + "/" + health.HP.ToString();
        health_text.text = allHealth;
        string allAmmo = ammo.currentShots.ToString() + "/" + ammo.currentAmmo.ToString();
        ammo_text.text = allAmmo;

    }
}
