using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Water : NetworkBehaviour {

    private Transform eyes;
    private PlayerController pc;
    private Fog fog;
    private float oldGravity;
    private float oldRunSpeed;

    //// Use this for initialization
    void Start()
    {
        eyes = transform.Find("Eyes");
        pc = GetComponent<PlayerController>();
        fog = GetComponentInChildren<Fog>();
        oldGravity = pc.gravity;
        oldRunSpeed = pc.runSpeed;
        fog.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Debug.Log("Enter");
            Transform waterTop = other.gameObject.transform.Find("WaterTop");
            pc.runSpeed = 4f;
            pc.gravity = 2f;

            if (waterTop.position.y > eyes.position.y)
            {
                fog.enabled = true;
            }
            else
            {
                fog.enabled = false;
                
            }
            if (Input.GetButton("Jump"))
            {
                pc.MakePlayerJump(2f);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            pc.runSpeed = oldRunSpeed;
            pc.gravity = oldGravity;
        }

    }

    //void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("a");
    //    pc.gravity = oldGravity;
    //    isInTheWater = false;
    //}

    //private void UnderWater()
    //{
    //    if (isInTheWater == true && transform.position.y < waterGameObject.transform.position.y - pc.maxHeight)
    //    {

    //        isUnderWater = true;

    //    }
    //    else if (isInTheWater == true && transform.position.y > waterGameObject.transform.position.y - pc.maxHeight)
    //    {
    //        isUnderWater = false;
    //    }
    //    Debug.Log(isUnderWater);
    //}

    //private void InTheWater()
    //{
    //    Debug.Log("inthewater");
    //    if (Input.GetButton("Jump") && transform.position.y < waterGameObject.transform.position.y - (pc.maxHeight / 2))
    //    {
    //        upOrDown = 8;
    //    }
    //    else if (Input.GetKey(KeyCode.X) || transform.position.y > waterGameObject.transform.position.y - (pc.maxHeight / 2))
    //    {
    //        upOrDown = -8;
    //    }
    //    else
    //    {
    //        upOrDown = 0;
    //    }
    //    UnderWater();
    //    pc.gravity = 0;
    //    pc.movement.y = upOrDown;
    //}

}
