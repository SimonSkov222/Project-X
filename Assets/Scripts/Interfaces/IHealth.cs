using UnityEngine;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//
//  De properties et scirpt skal have for kunne sige at det er i live.
//
////////////////////////////////////////////////////////////////////
public interface IHealth
{
    event SimpleHealth.OnDeathDelegate EventOnDeath;
    event SimpleHealth.OnDamageDelegate EventOnGiveDamage;
    event SimpleHealth.OnDamageDelegate EventOnTakeDamage;

    int HealthMax { get; }
    int ArmorMax { get; }
    int ShieldMax { get; }
    
    float WeaknessMultiplier { get; set; }

    int Health { get; set; }
    int Armor { get; set; }
    int Shield { get; set; }
    int HealthBonus { get; set; }


    /// <summary>
    /// Bliver kaldt når objecet dør via helper class
    /// </summary>
    void OnDeath(object sender);

    /// <summary>
    /// Bliver kaldt når objecet targer skade
    /// </summary>
    void OnTakeDmg(GameObject sender, int dmg);

    /// <summary>
    /// Bliver kaldt når objecet giver skade
    /// </summary>
    void OnGiveDmg(GameObject target, int dmg);
}
