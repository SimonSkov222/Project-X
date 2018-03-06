using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Affyre et skud der bliver større når det rammer noget 
//  eller når det har fløjet langt nok væk.
//  Giver skade til det den rammer.
//
//////////////////////////////////////////////////////
[CreateAssetMenu(fileName = "RaiBoomBullet", menuName = "Abilities/Rai/BoomBullet")]
public class RaiBoomBullet : AbilityBasic
{
    ///////////////////////////////
    //      Protected Fields
    ///////////////////////////////
    #region
    [SerializeField]
    protected GameObject bulletModel;
    [SerializeField]
    protected string gunEndLocation = "Eyes/Weapons/MainGun/GunEnd";

    [SerializeField]
    [Range(1, 15)]
    protected float radius = 8;
    [SerializeField]
    [Range(1, 25)]
    protected float range = 10;
    [SerializeField]
    [Range(1, 50)]
    protected float bulletSpeed = 10;
    [SerializeField]
    [Range(50, 500)]
    protected float explodingSpeed = 100;
    [SerializeField]
    [Range(0, 250)]
    protected int damage = 10;

    #endregion
    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region

    private GameObject player;
    private Transform gunEnd;
    private GameObject bullet;
    private bool isExploding = false;
    private Dictionary<int,GameObject> targets = new Dictionary<int, GameObject>();
    private bool debug_pause = false;
    #endregion

    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    #region
    public override string Name { get { return ""; } }
    #endregion
    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////
    #region
    /// <summary>
    /// Gør klar til at bruge evnen.
    /// Opretter skud/bullet
    /// </summary>
    public override void OnLoaded(GameObject characterGo)
    {
        base.OnLoaded(characterGo);
        player = characterGo;
        gunEnd = player.transform.Find(gunEndLocation);

        bullet = Instantiate<GameObject>(bulletModel);
        bullet.SetActive(false);
        UnityEvents ue = bullet.AddComponent<UnityEvents>();
        ue.EventOnUpdate += Bullet_OnUpdate;
        ue.EventOnTriggerStay += (b,c) => BulletHelper.CallMethodOnBulletHitObject(b,c, OnBulletHitCollider);
        ue.EventOnDisable += Bullet_OnDisable;
    }

    /// <summary>
    /// Affyre skudet/evnen
    /// </summary>
    public override void OnActivate()
    {
        bullet.transform.position = gunEnd.position;
        bullet.transform.rotation = gunEnd.rotation;
        bullet.transform.localScale = bulletModel.transform.localScale;
        BulletHelper.UpdateBulletStartPosition(bullet);

        bullet.SetActive(true);
    }
    #endregion

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Giver skade til alle fjender der blev ramt af skudet
    /// </summary>
    private void Bullet_OnDisable(GameObject sender)
    {
        foreach (var item in targets)
        {
            HealthHelper.GiveDamage(player, item.Value, damage);
        }

        isExploding = false;
        targets.Clear();
    }

    /// <summary>
    /// Gemmer de enemies der bliver ramt af skudet
    /// </summary>
    private void OnBulletHitCollider(GameObject sender, GameObject target, TargetType type)
    {


        if (target.GetInstanceID() == player.GetInstanceID() || type == TargetType.Teammate || type == TargetType.TeamShield)
        {
            return;
        }

        isExploding = true;
        if ((type == TargetType.Enemy || type == TargetType.EnemyShield) && !targets.ContainsKey(target.GetInstanceID()))
        {
            targets.Add(target.GetInstanceID(), target);
        }

    }

    /// <summary>
    /// Flytter skudet og gøre det større
    /// </summary>
    private void Bullet_OnUpdate(GameObject sender)
    {
        if (debug_pause) return;
        if (sender.activeSelf)
        {
            if (!isExploding)
            {
                BulletHelper.MoveBulletOnUpdate(sender, bulletSpeed);
            }
            

            if (BulletHelper.IsBulletOutOfRange(sender, range) || isExploding)
            {
                sender.transform.localScale += (new Vector3(0.1f, 0.1f, 0.1f) * explodingSpeed * Time.deltaTime);
            }
            if (sender.transform.localScale.x > radius || sender.transform.localScale.y > radius || sender.transform.localScale.z > radius)
            {
                bullet.SetActive(false);
            }
        }
    }
    #endregion
}
