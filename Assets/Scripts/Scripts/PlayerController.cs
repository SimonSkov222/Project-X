using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

//////////////////////////////////////////////////////
//      Beskrivelse
//
//  Dette script giver gameobjecetet liv og at man kan 
//  flytte/styre det.
//
//  Den styre også hvordan WeaponBasic,UltimateBasic og AbilityBasic
//  metoder bliver kaldt
//  
//////////////////////////////////////////////////////
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : NetworkBehaviour, IHealth
{

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    #region
    public delegate void OnEvent();
    public event OnEvent OnHitGround;
    public event SimpleHealth.OnDeathDelegate EventOnDeath;
    public event SimpleHealth.OnDamageDelegate EventOnGiveDamage;
    public event SimpleHealth.OnDamageDelegate EventOnTakeDamage;

    public float sensitivity = 2.0f;
    public float gravity = 20.0f;
    public float smoothing = 2.0f;
    public Camera eyes;
    #endregion

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region
    private Vector2 mouseLock;
    private Vector2 smoothV;
    private CharacterController player;
    private Vector3 movement = Vector3.zero;
    private float crouchR = 1f;
    private float moveFB;
    private float moveLR;
    private bool hasCallGroundEvent = false;

    private WeaponBasic m_weapon;
    private UltimateBasic m_ultimate;
    private AbilityBasic m_ability1;
    private AbilityBasic m_ability2;
    private AbilityBasic m_ability3;
    #endregion

    ///////////////////////////////
    //      Protected Fields
    ///////////////////////////////
    #region
    [Header("Height Settings")]
    [SerializeField]
    protected float maxHeight = 1.6f;
    [SerializeField]
    protected float minHeight = 1.3f;
    [SerializeField]
    protected float heightSmooth = 5f;

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
    #endregion

    ///////////////////////////////
    //      Public Properties
    ///////////////////////////////
    #region
    public WeaponBasic Weapon { get { if (weapon != null && m_weapon == null) m_weapon = Instantiate(weapon); return m_weapon; } }
    public UltimateBasic Ultimate { get { if (ultimate != null && m_ultimate == null) m_ultimate = Instantiate(ultimate); return m_ultimate; } }
    public AbilityBasic Ability1 { get { if (ability1 != null && m_ability1 == null) m_ability1 = Instantiate(ability1); return m_ability1; } }
    public AbilityBasic Ability2 { get { if (ability2 != null && m_ability2 == null) m_ability2 = Instantiate(ability2); return m_ability2; } }
    public AbilityBasic Ability3 { get { if (ability3 != null && m_ability3 == null) m_ability3 = Instantiate(ability3); return m_ability3; } }


    public float WeaknessMultiplier { get; set; }
    public int HealthMax { get { return healthMax; } }
    public int ArmorMax { get { return armorMax; } }
    public int ShieldMax { get { return shieldMax; } }

    public int Health { get; set; }
    public int Armor { get; set; }
    public int Shield { get; set; }
    public int HealthBonus { get; set; }

    public float JumpHeight { get { return jumpHeight; } }
    public float Speed { get { return runSpeed; } }
    public Camera Eyes { get { return eyes; } }
    #endregion



    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////
    #region

    //public void InvokeStart() { Start(); }

    /// <summary>
    /// Vi gemmer nogle components loader våben og evner
    /// </summary>
    void Start()
    {
        HealthHelper.Initialize(GetComponent<NetworkIdentity>().netId.Value, gameObject);
        player = GetComponent<CharacterController>();
        //OnLoaded();
        ButtonHelper.TryCallMethod(Weapon, "OnLoaded", gameObject);
        ButtonHelper.TryCallMethod(Ability1, "OnLoaded", gameObject);
        ButtonHelper.TryCallMethod(Ability2, "OnLoaded", gameObject);
        ButtonHelper.TryCallMethod(Ability3, "OnLoaded", gameObject);

        IsMouseLocked(true);

        if (isLocalPlayer)
        {
            GuiPlayer.Singleton.Player = gameObject;
        }
    }


    /// <summary>
    /// Her er hvordan kontrol over gameobjectet bliver udført.
    /// hvilken taster der flytter gameobjecetet og aktiver våben/evner
    /// </summary>
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        RotateView();
        CalculateMovement();

        //ButtonHelper.InvokeButtonEvent(this, "Test", OnFire3);

        ButtonHelper.InvokeButtonEvent(this, "Fire1", OnButtonPress, "Fire");
        ButtonHelper.InvokeButtonEvent(this, "Reload", OnButtonPress, "Reload");
        ButtonHelper.InvokeButtonEvent(this, "Crouch", OnButtonPress, "Crouch");
        //ButtonHelper.InvokeButtonEvent(this, "Test", OnFire3);
        //ButtonHelper.InvokeButtonEvent(this, "Test", OnFire3);

        ButtonHelper.InvokeButtonEvents(this, "Jump", "OnJump");
        //ButtonHelper.InvokeButtonEvents(this, "Crouch", "OnCrouch");
        //ButtonHelper.InvokeButtonEvents(weapon, "Fire1", "OnFire");
        //ButtonHelper.InvokeButtonEvents(weapon, "Reload", "OnReload");
        //ButtonHelper.InvokeButtonEvents(this, "Reload", "OnReload");
        //ButtonHelper.InvokeButtonEvents(this, "Ultimate", "OnUltimate");

        AbilityUpdate(Ability1, "Ability1");
        AbilityUpdate(Ability2, "Ability2");
        AbilityUpdate(Ability3, "Ability3");


        ApplyGravity();
        player.Move(movement * Time.deltaTime);
    }

    [Client]
    public void OnButtonPress(object sender, ButtonCall clickType, params object[] args)
    {
        CmdOnButtonPress(gameObject.name, clickType, (string)args[0]);
    }

    
    [Command]
    public void CmdOnButtonPress(string _ID, ButtonCall clickType, string code)
    {
        RpcOnButtonPress(_ID, clickType, code);
    }

    [ClientRpc]
    public void RpcOnButtonPress(string _ID, ButtonCall clickType, string code)
    {
        PlayerController pc = GameObject.Find(_ID).GetComponent<PlayerController>();
        string method = "";
        object sender = null;
        object[] parameters = { };

        switch (code)
        {
            case "Fire": method = "OnFire"; sender = pc.Weapon; break;
            case "Reload": method = "OnReload"; sender = pc.Weapon; break;
            case "Crouch": method = "OnCrouch"; sender = pc; break;
            case "Ability1": method = "OnButton"; sender = pc.Ability1; parameters = new object[]{ 0 }; break;
            case "Ability2": method = "OnButton"; sender = pc.Ability2; parameters = new object[] { 0 }; break;
            case "Ability3": method = "OnButton"; sender = pc.Ability3; parameters = new object[] { 0 }; break;
        }

        TryInvokeMethod(sender, method, clickType, parameters);
    }

    private bool TryInvokeMethod(object sender, string method, ButtonCall clickType, params object[] parameters)
    {
        if (sender == null)
        {
            return false;
        }
        string prefix = "";

        switch (clickType)
        {
            case ButtonCall.Down: prefix = "_Down";  break;
            case ButtonCall.Up: prefix = "_Up"; break;
            case ButtonCall.Hold: prefix = "_Hold"; break;
            case ButtonCall.Click: prefix = "_Click"; break;
        }
        Type senderType = sender.GetType();
        MethodInfo methodInfo = senderType.GetMethod(method+ prefix);
        if (methodInfo != null)
        {
            methodInfo.Invoke(sender, parameters);
            return true;
        }
        return false;
    }

    #endregion

    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////
    #region


    public void OnDeath(object sender)
    {
        Debug.Log("Player: Dead");
    }

    public void OnGiveDmg(GameObject target, int dmg)
    {
       // Debug.Log("Player: Give Damge");
    }
    public void OnTakeDmg(GameObject sender, int dmg)
    {
        //Debug.Log("Player: Taken Damge");

        if (EventOnTakeDamage != null)
        {
            EventOnTakeDamage(sender, gameObject, dmg);
        }
    }

    /// <summary>
    /// Får spiller til at hoppe
    /// </summary>
    public void OnJump_Down()
    {
        if (player.isGrounded)
        {
            MakePlayerJump(JumpHeight);
        }
    }

    /// <summary>
    /// Gør at vi kan dukke os når vi trykker på ctrl
    /// </summary>
    public void OnCrouch_Hold()
    {
        StartCoroutine(StopCrouching());
    }

    /// <summary>
    /// Gør at vi stopper med at dukke, når vi giver slip på ’ctrl’
    /// </summary>
    public void OnCrouch_Up()
    {
        StartCoroutine(StopCrouching());
    }

    #endregion

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    #region
    /// <summary>
    /// Kan få gameobjecet til at hoppe med en bestem højde
    /// </summary>
    public void MakePlayerJump(float height)
    {
        movement.y = height;
    }
    

    /// <summary>
    /// Bruges til at kunne aktivere en evne script
    /// </summary>
    private void AbilityUpdate(AbilityBasic a, string button)
    {
        if (a == null)
        {
            return;
        }

        if (a.UseButton)
        {
            ButtonHelper.InvokeButtonEvent(this, button, OnButtonPress, button);
        }
        else
        {
            a.OnActivate();
        }
    }

    /// <summary>
    /// Får character til at bevæge sig den retning man vil
    /// </summary>
    /// 
    [Client]
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
    /// 
    [Client]
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

    /// <summary>
    /// Giver tyngdekraft til gameobjecetet
    /// </summary>
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

    /// <summary>
    /// Her kan vi køre musen synlig eller usynlig.
    /// </summary>
    /// <param name="val"></param>
    private void IsMouseLocked(bool val)
    {
        Cursor.visible = !val;
        Cursor.lockState = val ? CursorLockMode.Locked : CursorLockMode.None;
    }


    /// <summary>
    /// Gør at vi kan dukke os
    /// </summary>
    private IEnumerator StartCrouching()
    {
        while (player.height != minHeight)
        {
            var value = heightSmooth * Time.deltaTime;
            var center = player.center;
            crouchR = 0.6f;

            // her sørger vi får at det tager tid for at dykke sig
            if (player.height > minHeight)
            {
                player.height -= value;
                center.y += value / 2;
            }

            // Her sætter vi character controllerns mide og højte så den passer til character
            if (player.height < minHeight)
            {

                center.y -= (minHeight - player.height) / 2;
                player.height = minHeight;
            }
            player.center = center;
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Gør at vi stopper med at dukke
    /// </summary>
    private IEnumerator StopCrouching()
    {
        while (player.height != maxHeight)
        {
            var value = heightSmooth * Time.deltaTime;
            var center = player.center;
            crouchR = 1f;

            // her sørger vi får at det tager tid for at rejse sig
            if (player.height < maxHeight)
            {
                player.height += value;
                center.y -= value / 2;
            }
            // Her sætter vi character controllerns mide og højte så den passer til character
            if (player.height > maxHeight)
            {
                center.y += (player.height - maxHeight) / 2;
                player.height = maxHeight;
            }

            player.center = center;
            yield return new WaitForFixedUpdate();
        }
    }


    /// <summary>
    /// Sørger for at vi kun kan rejse os op når der ikke er noget oven over os.
    /// Vi laver max 4 raycast som tjekker hvert hjørne af 
    /// character controller om der er noget over den
    /// </summary>
    /// <returns>Returner true eller false, kommer an på om der er noget over os</returns>
    private bool CanStand()
    {
        float move = player.radius - 0.1f;
        for (int i = 0; i < 4; i++)
        {
            var posTop = transform.position;
            posTop.y += maxHeight - 0.1f;

            // Tjekker max 4 raycast om der er noget over dem, hvis det er noget over dem kan vi ikke rejse os
            switch (i)
            {
                case 0: posTop.x += move; break;
                case 1: posTop.x -= move; break;
                case 2: posTop.z += move; break;
                case 3: posTop.z -= move; break;
            }
            // Her tjekker vi på om nogen af dem er false
            if (Physics.Raycast(posTop, Vector3.up, maxHeight - player.height))
                return false;
        }
        return true;
    }
    

    #endregion





}