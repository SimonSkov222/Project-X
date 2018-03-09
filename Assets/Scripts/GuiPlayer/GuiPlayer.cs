using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GuiPlayer : MonoBehaviour {

    public TextMeshProUGUI textAmmo;
    public TextMeshProUGUI textHealth;

    public Image ultimate;
    public Image ability1;
    public Image ability2;
    public Image ability3;

    public Image buff1;
    public Image buff2;
    public Image buff3;

    private WeaponBasic weaponBasic;
    private PlayerController health;


    // Use this for initialization
    void Start () {
        
        health = GetComponent<PlayerController>();
        

	}
	
	// Update is called once per frame
	void Update () {

        //textAmmo.text = weaponBasic.Ammo.ToString();
        textHealth.text = health.Health.ToString();
            
        
	}
}
