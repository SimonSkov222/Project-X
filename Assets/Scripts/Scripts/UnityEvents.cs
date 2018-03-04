using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
