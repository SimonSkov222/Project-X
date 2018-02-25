using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//
//  Med det her script ved vi hvad vores våben rammer.
//  Vi sørger også for at vores våben ikke rammer spilleren som har det.
//
////////////////////////////////////////////////////////////////////
public class SwordHit : MonoBehaviour {
    

    CharacterController c;

	// Use this for initialization
	void Start ()
    {
        c = GetComponentInParent<CharacterController>();
        Physics.IgnoreCollision(c, transform.GetComponent<Collider>(), true);
	}
	
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }

}
