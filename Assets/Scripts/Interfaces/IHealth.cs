using UnityEngine;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//
//  De properties et scirpt skal have for kunne sige at det er i live.
//
////////////////////////////////////////////////////////////////////
public interface IHealth
{

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

    void OnTakeDmg(GameObject sender, int dmg);
}
