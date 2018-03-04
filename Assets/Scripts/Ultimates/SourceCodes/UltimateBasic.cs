using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UltimateBasic : ScriptableObject
{
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

    public bool CanUseUltimate { get { return chargeHas >= chargeNeed;  } }

    public void OnButton_Click()
    {
        OnButton_Activate(ButtonCall.Click);
    }


    protected virtual void OnButton_Activate(ButtonCall arg1)
    {
        OnActivate();
    }


    public abstract void OnActivate();
}
