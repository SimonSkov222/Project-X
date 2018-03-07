using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuiPlayer : MonoBehaviour {

    public TextMeshProUGUI textAmmo;

    public GameObject player;

    public Weapon weapon;


    // Use this for initialization
    void Start () {

        weapon = player.GetComponent<Weapon>();

        Debug.Log(weapon.CurrentAmmo);
	}
	
	// Update is called once per frame
	void Update () {
        //textAmmo.text = weapon.CurrentAmmo.ToString();
	}
}
