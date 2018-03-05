using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BulletHelper
{
    public delegate void OnBulletHitObjectDelegate(GameObject sender, GameObject target, TargetType type);

    private static Dictionary<int, Vector3> bulletStartPosistions = new Dictionary<int, Vector3>();

    public static void UpdateBulletStartPosition(GameObject bullet)
    {
        int key = bullet.GetInstanceID();
        Vector3 position = bullet.transform.position;

        if (bulletStartPosistions.ContainsKey(key))
        {
            bulletStartPosistions[key] = position;
        }
        else
        {
            bulletStartPosistions.Add(key, position);
        }
    }

    public static void MoveBulletOnUpdate(GameObject bullet, float speed)
    {
        bullet.transform.position += bullet.transform.forward * speed * Time.deltaTime;
    }
    public static void MakeBulletSmallerOnUpdate(GameObject bullet, float speed)
    {
        bullet.transform.localScale -= (new Vector3(1, 1, 1) / 10) * 20 * Time.deltaTime;
    }

    public static bool IsBulletTooSmall(GameObject bullet)
    {
        return bullet.transform.localScale.x <= 0 || bullet.transform.localScale.y <= 0 || bullet.transform.localScale.z <= 0;
    }
    public static bool IsBulletOutOfRange(GameObject bullet, float range)
    {
        return Vector3.Distance(bullet.transform.position, bulletStartPosistions[bullet.GetInstanceID()]) > range;
    }


    public static void CallMethodOnBulletHitObject(GameObject sender, Collider col, OnBulletHitObjectDelegate method)
    {
        TargetType type = TargetType.None;

        if (col.gameObject.layer == LayerMask.NameToLayer("Teammate"))
        {
            type = TargetType.Teammate;
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("TeamShield"))
        {
            type = TargetType.TeamShield;
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            type = TargetType.Enemy;
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("EnemyShield"))
        {
            type = TargetType.EnemyShield;
        }

        method(sender, col.gameObject, type);

    }
}
