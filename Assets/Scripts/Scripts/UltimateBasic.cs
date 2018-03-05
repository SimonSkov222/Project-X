using UnityEngine;


//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Hvad en Ultimate evne skal indeholde og alle ultimate
//  evner skal arve fra denne classe
//
//////////////////////////////////////////////////////
public abstract class UltimateBasic : ScriptableObject
{
    ///////////////////////////////
    //      Protected Fields
    ///////////////////////////////
    #region
    [Header("Ultimate Charge")]
    [SerializeField]
    [Range(0, 1000)]
    protected int chargeByDmg = 0;
    [SerializeField]
    [Range(0, 1000)]
    protected int chargeByHealing = 0;
    [SerializeField]
    [Range(0, 1000)]
    protected int chargeByWait = 0;
    [SerializeField]
    [Range(0, 1000)]
    protected int chargeByBlock = 0;

    [SerializeField]
    [Range(0, 10000)]
    protected int chargeNeed = 0;

    protected int chargeHas = 0;

    [SerializeField]
    [Range(0, 120)]
    protected int time = 0;
    #endregion
    
    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    #region

    public bool CanUseUltimate { get { return chargeHas >= chargeNeed; } }

    #endregion

    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////
    #region
    /// <summary>
    /// Metoden der vil blive kaldt når evnen skal aktiveres
    /// </summary>
    public abstract void OnActivate();

    /// <summary>
    /// Metoden der kan sættes på en knap
    /// </summary>
    public void OnButton_Down()
    {
        OnButton_Activate(ButtonCall.Down);
    }

    
    #endregion

    ///////////////////////////////
    //      Protected Methods
    ///////////////////////////////
    #region
    /// <summary>
    /// Når knappen der aktivere evnen er blevet trykket på bliver denne metode kaldt
    /// som default vil aktivere evnen (kan overskrives)
    /// </summary>
    protected virtual void OnButton_Activate(ButtonCall arg1)
    {
        OnActivate();
    }
    #endregion
    
}
