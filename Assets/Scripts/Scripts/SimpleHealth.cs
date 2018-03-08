using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleHealth : MonoBehaviour, IHealth {


    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////

    public delegate void OnEventDelegate(GameObject sender);
    public delegate void OnDeathDelegate(GameObject sender, object obj);
    public delegate void OnDamageDelegate(GameObject sender, GameObject obj, int dmg);

    public event OnDeathDelegate EventOnDeath;
    public event OnDamageDelegate EventOnGiveDamage;
    public event OnDamageDelegate EventOnTakeDamage;
    public event OnEventDelegate EventOnEnable;


    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
   // public int HealthMax { get { return m_HealthMax; } }
    public int HealthMax { get; set ; }
    public int ArmorMax { get; set; }
    public int ShieldMax { get; set; }

    public int Health { get; set; }
    public int Armor { get; set; }
    public int Shield { get; set; }
    public int HealthBonus { get; set; }

    public float WeaknessMultiplier { get; set; }


    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////

    void OnEnable()
    {
        if (EventOnDeath != null)
        {
            EventOnEnable(gameObject);
        }
    }

    public void OnDeath(object sender)
    {
        if (EventOnDeath != null)
        {
            EventOnDeath(gameObject, sender);
        }
    }

    public void OnGiveDmg(GameObject target, int dmg)
    {
        if (EventOnDeath != null)
        {
            EventOnGiveDamage(gameObject, target, dmg);
        }
    }

    public void OnTakeDmg(GameObject sender, int dmg)
    {
        if (EventOnDeath != null)
        {
            EventOnTakeDamage(gameObject, sender, dmg);
        }
    }
}
