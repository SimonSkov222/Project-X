using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{

    [SerializeField]
    protected StandartCharacter character;
    
    


    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    public float sensitivity = 2.0f;
    public float gravity = 20.0f;
    public float maxHeight = 1.6f;
    public float minHeight = 1.3f;
    public float heightSmooth = 5f;
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
    private float rotX;
    private float rotY;
    private bool crouch;


    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////


    /// <summary>
    /// Vi gemmer nogle components og kameraet på gameobjectet.
    /// </summary>
    void Start()
    {
        player = GetComponent<CharacterController>();
        
        //

        var pm = Instantiate(character.PlayerModel, Vector3.zero, transform.rotation, transform);
        pm.transform.position = Vector3.zero;
        //character.PlayerModel.transform.parent = transform;
    }

    private float timeWeapon = 0;
    private float[] timeAbilities = { 0,0,0};

    /// <summary>
    /// Her er hvordan kontrol over gameobjectet bliver udført.
    /// </summary>
    void Update()
    {
        RotateView();
        CalculateMovement();

        ButtonHelper.InvokeButtonEvents(this,"Jump", "OnJump");

        //if (Time.time - timeWeapon >= character.Weapon.FireRate)
        //{
        //    if (ButtonHelper.InvokeButtonEvents(character.Weapon, "Fire1", "OnButton"))
        //    {
        //        timeWeapon = Time.time;
        //    }
        //}

        
        ButtonHelper.InvokeButtonEvents(this, "Reload", "OnReload");

       

        ButtonHelper.InvokeButtonEvents(this, "Ultimate", "OnUltimate");

        AbilityUpdate(character.Ability1, "Ability1");
        AbilityUpdate(character.Ability2, "Ability2");
        AbilityUpdate(character.Ability3, "Ability3");

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
            movement.y = character.JumpHeight;
        }
    }
    

    private void ApplyGravity()
    {
        if (!player.isGrounded)
        {
            movement.y -= gravity * Time.deltaTime;
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
        moveFB = Input.GetAxis("Vertical") * character.Speed * crouchR;
        moveLR = Input.GetAxis("Horizontal") * character.Speed * crouchR;

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
        character.Eyes.transform.localRotation = Quaternion.AngleAxis(-mouseLock.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(mouseLock.x, player.transform.up);

    }
    
    
}
