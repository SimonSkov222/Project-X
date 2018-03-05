using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Dette er en meget simple attack enemy
//  den kan skyde 3 skud afsted hurtigt
//  og den kan tage skade fra charatere og dø
//
//////////////////////////////////////////////////////
[RequireComponent(typeof(NPCMovement))]
[RequireComponent(typeof(SphereCollider))]
public class EnemyController : MonoBehaviour, IHealth
{

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region
    private NPCMovement agent;
    private List<GameObject> bulletPool = new List<GameObject>();
    private float timeAttack;
    #endregion
    
    ///////////////////////////////
    //      Protected Fields
    ///////////////////////////////
    #region

    [Header("Health Settings")]
    [SerializeField]
    [Range(0, 1000)]
    protected int healthMax = 100;
    [SerializeField]
    [Range(0, 1000)]
    protected int armorMax = 0;
    [SerializeField]
    [Range(0, 1000)]
    protected int shieldMax = 0;

    [Header("Weapon Settings")]
    [SerializeField]
    protected GameObject bulletModel;
    [SerializeField]
    [Range(0, 5)]
    protected float fireRate = 0.2f;
    [SerializeField]
    [Range(0, 25)]
    protected float coldown = 5;
    [SerializeField]
    [Range(1, 100)]
    protected float bulletSpeed = 25;
    [SerializeField]
    [Range(1, 100)]
    protected float bulletRange = 1;

    [SerializeField]
    protected Vector3 BulletRottaing;
    [SerializeField]
    protected GameObject gunEnd;

    #endregion



    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    #region

    public int HealthMax { get { return healthMax; } }
    public int ArmorMax { get { return armorMax; } }
    public int ShieldMax { get { return shieldMax; } }

    public int Health { get; set; }
    public int Armor { get; set; }
    public int Shield { get; set; }
    public int HealthBonus { get; set; }

    public float WeaknessMultiplier { get; set; }

    #endregion
    

    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////
    #region

    /// <summary>
    /// henter component og giver objecet liv
    /// </summary>
    void Start()
    {
        agent = GetComponent<NPCMovement>();
        HealthHelper.Initialize(this);
        timeAttack = Time.time;
    }
    /// <summary>
    /// Når vi kan se vores target gå til angrab
    /// </summary>
    void Update()
    {
        if (agent.target != null && Time.time - timeAttack > coldown && GameHelper.CanSeePlayer(gameObject, agent.target, 180, agent.m_FollowStopDistanceMax))
        {
            gunEnd.transform.LookAt(agent.target.transform);

            timeAttack = Time.time;
            StartCoroutine(Attack());
        }
    }

    /// <summary>
    /// Hvis det er en fjene(layer Teammate) og at vi kan se fjenen
    /// vil gameobjectet gå hen til fjenen når fjenen kan blive set.
    /// </summary>
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Teammate"))
        {
            if (GameHelper.CanSeePlayer(gameObject, col.gameObject, 180f, 35f) && (agent.target == null
                || Vector3.Distance(transform.position, agent.target.transform.position) > Vector3.Distance(transform.position, col.transform.position)))
            {
                agent.target = col.gameObject;
                agent.m_Pattern = MovementPattern.FollowTargetWhenSeen;
            }

        }
    }

    /// <summary>
    /// Hvis vores target gå ud at vores sphere collider
    /// stopper vores gameobjecet med at følge efter
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerExit(Collider col)
    {
        if (agent.target != null && agent.target.GetInstanceID() == col.gameObject.GetInstanceID())
        {
            agent.m_Pattern = MovementPattern.Idle;
            agent.target = null;
        }
    }

    #endregion


    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////
    #region

    public void OnDeath(object sender)
    {
        gameObject.SetActive(false);
    }

    public void OnTakeDmg(GameObject sender, int dmg)
    {
    }
    #endregion

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Henter et skud der ikke er aktiv
    /// </summary>
    private GameObject GetBullet()
    {
        GameObject bullet = bulletPool.FirstOrDefault(m => m != null && !m.activeSelf);
        if (bullet == null)
        {
            bullet = CreateBullet();
        }

        return bullet;
    }

    /// <summary>
    /// Opretter et nyt skud
    /// </summary>
    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate<GameObject>(bulletModel);
        UnityEvents ue = bullet.AddComponent<UnityEvents>();
        ue.EventOnUpdate += Bullet_OnUpdate;
        ue.EventOnTriggerEnter += (b,c) => BulletHelper.CallMethodOnBulletHitObject(b,c, OnBulletHitTarget);
        bullet.SetActive(false);

        bulletPool.Add(bullet);

        return bullet;
    }

    /// <summary>
    /// Bliver kaldt når et skud rammer noget.
    /// Giv skade til det object der bliver ramt
    /// </summary>
    private void OnBulletHitTarget(GameObject bullet, GameObject target, TargetType type)
    {
        //ignorere sig selv
        if (target.GetInstanceID() == gameObject.GetInstanceID())
        {
            return;
        }
        
        if (type == TargetType.None)
        {

            bullet.SetActive(false);
        }

        //Giv skade
        if (type == TargetType.Teammate || type == TargetType.TeamShield)
        {
            HealthHelper.GiveDamage(gameObject, target, 10);
            bullet.SetActive(false);
        }
    }

    /// <summary>
    /// Flytter et skud i en  retning så længe det er aktiv
    /// </summary>
    private void Bullet_OnUpdate(GameObject sender)
    {
        if (sender.activeSelf)
        {
            BulletHelper.MoveBulletOnUpdate(sender, bulletSpeed);

            if (BulletHelper.IsBulletOutOfRange(sender, bulletRange))
            {
                BulletHelper.MakeBulletSmallerOnUpdate(sender, 20);

                if (BulletHelper.IsBulletTooSmall(sender))
                {
                    sender.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Afyre 3 skud af sted
    /// </summary>
    private IEnumerator Attack()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = GetBullet();
            bullet.transform.position = gunEnd.transform.position;
            bullet.transform.rotation = gunEnd.transform.rotation;
            bullet.transform.localScale = bulletModel.transform.localScale;
            BulletHelper.UpdateBulletStartPosition(bullet);
            bullet.SetActive(true);
            yield return new WaitForSeconds(fireRate);
        }
    }

    #endregion
    








}
