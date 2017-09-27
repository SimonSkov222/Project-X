using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Her inde har vi lavet en sphere collider som
//  bruges med en trigger, triggeren bliver activeret
//  når player kommer inden for dens række vide.
//  Vi har lavet en metode som gør at man skal stå
//  foran NPC'en før den begynder at følge efter
//  playeren.
//  Hvis der ikke er en player foran NPC'en og inden
//  for dens sphere collider gå den rundt random.
//////////////////////////////////////////////////////

[RequireComponent(typeof(NavMeshAgent))]
public class MovementAI : MonoBehaviour
{

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    public LayerMask followTarget;
    [Range(0f, 360f)]
    public float fieldOfViewDegrees;
    public float radius;


    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 lastPoint;
    private SphereCollider sphereCollider;
    private NavMeshAgent enemy;
    private Animator anim;
    private int test1 = 0;
    private bool isFollowTarget;


    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////

    /// <summary>
    /// Sørger for at vi har noget vi kan bruge vores trigger med
    /// </summary>
    void Start()
    {
        anim = GetComponent<Animator>();

        enemy = gameObject.GetComponent<NavMeshAgent>();
        startPoint = transform.position;
        

        var targetRange = new GameObject();
        targetRange.name = "TriggerRange";
        targetRange.layer = LayerMask.NameToLayer("Trigger");
        targetRange.transform.parent = transform;
        targetRange.transform.position = transform.position;

        var sphereCollider = targetRange.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = radius;
        
        var targetTrigger = targetRange.AddComponent<ChildTriggerCollider>();
        targetTrigger.TriggerOnStay = TriggerOnStay;
        

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("h"))
        {
            anim.SetInteger("Test1", 1);
        }
        else if (Input.GetKeyDown("j"))
        {
            anim.SetInteger("Test1", 2);
        }
        if (Input.GetKeyDown("k"))
        {
            anim.SetInteger("Test1", 3);
        }
        if (Input.GetKeyDown("l"))
        {
            anim.SetInteger("Test1", 4);
        }

        
        //if (lastPoint == transform.position || enemy.isStopped || endPoint == null
        //    || layerMask.value == 1 << LayerMask.NameToLayer("Main Player"))
        //{
        //    anim.SetInteger("Test1", 3);
        //}

        
        if (lastPoint == transform.position || enemy.isStopped || endPoint == null || Vector3.Distance(transform.position, endPoint) < 10)
        {
            RandomMove();
            anim.SetInteger("Test1", 1);
        }
        lastPoint = transform.position;

        //RaycastHit hit;
        //var players = Physics.OverlapSphere(transform.position, radius, layerMask);

        //if (players.Length > 0)
        //{
        //    Debug.Log(players[0].transform.position);
        //    enemy.SetDestination(players[0].transform.position);
        //}

        
    }

    ///////////////////////////////
    //      Private Method
    ///////////////////////////////

    /// <summary>
    /// Tjekker om der er noget inden for dens trigger cirkel, hvis ja skal den følge efter den
    /// </summary>
    /// <param name="col"></param>
    private void TriggerOnStay(Collider col)
    {
        // Tjekker om layer mask er et match og om noget er foran den
        if ((followTarget.value & 1 << col.gameObject.layer) == (1 << col.gameObject.layer) && CanSeePlayer(col.gameObject))
            enemy.SetDestination(col.transform.position);
    }


    /// <summary>
    /// har en radom generator som får den til at gå forskellige steder hen
    /// 
    /// to do lave om
    /// </summary>
    private void RandomMove()
    {
        NavMeshHit hit;

        Vector3 rDirection = Random.insideUnitSphere * radius;

        rDirection += startPoint;

        NavMesh.SamplePosition(rDirection, out hit, radius, 1);
        enemy.SetDestination(hit.position);
        endPoint = hit.position;
    }


    /// <summary>
    /// Sørger for at Ai's trigger kun virker foran den
    /// </summary>
    /// <param name="gb">Det object som skal blive set</param>
    /// <returns>Returner true hvis noget er foran Ai</returns>
    private bool CanSeePlayer(GameObject gb)
    {
        
        RaycastHit hit;
        Vector3 rayDirection = gb.transform.position - transform.position;
        // Tjekker på om der er noget inden for den angle som vi laver
        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 0.5f)
            if (Physics.Raycast(transform.position, rayDirection, out hit, radius))
                return (hit.transform.CompareTag("Player"));

        return false;
    }


    //private float GetHighestValue(Vector3 value)
    //{
    //    float c = value.x;
    //    if (c < value.y) c = value.y;
    //    if (c < value.z) c = value.z;
    //    return c;
    //}


}
