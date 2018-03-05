using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  I denne klasse henter vi hp og ammo fra health 
//  og weapon klasserne og gemmer dem i en string
//  med noget tekst og udskriver dem i vores Ui/canvas
//  
//////////////////////////////////////////////////////

public class TextScript : MonoBehaviour {

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    public Text health_text;
    public Text ammo_text;


    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private Health health;
    private Weapon ammo;

    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////

    /// <summary>
    /// Henter Health og Weapon components fra parent, så vi ved hvor meget HP eller ammo vi har.
    /// </summary>
    void Start ()
    {
        health = GetComponentInParent<Health>();
        ammo = GetComponentInParent<Weapon>();
        
    }
	
	/// <summary>
    /// laver 2 strings som vi så udskriver i en canvas
    /// </summary>
	void Update ()
    {
        return;
        string allHealth = health.currentHP.ToString() + "/" + health.maxHP.ToString();
        health_text.text = allHealth;
        string allAmmo = ammo.CurrentShots.ToString() + "/" + ammo.CurrentAmmo.ToString();
        ammo_text.text = allAmmo;

    }
}
