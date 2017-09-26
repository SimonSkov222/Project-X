using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerCollider : MonoBehaviour {


    public delegate void OnTrigger(Collider collider);


    public OnTrigger TriggerOnEnter { private get; set; }
    public OnTrigger TriggerOnStay { private get; set; }
    public OnTrigger TriggerOnExit { private get; set; }


    void OnTriggerEnter(Collider collider)
    {
        if (TriggerOnEnter != null)
        {
            TriggerOnEnter(collider);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (TriggerOnStay != null)
        {
            TriggerOnStay(collider);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (TriggerOnExit != null)
        {
            TriggerOnExit(collider);
        }
    }

}
