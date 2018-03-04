using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBasic : ScriptableObject
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
    public bool IsReloading { get { return reloadCoroutine != null; } }
    public float FireRate { get { return fireRate; } }

    private float timeFire = 0;

    protected GameObject player;

    private Coroutine reloadCoroutine;


    public void OnFire_Click()
    {
        OnFire_Activate(ButtonCall.Click);
    }

    public void OnFire_Down()
    {
        OnFire_Activate(ButtonCall.Down);
    }

    public void OnFire_Hold()
    {
        OnFire_Activate(ButtonCall.Hold);
    }

    public void OnFire_Up()
    {
        OnFire_Activate(ButtonCall.Up);
    }


    public void OnReload_Down()
    {
        if (reloadCoroutine == null)
        {
            reloadCoroutine = player.GetComponent<PlayerController>().StartCoroutine(ReloadWeapon());
        }
    }

    public void CancelReload()
    {
        player.GetComponent<PlayerController>().StopCoroutine(reloadCoroutine);
        reloadCoroutine = null;
        OnReloadCancel();
    }

    public virtual void OnFire_Activate(ButtonCall arg1)
    {
        if (arg1 == ButtonCall.Hold)
        {
            if (Time.time - timeFire  >= FireRate)
            { 
                timeFire = Time.time;
                OnFire();
            }
        }
    }



    protected IEnumerator ReloadWeapon()
    {
        OnReloadStart();
        yield return new WaitForSeconds(reloadTime);
        OnReloadEnd();
        reloadCoroutine = null;
    }

    protected virtual void OnReloadStart()
    {
    }

    protected virtual void OnReloadEnd()
    {
    }
    protected virtual void OnReloadCancel()
    {
    }

    public virtual void OnLoaded(GameObject characterGo)
    {
        player = characterGo;
        timeFire = Time.time;
    }
    public abstract void OnFire();
}
