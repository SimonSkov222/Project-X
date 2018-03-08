using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//
//  Denne klasse styre hvordan man tager skade og får liv.
//  Hvis der er noget liv der ikke skal tage fuld skade skal 
//  det også definere her, som f.eks. armor og shield
//
////////////////////////////////////////////////////////////////////
public static class HealthHelper {


    ///////////////////////////////
    //      Private Static Fields
    ///////////////////////////////
    private static float armorReduction = 0.05f;
    private static float shieldReduction = 0.1f;
    private static Dictionary<uint, IHealth> healthObjects = new Dictionary<uint, IHealth>();
    private static Dictionary<uint, GameObject> healthGameObjects = new Dictionary<uint, GameObject>();

    ///////////////////////////////
    //      Public Static Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Bruges til debug. Udskriver hvor meget liv
    /// man har tilbage i de forskellige kategorier
    /// </summary>
    public static void PrintHealthInformation(IHealth obj)
    {
        int all = obj.Health + obj.Armor + obj.Shield + obj.HealthBonus;
        Debug.Log(string.Format("Health: {0}, Armor: {1}, Shield: {2}, Bonus: {4}, All: {3}", obj.Health, obj.Armor, obj.Shield, all, obj.HealthBonus));
    }

     /// <summary>
    /// Giver objectet fuld liv. også i de andre typer liv(armor, shield)
    /// </summary>
    public static void Initialize(uint netID, GameObject obj)
    {
        Debug.Log("Added: " + obj.name + ", ID: " + netID);

        IHealth obj2 = (IHealth)obj.GetComponent(typeof(IHealth));

        if (!healthObjects.ContainsKey(netID))
        {
            healthObjects.Add(netID, obj2);
            healthGameObjects.Add(netID, obj);
        }

        obj2.Health = obj2.HealthMax;
        obj2.Armor = obj2.ArmorMax;
        obj2.Shield = obj2.ShieldMax;
    }

    /// <summary>
    /// Giver skade til objecet forskellige typer liv i den rigtige rækkefølge.
    /// Tager også højde for om objecet skal ekstra skade via. IHealth.WeaknessMultiplier
    /// 
    /// De forskellige typer liv vil ikke kunne gå i minus.
    /// 
    /// Når objecet ikke har mere liv tilbage vil denne metode også kalde IHealth.OnDeath();
    /// </summary>

    public static void GiveDamage(GameObject sender, GameObject target, int damage)
    {
        uint senderID = sender.GetComponent<NetworkIdentity>().netId.Value;
        uint targetID = target.GetComponent<NetworkIdentity>().netId.Value;

        GiveDamage(senderID, targetID, damage);
    }

    public static void GiveDamageExecute(uint senderNetID, uint targetNetID, int damage)
    {
        IHealth objTarget = healthObjects[targetNetID];

        //Giv objectet skade
        if (objTarget.HealthBonus > 0)
        {
            objTarget.HealthBonus = GetValueAfterDamageIsDone(objTarget.HealthBonus, damage, 0, out damage);
        }

        if (objTarget.Shield > 0)
        {
            objTarget.Shield = GetValueAfterDamageIsDone(objTarget.Shield, damage, shieldReduction, out damage);
        }

        if (objTarget.Armor > 0)
        {
            objTarget.Armor = GetValueAfterDamageIsDone(objTarget.Armor, damage, armorReduction, out damage);
        }

        objTarget.Health = GetValueAfterDamageIsDone(objTarget.Health, damage, 0, out damage);



        //Object er død kald død metoden
        if (objTarget.Health == 0)
        {
            objTarget.OnDeath(objTarget);
        }
    }
    public static void GiveDamage(uint senderNetID, uint targetNetID, int damage)
    {
        IHealth objTarget = healthObjects[targetNetID];
        IHealth objSender = healthObjects[senderNetID];
        if (objTarget == null) return;

        //Hvis object har en magic over sig selv der gør at den tager mere skade
        int dmgExtra = Mathf.RoundToInt(damage * objTarget.WeaknessMultiplier);
        damage += dmgExtra;
        
        objTarget.OnTakeDmg(healthGameObjects[senderNetID], damage);
        objSender.OnGiveDmg(healthGameObjects[targetNetID], damage);
    }

    /// <summary>
    /// Giver healer objecet forskellige typer liv i den rigtige rækkefølge.
    /// de forskellige typer liv ikke kunne går over deres maximum.
    /// </summary>
    public static void GiveHealth(IHealth obj, int health)
    {
        if (obj.Health < obj.HealthMax)
        {
            obj.Health = GetValueAfterHealthIsDone(obj.Health, obj.HealthMax, health, out health);
        }

        if (obj.Armor < obj.ArmorMax)
        {
            obj.Armor = GetValueAfterHealthIsDone(obj.Armor, obj.ArmorMax, health, out health);
        }

        if (obj.Shield < obj.ShieldMax)
        {
            obj.Shield = GetValueAfterHealthIsDone(obj.Shield, obj.ShieldMax, health, out health);
        }
    }

    #endregion

    ///////////////////////////////
    //      Private Static Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Ligger værdierne sammen hvis der er noget 
    /// tilovers bliver det også sendt tilbage.
    /// </summary>
    private static int GetValueAfterHealthIsDone(int value, int max, int health, out int healthLeft)
    {
        value += health;

        if (value > max)
        {
            healthLeft = value - max;
            value = max;
        }
        else
        {
            healthLeft = 0;
        }

        return value;
    }
    /// <summary>
    /// trækker fra værdien og hvis den går i minus, bliver det der i minus også sendt tilbage
    /// og værdien vil blive 0.
    /// </summary>
    private static int GetValueAfterDamageIsDone(int value, int damage, float reductionPercent, out int damageLeft)
    {
        int reduction = Mathf.RoundToInt(damage * reductionPercent);
        damage -= reduction;
        

        // Må ikke blive mindre end 0 så vi ikke ender med at heale
        if (damage < 0)
        {
            damage = 0;
        }

        //Vi skal sørger for at kunne give resten af skaden vidre
        if (damage > value)
        {
            damageLeft = damage - value;
            value = 0;
        }
        else
        {
            damageLeft = 0;
            value -= damage;

        }

        return value;
    }
    #endregion
}
