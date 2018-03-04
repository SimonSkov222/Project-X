using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RaiMainWeapon", menuName = "Weapons/Rai/MainWeapon")]
public class RaiWeapon : WeaponBasic
{
    //[SerializeField]
    protected Transform gunEnd;

    [SerializeField]
    protected GameObject bulletModel;

    [SerializeField]
    protected int bulletPoolSize;
    [SerializeField]
    protected string gunEndLocation = "Eyes/Weapons/MainGun/GunEnd";
    private List<GameObject> bulletPool = new List<GameObject>();
    private Dictionary<int, Vector3> bulletPositions = new Dictionary<int, Vector3>();

    public override void OnLoaded(GameObject characterGo)
    {
        base.OnLoaded(characterGo);

        Ammo = ammoMax;

        gunEnd = player.transform.Find(gunEndLocation);

        for (int i = 0; i < bulletPoolSize; i++)
        {
            CreateNewBullet();
        }
    }

    public override void OnFire()
    {

        if (Ammo > 0 && !IsReloading)
        {
            GameObject bullet = GetBullet();
            bullet.transform.position = gunEnd.position;
            bullet.transform.rotation = gunEnd.rotation;
            bullet.transform.localScale = bulletModel.transform.localScale;
            bulletPositions[bullet.GetInstanceID()] = bullet.transform.position;
            bullet.SetActive(true);

            Ammo--;
        }
        else
        {
            OnReload_Down();
        }
    }

    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate<GameObject>(bulletModel);
        UnityEvents ue = bullet.AddComponent<UnityEvents>();
        ue.EventOnUpdate += Bullet_OnUpdate;
        ue.EventOnTriggerEnter += Bullet_OnTriggerEnter;
        bullet.SetActive(false);
        bulletPool.Add(bullet);
        bulletPositions.Add(bullet.GetInstanceID(), Vector3.zero);

        return bullet;
    }
    
    private void Bullet_OnTriggerEnter(GameObject sender, Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Teammate"))
        {
            OnBulletHitTarget(sender, col.gameObject, TargetType.Teammate);
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("TeamShield"))
        {
            OnBulletHitTarget(sender, col.gameObject, TargetType.TeamShield);
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnBulletHitTarget(sender, col.gameObject, TargetType.Enemy);
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("EnemyShield"))
        {
            OnBulletHitTarget(sender, col.gameObject, TargetType.EnemyShield);
        }
        else
        {
            sender.SetActive(false);
        }
    }

    private void Bullet_OnUpdate(GameObject sender)
    {

        if (sender.activeSelf)
        {
                
            sender.transform.position += sender.transform.forward * 2 * Time.deltaTime;

            if (Vector3.Distance(bulletPositions[sender.GetInstanceID()], sender.transform.position) > 10)
            {
                sender.transform.localScale -=  (new Vector3(0.1f, 0.1f, 0.1f) * 0.5f  *Time.deltaTime);
                if (BulletIsTooSmall(sender.transform.localScale))
                {
                    sender.SetActive(false);
                }
            }
        }
    }

    private bool BulletIsTooSmall(Vector3 scale)
    {
        return scale.x <= 0f || scale.y <= 0f || scale.z <= 0f;
    }

    protected virtual void OnBulletHitTarget(GameObject bullet, GameObject target, TargetType type)
    {

    }

    private GameObject GetBullet()
    {
        GameObject bullet = bulletPool.FirstOrDefault(m => m != null && !m.activeSelf);

        if (bullet == null)
        {
            bullet = CreateNewBullet();
        }

        return bullet;
    }


    protected override void OnReloadStart()
    {
    }

    protected override void OnReloadEnd()
    {
        Ammo = ammoMax;
    }
}
