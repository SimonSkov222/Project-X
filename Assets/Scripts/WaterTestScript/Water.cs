using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {


    public bool isUnderWater = false;
    private bool isInTheWater = false;
    private bool isOnLand = true;

    private PlayerMovement movement;
    private float oldGravity;

    private float upOrDown = 1;
    LayerMask water = 12;
    GameObject waterGameObject;

	// Use this for initialization
	void Start () {
        movement = GetComponent<PlayerMovement>();
        oldGravity = movement.gravity;
    }
	
	// Update is called once per frame
	void Update () {

        if (isInTheWater == true || isUnderWater == true)
        {
            InTheWater();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == water)
        {
            isInTheWater = true;
            waterGameObject = other.gameObject;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        Debug.Log("a");
        movement.gravity = oldGravity;
        isInTheWater = false;
    }
    
    private void UnderWater()
    {
        if (isInTheWater == true && transform.position.y < waterGameObject.transform.position.y - movement.maxHeight)
        {
            
            isUnderWater = true;

        }
        else if (isInTheWater == true && transform.position.y > waterGameObject.transform.position.y - movement.maxHeight)
        {
            isUnderWater = false;
        }
        Debug.Log(isUnderWater);
    }

    private void InTheWater()
    {
        Debug.Log("inthewater");
        if (Input.GetButton("Jump"))
        {
            upOrDown = 8;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            upOrDown = -8;
        }
        else
        {
            upOrDown = 0;
        }
        UnderWater();
        movement.gravity = 0;
        movement.movement.y = upOrDown;
    }
    
}
