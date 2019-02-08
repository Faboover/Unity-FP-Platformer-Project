using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 };

    public RotationAxes axes = RotationAxes.MouseXAndY;

    public GameObject playerCam;

    public Rigidbody rigid;

    public Text test;

    public float sensitivityX = 5F;
    public float joySensitivityX = 10F;

    // Moves body left, right, forward and back
    float x_velocity;
    float z_velocity;

    // Set the min and max of the rotation
    public float minimumX = -360F;
    public float maximumX = 360F;

    // movement speed of the player
    public float moveSpeed = 0.1f;
    public float sprintSpeed = 0.3f;
    public float speed;
    public float maxVelocityChange = 10.0f;
    public float gravity = 9.8f;

    // Used for vertical Rotation, turning left or right
    float rotationY = 0.0f;

    // Used for knowing when the player is back on a flat or tagged ground object
    public bool onGround = false;

    public bool onWall = false;

    public bool airJumped = false;

    // Used to know when the player is sprinting
    public bool isSprinting = false;
    public bool isMoving = false;

    // Used for knowing whether the player is sliding or ready to slide
    // isCrouched is for when the player hits the slide while not sprinting
    public bool isSliding = false;
    public bool isCrouched = false;

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

        rigid = this.GetComponent<Rigidbody>();
        rigid.useGravity = false;

        // Event Handler not yet made, starting with basic moving for both controller
        // and mouse and keyboard
        //eventHandler = GameObject.FindGameObjectWithTag("Handler");
    }

    // All collision instances
    void OnCollisionEnter(Collision obj)
    {
        /*
        if (CollisionFlags.CollidedBelow != 0)
        {
            onGround = true;
        }
        */

        if (obj.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            
            if (isCrouched)
            {
                Slide(x_velocity, z_velocity);
            }
        }
        else if (obj.gameObject.CompareTag("Wall"))
        {
            onWall = true;
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

    // May need to use
    void SetJumpForces()
    {
        // Regular Jump
        jumpForce = new Vector3(rigid.velocity.x, jumpForce.y, rigid.velocity.z);

        // Lighter Jump
        slidejumpForce = new Vector3(rigid.velocity.x, slidejumpForce.y, rigid.velocity.z);
    }

    // Jump function - will handle jump force for what the current player status is
    // Will depend on whether the chararcter is in the air, on the ground, sliding, or 
    // wall running
    void Jump()
    {
        // Cancels slide
        isCrouched = false;
        isSliding = false;

        Debug.Log("Jump Function entered");
        if (onGround && !isSliding)
        {
            //Debug.Log("Normal Jump: OnGround !isSliding");
            rigid.AddForce(jumpForce);
        }
        else if (onGround && isSliding)
        {
            //Debug.Log("Slide Jump: OnGround isSliding");
            rigid.AddForce(slidejumpForce);
        }
        else if (!onGround && !airJumped)
        {
            //Debug.Log("Air Jump:");
            airJumped = true;
            rigid.AddForce(jumpForce);
        }
    }

    void Sprint()
    {
        // Cancels Slide
        isCrouched = false;
        isSliding = false;

        speed = sprintSpeed;
        isSprinting = true;
    }

    void Slide(float xVel, float zVel)
    {
        Debug.Log("Entered Slide Function");

        if ((onGround && isSprinting)) //|| (onGround && isCrouched))
        {
            isSliding = true;

            Debug.Log("Ground Sliding!!");
            //this.GetComponent<Rigidbody>().AddForce(new Vector3 (xVel * slideMultiplier, 0, zVel * slideMultiplier));

            rigid.AddForce(rigid.velocity * slideMultiplier);
        }

        // For sliding after jumping, need to know the player velocity and whether it is as fast as sprinting.
    }

    // Update is called once per frame
    void Update ()
    {
        // Moves body left, right, forward and back
        x_velocity = Input.GetAxisRaw("Horizontal") * speed;
        z_velocity = Input.GetAxisRaw("Vertical") * speed;

        //rigid.MoveRotation(rigid.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * sensitivityX, 0)));

        // Calculate how fast we should be moving
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rigid.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        //velocityChange.y =  ;


        rigid.AddForce(velocityChange, ForceMode.VelocityChange);

        //rigid.velocity = velocityChange;

        // We apply gravity manually for more tuning control
        GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));

        test.text = "Target Velocity: " + targetVelocity +
            "\nVelocity Change: " + velocityChange +
            "\nCrrnt Velocity: " + rigid.velocity;

        // Turning the Camera and player left or right
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
                //rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                rigid.MoveRotation(rigid.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * sensitivityX, 0)));
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            //Debug.Log("Jump Button Pressed!!");
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
            if (isCrouched)
            {
                isCrouched = false;
            }
            else
            {
                isCrouched = true;
                Slide(x_velocity, z_velocity);
            }
        }
    }
}
