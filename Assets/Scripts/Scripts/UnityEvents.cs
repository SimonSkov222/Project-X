using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Gør at vi kan tilføj metoder til de forskellige
//  unity events f.eks. Update().
//
//  Dette kan være nyttig, når du laver et nyt gameobject
//  i et script og at dette gameobjecet, skal udføre noget i Update(),
//  i stedet for at lavet et nye script og tilføje dette nye script 
//  til gameobjecetet, kan man bare tildele en metode til EventOnUpdate
//  via. UnityEvents
//
//////////////////////////////////////////////////////
public class UnityEvents : MonoBehaviour {

    public delegate void OnEvent(GameObject sender);
    public delegate void OnTrigger(GameObject sender, Collider col);
    public delegate void OnCollision(GameObject sender, Collision col);

    public event OnEvent EventOnUpdate;
    public event OnEvent EventOnLateUpdate;
    public event OnEvent EventOnFixelUpdate;
    public event OnEvent EventOnEnable;
    public event OnEvent EventOnDisable;
    public event OnCollision EventOnCollisionEnter;
    public event OnCollision EventOnCollisionExit;
    public event OnCollision EventOnCollisionStay;
    public event OnTrigger EventOnTriggerEnter;
    public event OnTrigger EventOnTriggerExit;
    public event OnTrigger EventOnTriggerStay;

    public object[] tags;


    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////
    #region
    void Update ()      { if (EventOnUpdate != null) EventOnUpdate(gameObject); }
    void LateUpdate()   { if (EventOnLateUpdate != null) EventOnLateUpdate(gameObject); }
    void FixedUpdate()  { if (EventOnFixelUpdate != null) EventOnFixelUpdate(gameObject); }

    void OnEnable()     { if (EventOnEnable != null) EventOnEnable(gameObject); }
    void OnDisable()    { if (EventOnDisable != null) EventOnDisable(gameObject); }

    void OnCollisionEnter(Collision col)    { if (EventOnCollisionEnter != null) EventOnCollisionEnter(gameObject, col); }
    void OnCollisionExit(Collision col)     { if (EventOnCollisionExit != null) EventOnCollisionExit(gameObject, col); }
    void OnCollisionStay(Collision col)     { if (EventOnCollisionStay != null) EventOnCollisionStay(gameObject, col); }

    void OnTriggerEnter(Collider col)       { if (EventOnTriggerEnter != null) EventOnTriggerEnter(gameObject, col); }
    void OnTriggerExit(Collider col)        { if (EventOnTriggerExit != null) EventOnTriggerExit(gameObject, col); }
    void OnTriggerStay(Collider col)        { if (EventOnTriggerStay != null) EventOnTriggerStay(gameObject, col); }
    #endregion
}
