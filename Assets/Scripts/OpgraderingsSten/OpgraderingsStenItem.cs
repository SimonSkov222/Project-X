using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OpgraderingsStenItem : MonoBehaviour {

    private Image itemImage;
    public Sprite stenImage;
    public OpgraderingsSten sten;

    private WeaponBasic weaponBasic;

	// Use this for initialization
	void Start () {
        weaponBasic = GetComponentInParent<WeaponBasic>();
        itemImage = GetComponent<Image>();

	}
	
	// Update is called once per frame
	void Update () {

        weaponBasic.addOpgraderingsSten(sten);
        
	}
}
