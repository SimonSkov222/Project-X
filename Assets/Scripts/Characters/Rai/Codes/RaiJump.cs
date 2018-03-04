using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RaiJump", menuName = "Abilities/Rai/Jump")]
public class RaiJump : AbilityBasic
{
    [SerializeField]
    [Range(3, 20)]
    protected float junpHeight = 10f;

    [SerializeField]
    protected float dmgOnLand = 50f;

    private bool isHitGroundActivated = false;

    public override string Name { get { return "Rai Heigth Jump"; } }

    public override void OnLoaded(GameObject characterGo)
    {
        base.OnLoaded(characterGo);


        Debug.Log("OnLoaded");
        characterGo.GetComponent<PlayerController>().OnHitGround += RaiJump_OnHitGround;


    }

    private void RaiJump_OnHitGround()
    {
        if (isHitGroundActivated)
        {
            isHitGroundActivated = false;
            Debug.Log("Hit ground");
        }
        
    }

    public override void OnActivate()
    {
        PlayerController.Instance.MakePlayerJump(junpHeight);
        isHitGroundActivated = true;
    }
}
