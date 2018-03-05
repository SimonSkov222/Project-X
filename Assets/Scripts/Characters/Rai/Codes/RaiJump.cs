﻿using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Får spilleren til at hoppe højt og giver skade til
//  fjener indefor en radius
//
//////////////////////////////////////////////////////
[CreateAssetMenu(fileName = "RaiJump", menuName = "Abilities/Rai/Jump")]
public class RaiJump : AbilityBasic
{

    ///////////////////////////////
    //      Protected Fields
    ///////////////////////////////
    #region
    [SerializeField]
    [Range(3, 20)]
    protected float junpHeight = 10f;

    [SerializeField]
    protected float dmgOnLand = 50f;

    protected bool isHitGroundActivated = false;
    #endregion
    
    ///////////////////////////////
    //      Public Properties 
    ///////////////////////////////
    #region
    public override string Name { get { return "Rai Heigth Jump"; } }
    #endregion

    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Gør klar til at kunne bruge evnen
    /// </summary>
    public override void OnLoaded(GameObject characterGo)
    {
        base.OnLoaded(characterGo);
        playerController.OnHitGround += RaiJump_OnHitGround;
    }

    /// <summary>
    /// Får spilleren til at hoppe og når spilleren 
    /// rammer jorden, tillad at man kan give skade.
    /// </summary>
    public override void OnActivate()
    {
        playerController.MakePlayerJump(junpHeight);
        isHitGroundActivated = true;
    }
    #endregion

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Giv skade når man rammer jorden
    /// </summary>
    private void RaiJump_OnHitGround()
    {
        if (isHitGroundActivated)
        {
            isHitGroundActivated = false;
            Debug.Log("Hit ground");
        }
        
    }
    #endregion




}