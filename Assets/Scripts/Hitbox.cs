using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour {

    public delegate void TakeDamge(int dmg);

    public float percent = 1;

    public TakeDamge OnTakeDamge { private get; set; }

    private LayerMask CanGiveDamge;

    // Use this for initialization
    void Start () { CanGiveDamge = 1 << LayerMask.NameToLayer("Bullet") | 1 << LayerMask.NameToLayer("Main Player");  }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("Layer: " +  CanGiveDamge.value);
    }

    void OnGameObjectEnter(GameObject gb)
    {
        //Debug.Log("Hit: " + gb.layer);
        //

        if ((CanGiveDamge.value & (1 << gb.layer)) == (1 << gb.layer))
        {
            //Debug.Log("Hit: " + gb.activeSelf);
            var bullet = gb.GetComponent<Bullet>();
            if (!bullet.hasHitTarget || true)
            {
                bullet.hasHitTarget = true;
                OnTakeDamge(Mathf.RoundToInt(bullet.dmg * percent));
            }
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("HitC: " + collision.collider.name);

        //OnGameObjectEnter(collision.collider.gameObject);
    }
    
}
