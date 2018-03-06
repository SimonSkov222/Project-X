using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//
//
//////////////////////////////////////////////////////
public static class BulletHelper
{

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    #region
    public delegate void OnBulletHitObjectDelegate(GameObject sender, GameObject target, TargetType type);
    #endregion


    ///////////////////////////////
    //      Private Static Fields
    ///////////////////////////////
    #region
    private static Dictionary<int, Vector3> bulletStartPosistions = new Dictionary<int, Vector3>();
    #endregion


    ///////////////////////////////
    //      Public Static Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Opdatere gameobjecetet position.
    /// bruges når der skal måles afstand(IsBulletOutOfRange())
    /// </summary>
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
   
    /// <summary>
    /// Skal blive kaldt i Update().
    /// flytter gameobjecetet i en retning(Vector3.forward)
    /// </summary>
    public static void MoveBulletOnUpdate(GameObject bullet, float speed)
    {
        bullet.transform.position += bullet.transform.forward * speed * Time.deltaTime;
    }

    /// <summary>
    /// Skal blive kaldt i Update().
    /// gør objectet mindre
    /// </summary>
    public static void MakeBulletSmallerOnUpdate(GameObject bullet, float speed)
    {
        bullet.transform.localScale -= (new Vector3(1, 1, 1) / 10) * 20 * Time.deltaTime;
    }

    /// <summary>
    /// Tjekker om en af gameobjecet localScale(x,y,z) er mindre end 0
    /// </summary>
    /// <param name="bullet"></param>
    /// <returns></returns>
    public static bool IsBulletTooSmall(GameObject bullet)
    {
        return bullet.transform.localScale.x <= 0 || bullet.transform.localScale.y <= 0 || bullet.transform.localScale.z <= 0;
    }
    
    /// <summary>
    /// Tjekker om gameobjecetet er for langt væk.
    /// UpdateBulletStartPosition() skal bruges
    /// </summary>
    /// <param name="bullet"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static bool IsBulletOutOfRange(GameObject bullet, float range)
    {
        return Vector3.Distance(bullet.transform.position, bulletStartPosistions[bullet.GetInstanceID()]) > range;
    }

    /// <summary>
    /// Tjekker om det object der blivet ramt har en layer for team eller fjene.
    /// og kalder den opgivet metode med info om objecet der blev ramt 
    /// </summary>
    public static void CallMethodOnBulletHitObject(GameObject sender, Collider col, OnBulletHitObjectDelegate method)
    {
        TargetType type = TargetType.None;

        if (col.isTrigger)
        {
            return;
        }

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
    
    #endregion





}
