using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "New Gun")]
public class Guns : ScriptableObject {

    public string Name { get; set; }
    public GameObject gun;
    public string description;
    public Sprite weaponPicture;

    public int wShots;
    public int wAmmo;
    public float wReloadSpeed;
    public float wFireRate;
    public float wWeaponRange;

    public float bDmg;
    public float bRange;
    public float bSpeed;
    
    

}
