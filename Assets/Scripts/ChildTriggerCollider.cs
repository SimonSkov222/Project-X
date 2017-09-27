using UnityEngine;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//
//  Når vores gameobject trigger et event
//   vil den i stedet kalde en af vores properties.
//  f.eks. OnTriggerEnte()r vil kalde propertie TriggerOnEnter
//  
//  Dette script bliver tildelt til et child gameobject
//   så vi kan lave metoder inde i parent gameobject som så kan
//   blive sat på disse propertites.
//
////////////////////////////////////////////////////////////////////
public class ChildTriggerCollider : MonoBehaviour
{    
    ///////////////////////////////
    //      Public Deletages
    ///////////////////////////////
    public delegate void OnTrigger(Collider collider);
    

    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public OnTrigger TriggerOnEnter { private get; set; }
    public OnTrigger TriggerOnStay { private get; set; }
    public OnTrigger TriggerOnExit { private get; set; }


    ///////////////////////////////
    //      Unity Event
    ///////////////////////////////
    void OnTriggerEnter(Collider collider)  { if (TriggerOnEnter != null)   TriggerOnEnter(collider);   }
    void OnTriggerStay(Collider collider)   { if (TriggerOnStay != null)    TriggerOnStay(collider);    }
    void OnTriggerExit(Collider collider)   { if (TriggerOnExit != null)    TriggerOnExit(collider);    }

}
