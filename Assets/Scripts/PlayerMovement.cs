using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Denne klasse gør at vi kan bevæge os rundt med tastatur
//  og kigge rundt med mus, den sørger også for at vi ikke
//  kan dreje hovedet hele vejen rundt på x og z aksen.
//  Vi kan også dykke os.
//  Man kan fjerne musen på ’i’ og få den frem igen på ’i’.
//
//////////////////////////////////////////////////////

public class PlayerMovement : MonoBehaviour {


    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////

    public float speed = 2.0f;
    public float sensitivity = 2.0f;
    public float gravity = 20.0f;
    public float jumpSpeed = 8.0f;
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
    private Camera eyes;
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
        eyes = Camera.main;

        IsMouseLocked(true);
    }

    /// <summary>
    /// Her er hvordan kontrol over gameobjectet bliver udført.
    /// </summary>
    void Update()
    {

        RotateView();
        CalculateMovement();

        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            movement.y = jumpSpeed;
        }

        if (Input.GetButton("Crouch") || !CanStand())
        {
            StartCrouching();
        }
        else
        {
            StopCrouching();
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            IsMouseLocked(Cursor.visible);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            Time.timeScale = 0.1f;
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            Time.timeScale = 1;
        }

        //Debug.DrawLine(eyes.transform.position, eyes.transform.forward * 20f, Color.red);
        //Debug.Log("2"+movement.y);
        if (!player.isGrounded)
        {
            movement.y -= gravity * Time.deltaTime;
        }
        player.Move(movement * Time.deltaTime);
    }



    ///////////////////////////////
    //      Private Method
    ///////////////////////////////

    /// <summary>
    /// Får character til at bevæge sig den retning man vil
    /// </summary>
    private void CalculateMovement()
    {
        moveFB = Input.GetAxis("Vertical") * speed * crouchR;
        moveLR = Input.GetAxis("Horizontal") * speed * crouchR;

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
        eyes.transform.localRotation = Quaternion.AngleAxis(-mouseLock.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(mouseLock.x, player.transform.up);

    }

    /// <summary>
    /// Gør at vi kan dukke os når vi trykker på ctrl
    /// </summary>
    private void StartCrouching()
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
    }

    /// <summary>
    /// Gør at vi stopper med at dukke, når vi giver slip på ’ctrl’
    /// </summary>
    private void StopCrouching()
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
