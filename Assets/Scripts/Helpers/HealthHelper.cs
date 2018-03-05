using UnityEngine;

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
    public static void Initialize (IHealth obj)
    {
        obj.Health  = obj.HealthMax;
        obj.Armor   = obj.ArmorMax;
        obj.Shield  = obj.ShieldMax;
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
        IHealth obj = (IHealth)target.GetComponent(typeof(IHealth));
        
        //Hvis object har en magic over sig selv der gør at den tager mere skade
        int dmgExtra = Mathf.RoundToInt(damage * obj.WeaknessMultiplier);
        damage += dmgExtra;
        
        obj.OnTakeDmg(sender, damage);

        //Giv objectet skade
        if (obj.HealthBonus > 0)
        {
            obj.HealthBonus = GetValueAfterDamageIsDone(obj.HealthBonus, damage, 0, out damage);
        }

        if (obj.Shield > 0)
        {
            obj.Shield = GetValueAfterDamageIsDone(obj.Shield, damage, shieldReduction, out damage);
        }

        if (obj.Armor > 0)
        {
            obj.Armor = GetValueAfterDamageIsDone(obj.Armor, damage, armorReduction, out damage);
        }

        obj.Health = GetValueAfterDamageIsDone(obj.Health, damage, 0, out damage);



        //Object er død kald død metoden
        if (obj.Health == 0)
        {
            obj.OnDeath(obj);
        }
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

        Debug.Log("reduction: " + reduction);

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
