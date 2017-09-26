using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{


    public LayerMask layerMask;
    public Transform gunEnd;
    public GameObject bulletTemplate;

    public float weaponRange = 10f;
    public int shots = 30;
    public int ammo = 1000;
    public float reloadSpeed;
    public float fireRate = 0.25f;

    private bool HasShots { get { return currentShots > 0; }}
    private bool CanReload { get { return currentShots != shots && currentAmmo > 0; }}


    private List<GameObject> bullets = new List<GameObject>();
    private LineRenderer laserLine;
    private Camera eyes;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);


    private float nextFire;
    private bool isReloading = false;
    
    
    public int currentAmmo { get; private set; }
    public int currentShots { get; private set; }

    // Use this for initialization
    void Start()
    {
        eyes = Camera.main;
        currentShots = shots;
        currentAmmo = ammo;
        laserLine = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {



        if (!isReloading && CanReload && (Input.GetButtonUp("Reload") || currentShots == 0))
        {
            StartCoroutine(Reload());
        }

    }

    void LateUpdate()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire && HasShots)
        {
            //Debug.Log("shots " + currentShots);
            if (!isReloading)
            {
                Shoot();
                currentShots--;
            }
        }
    }

    

    private void Shoot()
    {
        nextFire = Time.time + fireRate;
        Vector3 rayOrigin = eyes.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        //Vector3 dir = rayOrigin + eyes.transform.forward * weaponRange;
        Vector3 dir = rayOrigin + eyes.transform.forward * weaponRange;


        //StartCoroutine(ShotEffect());

        laserLine.SetPosition(0, gunEnd.position);
        
        //Debug.Log("B " + dir);
        //Vector3 dir = rayOrigin - gunEnd.position;


        var bullet = GetBullet();
        bullet.transform.position =gunEnd.position; //rayOrigin;//
        bullet.transform.rotation = eyes.transform.rotation;

        RaycastHit hit;
        Vector3 newSpot = rayOrigin + (dir.normalized * weaponRange);
        if (Physics.Raycast(rayOrigin, eyes.transform.forward, out hit, weaponRange, ~(1 << LayerMask.NameToLayer("Trigger"))))
        //if (Physics.Linecast(rayOrigin, dir, out hit, ~1 << LayerMask.NameToLayer("Trigger")))
        {
            laserLine.SetPosition(1, hit.point);
            bullet.GetComponent<Bullet>().endPoint = hit.point;
            //Debug.Log("hh " + hit.point);
        }
        else
        {
            bullet.GetComponent<Bullet>().endPoint = dir;
            laserLine.SetPosition(1, dir);
        }
        //Debug.Log("log a " + dir);
        //Debug.DrawLine(gunEnd.position, bullet.GetComponent<Bullet>().endPoint, Color.green, 10f);

        bullet.SetActive(true);


    }

    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        //gunAudio.Play();

        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }

    private GameObject GetBullet()
    {

        foreach (var bullet in bullets)
        {
            if (!bullet.activeSelf)
            {
                //bullet.SetActive(true);
                return bullet;
            }
        }
        
        var newBullet = Instantiate(bulletTemplate);
        newBullet.GetComponent<Bullet>().range = weaponRange;
        Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        //newBullet.SetActive(true);
        //var colbox = newBullet.AddComponent<BoxCollider>();


        bullets.Add(newBullet);



        return newBullet;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        //Debug.Log("Reload ");
        
        yield return new WaitForSeconds(2f);

        int cAmmo = currentAmmo;
        int cShots = currentShots;

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
        //currentAmmo += cAmmo - shots > 0 ? -(shots - currentShots) : -currentAmmo;
        //currentShots = currentAmmo - shots > 0 ? shots : currentAmmo;

        //Debug.Log("sho " + currentShots);
        //Debug.Log("ammo " + currentAmmo);
        isReloading = false;
    }


}
