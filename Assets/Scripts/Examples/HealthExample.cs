using UnityEngine;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//
//  Dette er kun et eksempel på hvordan man kan bruge IHealth
//
////////////////////////////////////////////////////////////////////
public class HealthExample : MonoBehaviour, IHealth {


    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    [SerializeField]
    [Range(0, 1000)]
    private int m_HealthMax = 250;

    [SerializeField]
    [Range(0, 1000)]
    private int m_ArmorMax = 75;

    [SerializeField]
    [Range(0, 1000)]
    private int m_ShieldMax = 75;


    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public int HealthMax { get { return m_HealthMax; } }
    public int ArmorMax { get { return m_ArmorMax; } }
    public int ShieldMax { get { return m_ShieldMax; } }

    public int Health { get; set; }
    public int Armor { get; set; }
    public int Shield { get; set; }
    public int HealthBonus { get; set; }

    public float WeaknessMultiplier { get; set; }


    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////

    /// <summary>
    /// Bliver kaldt når objecet dør
    /// </summary>
    public void OnDeath(object sender)
    {
        Debug.Log("You are dead. :(");
    }


    ///////////////////////////////
    //      Unity Metods
    ///////////////////////////////

    /// <summary>
    /// Giver objecet fuld liv og udskriver det i console
    /// </summary>
    void Start () {
        HealthHelper.Initialize(this);
        HealthHelper.PrintHealthInformation(this);
        WeaknessMultiplier = 0.1f;
    }
	
    /// <summary>
    /// Tryk på P for at tage skade.
    /// Hold O nede for at få liv igen.
    /// Tryk på I for at få bonus liv.
    /// </summary>
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            HealthHelper.GiveDamage(this, 75);
            HealthHelper.PrintHealthInformation(this);
        }

        if (Input.GetKey(KeyCode.O))
        {

            HealthHelper.GiveHealth(this, 1);
            HealthHelper.PrintHealthInformation(this);
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            HealthBonus += 100;
            HealthHelper.PrintHealthInformation(this);
        }
    }
}
