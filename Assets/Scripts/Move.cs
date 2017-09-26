using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {


    CharacterController player;

    private Camera eyes;
    private Vector3 movement = Vector3.zero;
    
    public float speed = 2.0f;
    public float sensitivity = 2.0f;
    public float gravity = 20.0f;
    public float jumpSpeed = 8.0f;
    public float maxHeight = 1.6f;
    public float minHeight = 1.3f;
    public float crouchTime = 5f;
    public float smoothing = 2.0f;

    private Vector2 mouseLock;
    private Vector2 smoothV;

    private float crouchR = 1f;
    private float moveFB;
    private float moveLR;
    private float rotX;
    private float rotY;

    private bool crouch;
    
    // Use this for initialization
    void Start ()
    {
        player = GetComponent<CharacterController>();
        eyes = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        RotateView();
        CalculateMovement();

        if (player.isGrounded && Input.GetButton("Jump"))
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
        

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        
        //Debug.DrawLine(eyes.transform.position, eyes.transform.forward * 20f, Color.red);

        movement.y -= gravity * Time.deltaTime;
        player.Move(movement * Time.deltaTime);
    }
    
    private void CalculateMovement()
    {
        float oldY = movement.y;
        moveFB = Input.GetAxis("Vertical") * speed * crouchR;
        moveLR = Input.GetAxis("Horizontal") * speed * crouchR;
        
        movement = new Vector3(moveLR, 0, moveFB);
        movement = transform.rotation * movement;
        movement.y = oldY;
    }

    private void RotateView()
    {
        var md = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);

        mouseLock += smoothV;
        mouseLock.y = Mathf.Clamp(mouseLock.y, -75f, 75f);

        eyes.transform.localRotation = Quaternion.AngleAxis(-mouseLock.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(mouseLock.x, player.transform.up);
        
    }

    private void StartCrouching()
    {
        var value = crouchTime * Time.deltaTime;
        var center = player.center;
        crouchR = 0.6f;

        if (player.height > minHeight)
        {
            player.height -= value;
            center.y += value / 2;
        }

        if (player.height < minHeight)
        {
            center.y -= (minHeight - player.height) / 2;
            player.height = minHeight;
        }
        player.center = center;
    }

    private void StopCrouching()
    {
        var value = crouchTime * Time.deltaTime;
        var center = player.center;
        crouchR = 1f;
        if (player.height < maxHeight)
        {
            
            player.height += value;
            center.y -= value / 2;
        }

        if (player.height > maxHeight)
        {
            center.y += (player.height - maxHeight) / 2;
            player.height = maxHeight;
        }

        player.center = center;
    }

    private bool CanStand()
    {
        float move = player.radius - 0.1f;
        for (int i = 0; i < 4; i++)
        {
            var posTop = transform.position;
            posTop.y += maxHeight - 0.1f;

            switch (i)
            {
                case 0:     posTop.x += move;       break;
                case 1:     posTop.x -= move;       break;
                case 2:     posTop.z += move;       break;
                case 3:     posTop.z -= move;       break;
            }
            if (Physics.Raycast(posTop, Vector3.up, maxHeight - player.height))
                return false;
        }
        return true;
    }
    

    
    }


    //void EyesPositionChanging()
    //{
    //    Vector3 posA = new Vector3(transform.position.x, transform.position.y + crouchD, transform.position.z);
    //    Vector3 posB = new Vector3(transform.position.x, transform.position.y + crouchU, transform.position.z);

    //    var test = transform.position;
    //    test.y += player.height - 1f;

    //    float yPos = transform.position.y + player.height - 1f;

    //    if (crouch)
    //        eyesNewPos = posA;
    //    if (!crouch)
    //        eyesNewPos = posB;

    //    eyes.transform.position = Vector3.Lerp(eyes.transform.position, eyesNewPos, Time.deltaTime * crouchR);
        
    //}
    
