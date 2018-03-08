using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Affyre skud som giver skade
//
//////////////////////////////////////////////////////
[CreateAssetMenu(fileName = "RaiMainWeapon", menuName = "Weapons/Rai/MainWeapon")]
public class RaiWeapon : WeaponBasic
{

    ///////////////////////////////
    //      Protected Fields
    ///////////////////////////////
    #region

    [SerializeField]
    [Range(1, 250)]
    protected float speed;
    [SerializeField]
    [Range(1, 250)]
    protected float range;
    [SerializeField]
    protected GameObject bulletModel;

    [SerializeField]
    protected int bulletPoolSize;
    [SerializeField]
    protected string gunEndLocation = "Eyes/Weapons/MainGun/GunEnd";
    protected Transform gunEnd;

    protected List<GameObject> bulletPool = new List<GameObject>();
    protected GameObject bulletParant;
    #endregion

    ///////////////////////////////
    //      public Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Gør klar til at bruge våbnet.
    /// Laver array af skud
    /// </summary>
    public override void OnLoaded(GameObject characterGo)
    {
        base.OnLoaded(characterGo);

        Ammo = ammoMax;
        
        gunEnd = player.transform.Find(gunEndLocation);
        bulletParant = new GameObject(characterGo.name + "_RaiBulletPool");
        for (int i = 0; i < bulletPoolSize; i++)
        {
            CreateNewBullet();
        }
    }

    /// <summary>
    /// Affyre et skud et skud og reload hvis man er løbet tør
    /// for skud.
    /// </summary>
    public override void OnFire()
    {
        if (Ammo > 0 && !IsReloading)
        {
            GameObject bullet = GetBullet();
            bullet.transform.position = gunEnd.position;
            bullet.transform.rotation = gunEnd.rotation;
            bullet.transform.localScale = bulletModel.transform.localScale;
            BulletHelper.UpdateBulletStartPosition(bullet);
            bullet.SetActive(true);

            Ammo--;
        }
        else
        {
            OnReload_Down();
        }
    }

    #endregion

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Opretter et nyt skud og tildeler den de metoder den skal bruge for at virke
    /// </summary>
    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate<GameObject>(bulletModel);
        bullet.transform.SetParent(bulletParant.transform);
        UnityEvents ue = bullet.AddComponent<UnityEvents>();
        ue.EventOnUpdate += Bullet_OnUpdate;
        ue.EventOnTriggerEnter += (b,c) => BulletHelper.CallMethodOnBulletHitObject(b,c, OnBulletHitObject);
        bullet.SetActive(false);
        bulletPool.Add(bullet);

        return bullet;
    }

    /// <summary>
    /// Prøver at hent et deaktiveret skud,
    /// ellers laver den et nyt.
    /// </summary>
    private GameObject GetBullet()
    {
        GameObject bullet = bulletPool.FirstOrDefault(m => m != null && !m.activeSelf);

        if (bullet == null)
        {
            bullet = CreateNewBullet();
        }

        return bullet;
    }

    /// <summary>
    /// flytter skudet
    /// </summary>
    private void Bullet_OnUpdate(GameObject sender)
    {

        if (sender.activeSelf)
        {
            BulletHelper.MoveBulletOnUpdate(sender, speed);

            if (BulletHelper.IsBulletOutOfRange(sender, range))
            {
                BulletHelper.MakeBulletSmallerOnUpdate(sender, 20);
                if (BulletHelper.IsBulletTooSmall(sender))
                {
                    sender.SetActive(false);
                }
            }
        }
    }
    
    #endregion

    ///////////////////////////////
    //      Protected Methods
    ///////////////////////////////
    #region
    /// <summary>
    /// Når et skud rammer et object giv skade hvis det er en fjene.
    /// </summary>
    protected virtual void OnBulletHitObject(GameObject bullet, GameObject target, TargetType type)
    {
        if (target.GetInstanceID() == player.GetInstanceID() || type == TargetType.Teammate || type == TargetType.TeamShield)
        {
            return;
        }

        if (type == TargetType.Enemy || type == TargetType.EnemyShield)
        {
            HealthHelper.GiveDamage(player, target, damage);
        }

        bullet.SetActive(false);
    }

    /// <summary>
    /// Giv ammo når reload Coroutine er færdig
    /// </summary>
    protected override void OnReloadEnd()
    {
        Ammo = ammoMax;
    }
    #endregion
    

}
