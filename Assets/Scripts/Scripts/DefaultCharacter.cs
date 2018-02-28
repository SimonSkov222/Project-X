using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Characters/DefaultCharacter")]
public class DefaultCharacter : MonoBehaviour, IHealth {
    const float ClickTime = 0.4f;

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private float[] timeButtonDown = new float[5];
    private string[] buttons = { "Fire1" };

    ///////////////////////////////
    //      Protected Fields
    ///////////////////////////////
    [Header("Health Settings")]
    [SerializeField]
    [Range(0, 1000)]
    protected int healthMax = 100;

    [SerializeField]
    [Range(0, 1000)]
    protected int armorMax = 0;

    [SerializeField]
    [Range(0, 1000)]
    protected int shieldMax = 0;
    

    [Header("Settings")]
    [SerializeField]
    protected WeaponBasic weapon;
    [SerializeField]
    protected UltimateBasic ultimate;
    [SerializeField]
    protected AbilityBasic ability1;
    [SerializeField]
    protected AbilityBasic ability2;
    [SerializeField]
    protected AbilityBasic ability3;


    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public int HealthMax { get { return healthMax; } }
    public int ArmorMax { get { return armorMax; } }
    public int ShieldMax { get { return shieldMax; } }

    public int Health { get; set; }
    public int Armor { get; set; }
    public int Shield { get; set; }
    public int HealthBonus { get; set; }

    public float WeaknessMultiplier { get; set; }

    ///////////////////////////////
    //      Private Properties
    ///////////////////////////////
    

    void Start () {
        HealthHelper.Initialize(this);
	}
	
	// Update is called once per frame
	void Update () {


        for (int i = 0; i < buttons.Length; i++)
        {
            IButton button = GetButton(i);

            if (Input.GetButtonDown(buttons[i]))
            {
                timeButtonDown[i] = Time.time;
                button.OnButtonDown(gameObject, null);
            }
            if (Input.GetButton(buttons[i]))
            {
                button.OnButtonHold(gameObject, null);
            }
            if (Input.GetButtonUp(buttons[i]))
            {
                float pressTime = Time.time - timeButtonDown[i];
                button.OnButtonUp(gameObject, null);

                if (pressTime <= ClickTime)
                {
                    button.OnButtonClick(gameObject, null);
                }
            }
        }

	}

    private IButton GetButton(int id)
    {
        return null;
    }


    protected virtual void OnWeapon_Hold(GameObject player, object weapon) { Debug.Log("OnWeapon_Hold"); }
    protected virtual void OnWeapon_Down(GameObject player, object weapon) { Debug.Log("OnWeapon_Down"); }
    protected virtual void OnWeapon_Up(GameObject player, object weapon) { Debug.Log("OnWeapon_Up"); }
    protected virtual void OnWeapon_Click(GameObject player, object weapon) { Debug.Log("OnWeapon_Click"); }

    //protected virtual void OnUltimate_Hold(GameObject player, object weapon)       { Debug.Log("OnUltimate_Hold"); }
    //protected virtual void OnUltimate_Down(GameObject player, object weapon)        { Debug.Log("OnUltimate_Down"); }
    //protected virtual void OnUltimate_Up(GameObject player, object weapon)          { Debug.Log("OnUltimate_Up"); }
    //protected virtual void OnUltimate_Click(GameObject player, object weapon)       { Debug.Log("OnUltimate_Click"); }

    //protected virtual void OnAbility1_Hold(GameObject player, object weapon)       { Debug.Log("OnAbility1_Hold"); }
    //protected virtual void OnAbility1_Down(GameObject player, object weapon)        { Debug.Log("OnAbility1_Down"); }
    //protected virtual void OnAbility1_Up(GameObject player, object weapon)          { Debug.Log("OnAbility1_Up"); }
    //protected virtual void OnAbility1_Click(GameObject player, object weapon)       { Debug.Log("OnAbility1_Click"); }

    //protected virtual void OnAbility2_Hold(GameObject player, object weapon)       { Debug.Log("OnAbility2_Hold"); }
    //protected virtual void OnAbility2_Down(GameObject player, object weapon)        { Debug.Log("OnAbility2_Down"); }
    //protected virtual void OnAbility2_Up(GameObject player, object weapon)          { Debug.Log("OnAbility2_Up"); }
    //protected virtual void OnAbility2_Click(GameObject player, object weapon)       { Debug.Log("OnAbility2_Click"); }

    //protected virtual void OnAbility3_Hold(GameObject player, object weapon)       { Debug.Log("OnAbility3_Hold"); }
    //protected virtual void OnAbility3_Down(GameObject player, object weapon)        { Debug.Log("OnAbility3_Down"); }
    //protected virtual void OnAbility3_Up(GameObject player, object weapon)          { Debug.Log("OnAbility3_Up"); }
    //protected virtual void OnAbility3_Click(GameObject player, object weapon)       { Debug.Log("OnAbility3_Click"); }


    public virtual void OnDeath(object sender)
    {
    }

    public virtual void OnSpawn(object sender)
    {
    }


}
