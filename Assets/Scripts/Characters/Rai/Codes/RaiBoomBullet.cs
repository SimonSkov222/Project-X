using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RaiBoomBullet", menuName = "Abilities/Rai/BoomBullet")]
public class RaiBoomBullet : AbilityBasic
{
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

    private GameObject player;
    private Transform gunEnd;
    private GameObject bullet;
    private Vector3 bulletPosition;
    private bool isExploding = false;
    private List<GameObject> targets = new List<GameObject>();

    public override string Name { get { return ""; } }

    public override void OnLoaded(GameObject characterGo)
    {
        base.OnLoaded(characterGo);
        player = characterGo;
        gunEnd = player.transform.Find(gunEndLocation);

        bullet = Instantiate<GameObject>(bulletModel);
        bullet.SetActive(false);
        UnityEvents ue = bullet.AddComponent<UnityEvents>();

        ue.EventOnUpdate += Bullet_OnUpdate;
        ue.EventOnTriggerEnter += Bullet_OnTriggerEnter;
        ue.EventOnDisable += Ue_EventOnDisable;
    }

    private void Ue_EventOnDisable(GameObject sender)
    {
        isExploding = false;

        targets.Clear();
    }

    private void Bullet_OnTriggerEnter(GameObject sender, Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Teammate"))
        {
            OnBulletHitCollider(col.gameObject, TargetType.Teammate);
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("TeamShield"))
        {
            OnBulletHitCollider(col.gameObject, TargetType.TeamShield);
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnBulletHitCollider(col.gameObject, TargetType.Enemy);
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("EnemyShield"))
        {
            OnBulletHitCollider(col.gameObject, TargetType.EnemyShield);
        }
        else
        {
            OnBulletHitCollider(col.gameObject, TargetType.None);
        }
    }

    private void OnBulletHitCollider(GameObject target, TargetType type)
    {

        Debug.Log("Hit");
        if (type == TargetType.Teammate || type == TargetType.TeamShield)
        {
            return;
        }

        isExploding = true;
        if (type == TargetType.Enemy || type == TargetType.EnemyShield)
        {
            targets.Add(target);
        }

    }
    private bool debug_pause = false;
    private void Bullet_OnUpdate(GameObject sender)
    {
        if (debug_pause) return;
        if (sender.activeSelf)
        {
            if (!isExploding)
            {
                sender.transform.position += sender.transform.forward * bulletSpeed * Time.deltaTime;
            }
            

            if (Vector3.Distance(bulletPosition, sender.transform.position) > range || isExploding)
            {
                sender.transform.localScale += (new Vector3(0.1f, 0.1f, 0.1f) * explodingSpeed * Time.deltaTime);
            }
            if (sender.transform.localScale.x > radius || sender.transform.localScale.y > radius || sender.transform.localScale.z > radius)
            {
                bullet.SetActive(false);
            }
        }
    }

    public override void OnActivate()
    {
        bullet.transform.position = gunEnd.position;
        bullet.transform.rotation = gunEnd.rotation;
        bullet.transform.localScale = bulletModel.transform.localScale;
        bulletPosition = bullet.transform.position;

        bullet.SetActive(true);
    }
}
