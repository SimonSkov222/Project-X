using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IHealth
{
    public delegate void OnEvent();
    public event OnEvent OnHitGround;


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


    [Header("Movement Settings")]
    [SerializeField]
    [Range(1, 100)]
    protected int walkSpeed = 1;
    [SerializeField]
    [Range(1, 100)]
    protected int runSpeed = 1;
    [SerializeField]
    [Range(1, 100)]
    protected float jumpHeight = 1;


    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    public float sensitivity = 2.0f;
    public float gravity = 20.0f;
    //public float maxHeight = 1.6f;
    //public float minHeight = 1.3f;
    //public float heightSmooth = 5f;
    public float smoothing = 2.0f;


    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private Vector2 mouseLock;
    private Vector2 smoothV;
    private CharacterController player;
    private Vector3 movement = Vector3.zero;
    private float crouchR = 1f;
    private float moveFB;
    private float moveLR;
    private bool hasCallGroundEvent = false;
    //private float rotX;
    //private float rotY;
    //private bool crouch;


    [Header("Attack Settings")]
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

    
    private WeaponBasic m_weapon;
    private UltimateBasic m_ultimate;
    private AbilityBasic m_ability1;
    private AbilityBasic m_ability2;
    private AbilityBasic m_ability3;



    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    public static PlayerController Instance { get; private set; }

    public WeaponBasic Weapon { get { if (weapon != null && m_weapon == null) m_weapon = Instantiate(weapon); return m_weapon; } }
    public UltimateBasic Ultimate { get { if (ultimate != null && m_ultimate == null) m_ultimate = Instantiate(ultimate); return m_ultimate; } }
    public AbilityBasic Ability1 { get { if (ability1 != null && m_ability1 == null) m_ability1 = Instantiate(ability1); return m_ability1; } }
    public AbilityBasic Ability2 { get { if (ability2 != null && m_ability2 == null) m_ability2 = Instantiate(ability2); return m_ability2; } }
    public AbilityBasic Ability3 { get { if (ability3 != null && m_ability3 == null) m_ability3 = Instantiate(ability3); return m_ability3; } }




    public int HealthMax { get { return healthMax; } }
    public int ArmorMax { get { return armorMax; } }
    public int ShieldMax { get { return shieldMax; } }

    public int Health { get; set; }
    public int Armor { get; set; }
    public int Shield { get; set; }
    public int HealthBonus { get; set; }


    public float JumpHeight { get { return jumpHeight; } }
    public float Speed { get { return runSpeed; } }
    public Camera Eyes { get { return Camera.main; } }

    public float WeaknessMultiplier
    {
        get; set;


    }

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Vi gemmer nogle components og kameraet på gameobjectet.
    /// </summary>
    void Start()
    {
        player = GetComponent<CharacterController>();
        ButtonHelper.TryCallMethod(weapon, "OnLoaded", gameObject);
        ButtonHelper.TryCallMethod(Ability2, "OnLoaded", gameObject);
        ButtonHelper.TryCallMethod(Ability3, "OnLoaded", gameObject);
        ButtonHelper.TryCallMethod(Ability3, "OnLoaded", gameObject);

        IsMouseLocked(true);
    }


    /// <summary>
    /// Her er hvordan kontrol over gameobjectet bliver udført.
    /// </summary>
    void Update()
    {
        RotateView();
        CalculateMovement();

        ButtonHelper.InvokeButtonEvents(this, "Jump", "OnJump");

        //if (Time.time - timeWeapon >= character.Weapon.FireRate)
        //{
        //    if (ButtonHelper.InvokeButtonEvents(character.Weapon, "Fire1", "OnButton"))
        //    {
        //        timeWeapon = Time.time;
        //    }
        //}
        ButtonHelper.InvokeButtonEvents(weapon, "Fire1", "OnFire");
        ButtonHelper.InvokeButtonEvents(weapon, "Reload", "OnReload");


        ButtonHelper.InvokeButtonEvents(this, "Reload", "OnReload");



        ButtonHelper.InvokeButtonEvents(this, "Ultimate", "OnUltimate");

        AbilityUpdate(Ability1, "Ability1");
        AbilityUpdate(Ability2, "Ability2");
        AbilityUpdate(Ability3, "Ability3");

        ApplyGravity();
        player.Move(movement * Time.deltaTime);
    }

    private void AbilityUpdate(AbilityBasic a, string button)
    {
        if (a == null)
        {
            return;
        }

        if (a.UseButton)
        {
            ButtonHelper.InvokeButtonEvents(a, button, "OnButton", 0f);
        }
        else
        {
            a.OnActivate();
        }
    }

    public void OnJump_Click()
    {
        if (player.isGrounded)
        {
            MakePlayerJump(JumpHeight);
        }
    }

    public void MakePlayerJump(float height)
    {
        movement.y = height;
    }

    private void ApplyGravity()
    {
        if (!player.isGrounded)
        {
            hasCallGroundEvent = false;
            movement.y -= gravity * Time.deltaTime;
        }
        else if (player.isGrounded && !hasCallGroundEvent)
        {
            hasCallGroundEvent = true;
            if (OnHitGround != null)
            {
                OnHitGround();
            }
            
        }
    }


    ///////////////////////////////
    //      Private Method
    ///////////////////////////////

    /// <summary>
    /// Får character til at bevæge sig den retning man vil
    /// </summary>
    private void CalculateMovement()
    {
        moveFB = Input.GetAxis("Vertical") * Speed * crouchR;
        moveLR = Input.GetAxis("Horizontal") * Speed * crouchR;

        movement = new Vector3(moveLR, movement.y, moveFB);

        // Gør at tasterne passer iforhold til hvor vi kigger
        movement = transform.rotation * movement;
    }

    /// <summary>
    /// Bevæger kameraet med musen
    /// </summary>
    private void RotateView()
    {
        var md = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // Ganger den første vector2 x og y med den anden vector2 x og y
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));


        // Gør at man hurtigt langsomt stopper hovedet (som hvis du fader noget ud)
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);

        // Gør at vi ikke kan lave en 360 rundt nede og oppe
        mouseLock += smoothV;
        mouseLock.y = Mathf.Clamp(mouseLock.y, -75f, 75f);

        // Her rotere vi kameraet og kroppe
        Eyes.transform.localRotation = Quaternion.AngleAxis(-mouseLock.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(mouseLock.x, player.transform.up);

    }

    public void OnDeath(object sender)
    {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// Her kan vi køre musen synlig eller usynlig.
    /// </summary>
    /// <param name="val"></param>
    private void IsMouseLocked(bool val)
    {

        Cursor.visible = !val;
        Cursor.lockState = val ? CursorLockMode.Locked : CursorLockMode.None;
    }
}