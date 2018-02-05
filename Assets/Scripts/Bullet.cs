using UnityEngine;


////////////////////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Denne klasse bruges til at flytte "Bullet"(gameobject).
//  Når "Bullet"(gamepbject) rammer et andet object
//   kalder vi metoden OnGameObjectEnter() på det object
//   vi ramte.
//  
////////////////////////////////////////////////////////////////////
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{

    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public Camera PlayerEyes { private get; set; }

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    public LayerMask ignoreCollision;
    public bool hasHitTarget;
    public float range = 50;
    public float speed = 70f;
    public float dmg = 50;

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private Vector3 startPoint;
    private Vector3 endPoint;
    private LayerMask ignoreMask;
    private Vector3? lastPosition;



    ///////////////////////////////
    //      Unity Event
    ///////////////////////////////

    /// <summary>
    /// Når gameobjectet er synlig flytter vi det hent mod endPoint
    ///  hvis gameobjectet rammer noget på vejen kalder vi
    ///  OnGameObjectEnter() på det object vi ramte
    ///  
    /// Når gameobjecet rammer noget gør vi gameobjecet usynlig 
    /// </summary>
    void Update ()
    {


        // Så længe gameObject ikke er synlig skal vi ikke gøre noget
        if (!gameObject.activeSelf) return;
        

        // Da vores gameobject kan "teleport"(hvis den har meget speed) bliver vi nød til 
        //  at tjekke om vi ville have ramt noget i mellem den gamle
        //  position til den nye position
        RaycastHit hit;
        if (lastPosition.HasValue && Physics.Linecast(lastPosition.Value, transform.position, out hit, ~ignoreCollision.value))
        {
            // Gør gameObject usynlig
            gameObject.SetActive(false);

            // Hvis hit har Metoden OnGameObjectEnter() på sig i et
            //  script sender vi dette gameObject som parameter
            hit.collider.SendMessage("OnGameObjectEnter", gameObject, SendMessageOptions.DontRequireReceiver);
        }
        
        // Flyt skudet/Sæt ny position 
        Vector3 heading = endPoint - startPoint;
        Vector3 direction = heading / heading.magnitude;

        float distanceThisFrame = speed * Time.deltaTime;
        //transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.position += direction * speed * Time.deltaTime;

        // Gør gameObject usynlig hvis den er ude af range
        if (Vector3.Distance(startPoint, transform.position) > range)
            gameObject.SetActive(false);

        // Opdater position
        lastPosition = transform.position;

        
    }

    /// <summary>
    /// Tjekker om vi kalde OnGameObjectEnter
    ///  på det object den vi ramte og gør 
    ///  gameobjectet usynlig bagefter
    /// </summary> 
    /// <param name="collision">Det object vores gameobject ramte</param>
    void OnTriggerEnter(Collider collision)
    {
        // Vores gameobject skal være synlig så vi ikke kan kalde den dobbelt. (Kan kaldes fra Update())
        if (gameObject.activeSelf && (ignoreCollision.value & (1 << collision.gameObject.layer)) != (1 << collision.gameObject.layer))
        {
            collision.gameObject.SendMessage("OnGameObjectEnter", gameObject, SendMessageOptions.DontRequireReceiver);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Kaldes nå vores gameobject bliver synlig
    /// 
    /// 
    /// </summary>
    void OnEnable()
    {
        //startPoint = transform.position;
        lastPosition = startPoint;
        hasHitTarget = false;
        
    }


    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////

    /// <summary>
    /// Her beregner vi gameobject skal flyve hent til
    /// ud fra hvor spilleren kigger hen
    /// </summary>
    /// <param name="startPos">(gunend) Hvor "bullet" skal starte fra</param>
    public void Fire(Vector3 startPos)
    {

        
        //Sæt start position
        startPoint = startPos;
        transform.position = startPoint;
        // Henter kameraes position ud fra "verden" og 
        // Vector3 variablen gør at den tager fra midten af skærmen
        Vector3 rayOrigin = PlayerEyes.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));


        // Tjekker om der er et object foran vores skyd, hvis ja får vi af vide hvad, hvis nej flyver skydet bare lige ud
        RaycastHit hit;
        bool hitTarget = Physics.Raycast(rayOrigin, PlayerEyes.transform.forward, out hit, range, ~ignoreCollision);
        endPoint = hitTarget ? hit.point : rayOrigin + PlayerEyes.transform.forward * range;
        if (hitTarget) { Debug.Log("Hit T: " + hit.collider.name); }

        //Gør synlig
        gameObject.SetActive(true);


    }
}
