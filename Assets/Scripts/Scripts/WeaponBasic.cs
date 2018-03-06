using System.Collections;
using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Hvad et våben skal indeholde og alle våben
//  skal arve fra denne classe
//
//////////////////////////////////////////////////////
public abstract class WeaponBasic : ScriptableObject
{
    ///////////////////////////////
    //      Protected Fields
    ///////////////////////////////
    #region
    [SerializeField]
    [Range(0, 1000)]
    protected int ammoMax;

    [SerializeField]
    [Range(0, 100)]
    protected float reloadTime;

    [SerializeField]
    [Range(0, 100)]
    protected float fireRate;

    [SerializeField]
    [Range(1, 100)]
    protected int damage;

    protected GameObject player;
    #endregion

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region

    private float timeFire = 0;
    private Coroutine reloadCoroutine;

    #endregion

    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    #region
    public int Ammo { get; set; }

    public bool IsInfinityAmmo { get { return ammoMax == 0; } }
    public bool IsReloading { get { return reloadCoroutine != null; } }
    public float FireRate { get { return fireRate; } }
    #endregion

    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////
    #region
    public abstract void OnFire();
    /// <summary>
    /// Bliver kaldt når objecet loades
    /// kan overskrives
    /// </summary>
    public virtual void OnLoaded(GameObject characterGo)
    {
        player = characterGo;
        timeFire = Time.time;
    }
    /// <summary>
    /// bliver kaldt når man trykker på affyre knappen
    /// og kalder en anden metode der kan overskrives
    /// </summary>
    public void OnFire_Click()
    {
        OnFire_Activate(ButtonCall.Click);
    }

    /// <summary>
    /// bliver kaldt når man trykker på affyre knappen
    /// og kalder en anden metode der kan overskrives
    /// </summary>
    public void OnFire_Down()
    {
        OnFire_Activate(ButtonCall.Down);
    }

    /// <summary>
    /// bliver kaldt når man trykker på affyre knappen
    /// og kalder en anden metode der kan overskrives
    /// </summary>
    public void OnFire_Hold()
    {
        OnFire_Activate(ButtonCall.Hold);
    }

    /// <summary>
    /// bliver kaldt når man trykker på affyre knappen
    /// og kalder en anden metode der kan overskrives
    /// </summary>
    public void OnFire_Up()
    {
        OnFire_Activate(ButtonCall.Up);
    }

    /// <summary>
    /// bliver kaldt når man trykker på affyre knappen.
    /// Default er at hvis man holder knappen nede bliver
    /// OnFire() kaldt
    /// 
    /// kan overskrives
    /// </summary>
    public virtual void OnFire_Activate(ButtonCall arg1)
    {
        if (arg1 == ButtonCall.Hold)
        {
            if (Time.time - timeFire >= FireRate)
            {
                timeFire = Time.time;
                OnFire();
            }
        }
    }

    /// <summary>
    /// bliver kaldt når man trykker på reload knappen
    /// og starter reload af våbnet
    /// </summary>
    public void OnReload_Down()
    {
        if (reloadCoroutine == null)
        {
            reloadCoroutine = player.GetComponent<PlayerController>().StartCoroutine(ReloadWeapon());
        }
    }

    /// <summary>
    /// annuller reloade og kalder OnReloadCancel()
    /// </summary>
    public void CancelReload()
    {
        player.GetComponent<PlayerController>().StopCoroutine(reloadCoroutine);
        reloadCoroutine = null;
        OnReloadCancel();
    }


    #endregion
    ///////////////////////////////
    //      Protected Methods
    ///////////////////////////////
    #region
    /// <summary>
    /// kalder OnReloadStart() og venter noget tid(reloadTime) 
    /// hvorefter OnReloadEnd() bliver kaldt
    /// </summary>
    protected IEnumerator ReloadWeapon()
    {
        OnReloadStart();
        yield return new WaitForSeconds(reloadTime);
        OnReloadEnd();
        reloadCoroutine = null;
    }
    
    protected virtual void OnReloadStart() { }
    protected virtual void OnReloadEnd() { }
    protected virtual void OnReloadCancel() { }
    #endregion






}
