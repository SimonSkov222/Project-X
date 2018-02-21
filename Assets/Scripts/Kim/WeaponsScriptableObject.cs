using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon",menuName = "New Weapons")]
public class Weapons : ScriptableObject {


    [Header("Standard")]
    public new string name;
    public GameObject weaponPrefab;
    public string description;
    public Sprite picture;

    [Header("Guns")]
    public int wShots;
    public int wAmmo;
    public float wReloadSpeed;
    public float wFireRate;
    public float wWeaponRange;

    public float bDmg;
    public float bRange;
    public float bSpeed;
    
    
    [Header("Swords")]
    public float speed;
    public float dmg;
    public float range;


}
