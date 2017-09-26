using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    private string myText = "";

    public string MyText { get; set; }

    public Vector3 endPoint;                    // S
    public LayerMask ignoreCollision;
    public Vector3? oldPos;

    public bool hasHitTarget;

    public float range;

    public float speed = 70f;

    public float dmg = 50;

    private Vector3 startPos;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {


        if (gameObject.activeSelf)
        {
            //Debug.Log("start et eller andet");
            //Debug.Log("is " + Vector3.Distance(transform.position, endPoint));
            //Debug.Log("Q "+ endPoint.magnitude);
            //Debug.Log("S "+ transform.position.magnitude);
            

            float distanceThisFrame = speed * Time.deltaTime;
            //Debug.Log("1 " + Vector3.Distance(startPos, transform.position));
            

            if (oldPos.HasValue)
            {
                //Debug.Log("3 start et eller andet");
                RaycastHit hit;

                if (Physics.Linecast(oldPos.Value, transform.position, out hit, ~ignoreCollision.value))
                {
                    //Debug.Log("Hit on Linecast: "+hit.collider.name);
                    gameObject.SetActive(false);
                    
                    hit.collider.SendMessage("OnGameObjectEnter", gameObject, SendMessageOptions.DontRequireReceiver);
                    //HitTarget(hit.collider.gameObject);
                }
                else
                {
                    Debug.DrawLine(oldPos.Value, transform.position, Color.red, 20f);
                }

                
            }
            
            //Debug.Log("e "+ endPoint);
            //Debug.Log("c " + transform.position);
           // Debug.DrawLine(startPos, GetComponent<Bullet>().endPoint, Color.green, 10f);

            oldPos = transform.position;
            //transform.Translate(endPoint.normalized * distanceThisFrame, Space.World);
            //transform.position = Vector3.MoveTowards(transform.position, endPoint, distanceThisFrame);
            var heading = endPoint - startPos;
            //Debug.Log("a " + heading);
            //Debug.Log("b " + heading.normalized);
            transform.Translate(heading.normalized * distanceThisFrame, Space.World);
            if (Vector3.Distance(startPos, transform.position) > range)
            {
                //Debug.Log("Distance");

                gameObject.SetActive(false);

            }
        }
	}
    

    void OnCollisionEnter(Collision collision)
    {
        if ((ignoreCollision.value & (1 << collision.gameObject.layer)) != (1 << collision.gameObject.layer) && gameObject.activeSelf)
        {
            //Debug.Log("Hit collision: ");
            collision.gameObject.SendMessage("OnGameObjectEnter", gameObject, SendMessageOptions.DontRequireReceiver);
            gameObject.SetActive(false);
        }
        //HitTarget(collision.gameObject);
    }

    void OnEnable()
    {
        startPos = transform.position;
        oldPos = transform.position;
        hasHitTarget = false;
        //Debug.DrawLine(transform.position, endPoint, Color.blue, 10f);


    }

    private void HitTarget(GameObject go)
    {
        var hitHealth = go.GetComponent<Health>();
        //Debug.Log("hit targhet");
        //Debug.Log("11 takedamg");
        if (hitHealth != null)
        {
            hitHealth.TakeDamage(50);
            //Debug.Log("takedamg");
            //Debug.Log(hitHealth.currentHP);
        }
    }
    
    

}
