using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Den her klasse sørger for at, det kun er
//  udvalgte layers som kan give dmg til en collider
//  Man kan også vægle hvor meget % dmg de skal tage
//  
//////////////////////////////////////////////////////

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour {

    ///////////////////////////////
    //      Public Delegate
    ///////////////////////////////
    public delegate void TakeDamge(int dmg);

    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public TakeDamge OnTakeDamge { private get; set; }

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    public float percent = 1;

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private LayerMask CanGiveDamge;


    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////

    /// <summary>
    /// Her definere hvad der kan give skade til gameobjectet.
    /// </summary>
    void Start () { CanGiveDamge = 1 << LayerMask.NameToLayer("Bullet") | 1 << LayerMask.NameToLayer("Main Player");  }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("Layer: " +  CanGiveDamge.value);
    }

    ///////////////////////////////
    //      Private Method
    ///////////////////////////////


    /// <summary>
    /// Denne metode skal blive kaldt fra et andet script.
    /// Hvis vi bliver ramt af et gameobject som kan give os skade,
    /// tager vi skade.
    /// </summary>
    /// <param name="gb"></param>
    private void OnGameObjectEnter(GameObject gb)
    {
        //Debug.Log("Hit: " + gb.layer);
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
    
    
}
