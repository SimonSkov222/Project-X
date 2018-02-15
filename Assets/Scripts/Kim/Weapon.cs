﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  I denne klasse gør vi at vi kan skyde og at vores
//  skyd rammer rigtigt iforhold til vores kamera.
//  vi sørger også for at bruge de samme skyd som
//  har været brugt eller lave nye hvis der ikke er
//  flere at bruge, så vi ikke bruger ligeså meget
//  resourse kraft
//  Det er også her vi reloader våbent når man trykker
//  (R) eller der ikke er flere skyd tilbage
//  
//////////////////////////////////////////////////////

public class Weapon : MonoBehaviour
{

    Animator anim;

    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public int CurrentAmmo { get; private set; }
    public int CurrentShots { get; private set; }


    ///////////////////////////////
    //      Private Properties
    ///////////////////////////////
    private bool HasShots { get { return CurrentShots > 0; } }
    private bool CanReload { get { return CurrentShots != shots && CurrentAmmo > 0; } }


    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    public Swords sword;
    public Guns gun;
    public Transform gunEnd;
    public GameObject bulletTemplate;
    private float weaponRange;
    private int shots;
    private int ammo;
    private float reloadSpeed;
    private float fireRate;


    private float swordSpeed;
    private float swordDmg;
    private float swordRange;


    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private List<GameObject> bullets = new List<GameObject>();
    private LineRenderer laserLine;
    private Camera eyes;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private float nextFire;
    private bool isReloading = false;
    



    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////


    /// <summary>
    /// Vi henter kameraet, så vi ved hvor vi skal sende skydende hen.
    /// Giver spilleren fuld ammunition.
    /// </summary>
    void Start()
    {
        eyes = Camera.main;
        anim = GetComponent<Animator>();

        if (gun != null)
        {
            weaponRange = gun.wWeaponRange;
            shots = gun.wShots;
            ammo = gun.wAmmo;
            reloadSpeed = gun.wReloadSpeed;
            fireRate = gun.wFireRate;
            //Debug.Log("weapon range: " + weaponRange + " shots: " + shots + " ammo: " + ammo + " reload speed: " + reloadSpeed + " firerate: " + fireRate);
            CurrentShots = shots;
            CurrentAmmo = ammo;
        }
        else if (sword != null)
        {

            var iSword = Instantiate(sword.sword);

            iSword.transform.position = eyes.transform.GetChild(0).transform.position;

            iSword.transform.parent = eyes.transform.GetChild(0).transform;

            iSword.transform.Rotate(-4.955f, -97.813f, 4.463f);
            
            swordSpeed = sword.speed;
            swordDmg = sword.dmg;
            swordRange = sword.range;
            
        }
        
        

        
    }

    /// <summary>
    /// Vi giver spilleren mulighed for at reloade
    /// og tvinger ham til at reloade når han løber tør.
    /// </summary>
    void Update()
    {
        //Debug.Log("forth");
        if (!isReloading && CanReload && (Input.GetButtonUp("Reload") || CurrentShots == 0))
        {
            //Debug.Log("third");
            // Starter Reload() så den kan blive paused når som helst
            StartCoroutine(Reload());
        }

        
    }

    /// <summary>
    /// Afyre skyd på det rigtige tidspunkt.
    /// 
    /// Hvis vi ikke udføre den i LateUpdate(),
    /// så kan vi ikke være sikker på at spillerens
    /// rigidbody/krop er det rigtige sted
    /// </summary>
    void LateUpdate()
    {
        //Debug.Log("second");
        //Debug.Log("Currentshots: " + CurrentShots + " shots: " + shots + " has shots: " + HasShots);
        //if (Input.GetButton("Fire1") && !isReloading && Time.time > nextFire && HasShots)
        //{
        //    //Debug.Log("first");
        //    // Laver en begrænsning for hvornår man kan skyde igen
        //    nextFire = Time.time + fireRate;
        //    GetBullet().GetComponent<Bullet>().Fire(gunEnd.position);
        //    CurrentShots--;
        //}

        if (Input.GetButtonDown("Fire1"))
        {
            SwordAttack();
        }
    }


    ///////////////////////////////
    //      Private Method
    ///////////////////////////////

    /// <summary>
    /// Hvis der er et bullet som er deaktiveret i spillet,
    /// bliver den genbrugt, og hvis der ikke er nogen deaktiveret bullets,
    /// bliver der lavet et nyt.
    /// </summary>
    /// <returns>Sender et usynligt bullet tilbage</returns>
    private GameObject GetBullet()
    {

        // Her ser vi om der er et usynligt bullet, hvis ja sender vi det tilbage
        foreach (var bullet in bullets)
            if (!bullet.activeSelf)
                return bullet;



        var newBullet = Instantiate(bulletTemplate);
        newBullet.GetComponent<Bullet>().PlayerEyes = eyes;
        // Sørger for at collideren på vores bullet og våben ikke kan ramme ind i hinanden
        Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        bullets.Add(newBullet);

        return newBullet;
    }


    /// <summary>
    /// Efter 2 sekunder får spilleren fuld ammunition, eller det antal skyd de har tilbage.
    /// </summary>
    /// <returns>Returner om vi er igang med at reloade eller ej</returns>
    private IEnumerator Reload()
    {
        isReloading = true;
        //Debug.Log("Reload ");

        // laver en time på 2 sek
        yield return new WaitForSeconds(2f);

        int cAmmo = CurrentAmmo;
        int cShots = CurrentShots;

        // sørger for at vi får fuld reload hvis der er nok skyd til det eller giver os det som er tilbage
        if (CurrentAmmo + CurrentShots - shots > 0)
        {
            CurrentAmmo = CurrentAmmo + CurrentShots - shots;
            CurrentShots = shots;
        }
        else
        {
            CurrentShots = CurrentAmmo + CurrentShots;
            CurrentAmmo = 0;
        }

        //Debug.Log("shot " + CurrentShots);
        //Debug.Log("ammo " + CurrentAmmo);
        isReloading = false;
    }


    private void SwordAttack()
    {

        Vector3 rayOrigin = eyes.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Debug.Log("Second");

        RaycastHit[] hits;

        hits = Physics.RaycastAll(rayOrigin, eyes.transform.forward, swordRange);

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("First");
            RaycastHit hit = hits[i];

            Debug.Log(hit.collider.name);
        }


    }


}