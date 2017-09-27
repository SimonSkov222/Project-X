using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{


    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public int currentAmmo { get; private set; }
    public int currentShots { get; private set; }


    ///////////////////////////////
    //      Private Properties
    ///////////////////////////////
    private bool HasShots { get { return currentShots > 0; } }
    private bool CanReload { get { return currentShots != shots && currentAmmo > 0; } }


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
    private LayerMask ignoreMask;
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
        currentShots = shots; 
        currentAmmo = ammo;
        ignoreMask = 1 << LayerMask.NameToLayer("Trigger");
}
    
    /// <summary>
    /// Vi giver spilleren mulighed for at reloade og
    /// tvinger til at reloade når han løber tør
    /// </summary>
    void Update()
    {
        if (!isReloading && CanReload && (Input.GetButtonUp("Reload") || currentShots == 0))
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

            //Debug.Log("shots " + currentShots);
            Shoot();
            currentShots--;
        }
    }


    ///////////////////////////////
    //      Public Method
    ///////////////////////////////

    /// <summary>
    /// Hver gang vi kalder metoden Shoot()
    /// finder den ud af hvor kameraet er
    /// og hvilken vej den kigger, også
    /// skyder den vej.
    /// </summary>
    private void Shoot()
    {
        
        
        // Henter kameraes position ud fra "verden" og 
        // Vector3 variablen gør at den tager fra midten af skærmen
        Vector3 rayOrigin = eyes.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        
        var bullet = GetBullet();
        bullet.transform.position = gunEnd.position;
        bullet.transform.rotation = eyes.transform.rotation;


        // Tjekker om der er et object foran vores skyd, hvis ja får vi af vide hvad, hvis nej flyver skydet bare lige ud
        RaycastHit hit;
        bool hitTarget = Physics.Raycast(rayOrigin, eyes.transform.forward, out hit, weaponRange, ~ignoreMask);
        bullet.GetComponent<Bullet>().endPoint = hitTarget ? hit.point : rayOrigin + eyes.transform.forward * weaponRange;
        
    }


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
        newBullet.GetComponent<Bullet>().range = weaponRange;
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

        int cAmmo = currentAmmo;
        int cShots = currentShots;

        // sørger for at vi får fuld reload hvis der er nok skyd til det eller giver os det som er tilbage
        if (currentAmmo + currentShots - shots > 0)
        {
            currentAmmo = currentAmmo + currentShots - shots;
            currentShots = shots;
        }
        else
        {
            currentShots = currentAmmo + currentShots;
            currentAmmo = 0;
        }

        //Debug.Log("shot " + currentShots);
        //Debug.Log("ammo " + currentAmmo);
        isReloading = false;
    }


}
