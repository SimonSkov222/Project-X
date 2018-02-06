using System.Collections;
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
    public Transform gunEnd;
    public GameObject bulletTemplate;
    public float weaponRange = 10f;
    public int shots = 30;
    public int ammo = 1000;
    public float reloadSpeed;
    public float fireRate = 0.25f;


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
    /// Vi henter spillerens øjne så vi ved hvor 
    /// vi skal sende skydne hen
    /// 
    /// Giver spilleren fuld ammonition
    /// </summary>
    void Start()
    {
        eyes = Camera.main;
        CurrentShots = shots;
        CurrentAmmo = ammo;
    }

    /// <summary>
    /// Vi giver spilleren mulighed for at reloade og
    /// tvinger til at reloade når han løber tør
    /// </summary>
    void Update()
    {
        if (!isReloading && CanReload && (Input.GetButtonUp("Reload") || CurrentShots == 0))
        {
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
        if (Input.GetButton("Fire1") && !isReloading && Time.time > nextFire && HasShots)
        {
            // Laver en begrænsning for hvornår man kan skyde igen
            nextFire = Time.time + fireRate;
            GetBullet().GetComponent<Bullet>().Fire(gunEnd.position);
            CurrentShots--;
        }
    }


    ///////////////////////////////
    //      Public Method
    ///////////////////////////////

    /// <summary>
    /// Hvis der er et bullet som er deaktiveret i spillet,
    /// bliver det genbrugt, og hvis der ikke er noget deaktiveret
    /// bullet, bliver der lavet et nyt.
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
    /// Efter 2 sek så får spilleren full reload,
    /// eller det antal skyd de har tilbage
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


}
