using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "New Sword")]
public class Swords : ScriptableObject
{
    public string Name { get; set; }
    public GameObject sword;
    public string description;
    public Sprite swordPicture;

    public float speed;
    public float dmg;
    public float range;

    
}
