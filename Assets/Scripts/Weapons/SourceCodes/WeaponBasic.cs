using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBasic : ScriptableObject, IButton
{
    [SerializeField]
    protected GameObject model;

    [SerializeField]
    [Range(0, 1000)]
    protected int ammoMax;

    [SerializeField]
    [Range(0, 100)]
    protected float reloadTime;

    [SerializeField]
    [Range(0, 100)]
    protected float fireRate;

    public int Ammo { get; set; }
    public bool IsInfinityAmmo { get { return ammoMax == 0; } }


    public virtual void OnReloadStart(GameObject player, object weapon)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnReloadEnd(GameObject player, object weapon)
    {
        throw new System.NotImplementedException();
    }
    public virtual void OnReloadCancel(GameObject player, object weapon)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnButtonClick(GameObject player, object weapon)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnButtonDown(GameObject player, object weapon)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnButtonHold(GameObject player, object weapon)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnButtonUp(GameObject player, object weapon)
    {
        throw new System.NotImplementedException();
    }
}
