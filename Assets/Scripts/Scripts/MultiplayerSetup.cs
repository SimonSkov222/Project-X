using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerSetup : NetworkBehaviour {

    [SerializeField]
    protected Behaviour[] disableComponents;

	// Use this for initialization
	void Start ()
    {
        
        if (!isLocalPlayer)
        {
            foreach (var comp in disableComponents)
            {
                comp.enabled = false;

                //if (comp is PlayerController)
                //{
                //    ((PlayerController)comp).InvokeStart();
                //}
            }
        }

        IHealth obj = (IHealth)GetComponent(typeof(IHealth));
        if(obj != null)
            obj.EventOnTakeDamage += Obj_EventOnTakeDamage;

        gameObject.name = "Player " + GetComponent<NetworkIdentity>().netId;
	}
    
    [Client]
    private void Obj_EventOnTakeDamage(GameObject sender, GameObject target, int dmg)
    {
        uint senderID = sender.GetComponent<NetworkIdentity>().netId.Value;
        uint targetID = target.GetComponent<NetworkIdentity>().netId.Value;
        CmdTakeDmg(senderID, targetID, dmg);
    }

    [Command]
    private void CmdTakeDmg(uint senderID, uint targetID, int dmg)
    {
        RpcTakeDmg(senderID, targetID, dmg);
    }
    [ClientRpc]
    private void RpcTakeDmg(uint senderID, uint targetID, int dmg)
    {
        HealthHelper.GiveDamageExecute(senderID, targetID, dmg);
    }
}
