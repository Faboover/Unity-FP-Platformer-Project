using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 };

    public RotationAxes axes = RotationAxes.MouseXAndY;

    public GameObject playerCam;

    public float sensitivityX = 5F;
    public float joySensitivityX = 10F;

    // Set the min and max of the rotation
    public float minimumX = -360F;
    public float maximumX = 360F;

    // movement speed of the player
    public float moveSpeed = 0.1f;
    public float sprintSpeed = 0.3f;
    public float speed;

    // Used for vertical Rotation, turning left or right
    float rotationY = 0.0f;

    // Used for knowing when the player is back on a flat or tagged ground object
    public bool onGround = false;

    public bool airJumped = false;

    // Used to know when the player is sprinting
    public bool isSprinting = false;

    // Used for knowing whether the player is sliding or ready to slide
    // Ready to slide is for when the player hits the slide button while in the air
    public bool isSliding = false;
    public bool readytoSlide = false;

    public float slideMultiplier;

    // Vertical upward force to throw the player up when jumping
    // Set in the Unity Inspecter
    public Vector3 jumpForce;

    // In Titanfall 2, jumping while sliding doesn't go as high as jumping when not sliding.
    // Could use for double jump
    // Set in the Unity Inspecter
    public Vector3 slidejumpForce;

    public Vector3 slideForce;

    // This is used to know whether or not the game has been paused
    public GameObject eventHandler;

    // Use this for initialization
    void Start ()
    {
        speed = moveSpeed;

        playerCam = GameObject.FindGameObjectWithTag("MainCamera");

        // Event Handler not yet made, starting with basic moving for both controller
        // and mouse and keyboard
        //eventHandler = GameObject.FindGameObjectWithTag("Handler");
    }

    // All collision instances
    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            
            if (readytoSlide)
            {
                isSliding = true;
            }
        }
    }

    void OnCollisionStay(Collision obj)
    {
        if (obj.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            airJumped = false;
        }
    }

    void OnCollisionExit(Collision obj)
    {
        if (obj.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    // Jump function - will handle jump force for what the current player status is
    // Will depend on whether the chararcter is in the air, on the ground, sliding, or 
    // wall running
    void Jump()
    {
        Debug.Log("Jump Function entered");
        if (onGround && !isSliding)
        {
            Debug.Log("Normal Jump: OnGround !isSliding");
            this.GetComponent<Rigidbody>().AddForce(jumpForce);
        }
        else if (onGround && isSliding)
        {
            Debug.Log("Slide Jump: OnGround isSliding");
            this.GetComponent<Rigidbody>().AddForce(slidejumpForce);
        }
        else if (!onGround && !airJumped)
        {
            Debug.Log("Air Jump:");
            airJumped = true;
            this.GetComponent<Rigidbody>().AddForce(jumpForce);
        }
    }

    void Sprint()
    {
        speed = sprintSpeed;
        isSprinting = true;
    }

    void Slide(float xVel, float zVel)
    {
        if (onGround && isSprinting)
        {
            isSliding = true;

            this.GetComponent<Rigidbody>().AddForce(new Vector3 (xVel * slideMultiplier, 0, zVel * slideMultiplier));
        }
    }

    // Update is called once per frame
    void Update ()
    {
        // Moves body left, right, forward and back
        float x_velocity = Input.GetAxisRaw("Horizontal") * speed;
        float z_velocity = Input.GetAxisRaw("Vertical") * speed;

        // Its best to look up for yourself what is going on from this point on
        // This was code that I got by looking for a way to do a first person camera
        transform.Translate(new Vector3(x_velocity, 0, z_velocity));

        if (axes == RotationAxes.MouseXAndY)
        {
            //Debug.Log (Input.GetJoystickNames ().Length);

            // If a controller is plugged in
            if (Input.GetJoystickNames().Length != 0)
            {
                rotationY = transform.localEulerAngles.y + Input.GetAxis("JoyX") * joySensitivityX;
            }
            else
            {
                rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            }

            transform.localEulerAngles = new Vector3(0, rotationY, 0);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump Button Pressed!!");
            Jump();
        }

        if (Input.GetButtonDown("Sprint"))
        {
            Sprint();
        }
        else if (Input.GetAxis("Vertical") <= 0)
        {
            isSprinting = false;
            speed = moveSpeed;
        }

        if (Input.GetButtonDown("Slide"))
        {
            if (readytoSlide)
            {
                readytoSlide = false;
            }
            else
            {
                readytoSlide = true;
                Slide(x_velocity, z_velocity);
            }
        }
    }
}
