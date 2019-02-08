using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControls : MonoBehaviour
{
    private Rigidbody rigid;

    public Text test;

    public Vector3 velocity;
    private Vector3 velocityChange;
    private Vector3 targetVelocity;

    public float moveSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float speed = 10.0f;

    public float slideMultiplier;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;

    public bool canJump = true;
    public bool onGround = false;
    public bool isCrouched = false;
    public bool isSprinting = false;
    public bool isSliding = false;

    public float jumpHeight = 2.0f;
    

    public float sensitivityX;
    public float joySensitivityX;

    void Awake()
    {
        speed = moveSpeed;

        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
        rigid.useGravity = false;
    }

    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            canJump = true;
        }
    }

    void OnCollisionStay(Collision obj)
    {
        if (obj.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            canJump = true;
        }
    }

    void OnCollisionExit(Collision obj)
    {
        if (obj.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        if (onGround)
        {
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }
        else if (isCrouched || isSliding)
        {
            return Mathf.Sqrt(2 * (jumpHeight - 1) * gravity);
        }
        else
        {
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }
    }

    void Jump()
    {
        if (onGround)
        {
            rigid.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
        }
        else if (!onGround && canJump)
        {
            Debug.Log("Air Jump:");
            canJump = false;
            rigid.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
        }

        //rigid.AddForce(0, CalculateJumpVerticalSpeed(), 0);
    }

    void Sprint()
    {
        // Cancels Slide
        isCrouched = false;
        isSliding = false;

        speed = sprintSpeed;
        isSprinting = true;
    }

    void Slide()
    {
        Debug.Log("Entered Slide Function");
        
        Debug.Log("Sliding on the Ground");
        isSliding = true;
        rigid.AddForce(rigid.velocity * slideMultiplier);
    }

    void FixedUpdate()
    {
        
    }

    void Update()
    {
        Vector2 xzVelocity = new Vector2(rigid.velocity.x, rigid.velocity.z);

        test.text = "Target Velocity: " + targetVelocity +
            "\nVelocity Change: " + velocityChange +
            "\nCrrnt Velocity: " + rigid.velocity +
            "\nMagnitude: " + rigid.velocity.magnitude +
            "\nXZMagnitude: " + xzVelocity.magnitude;

        // Turning the Camera and player left or right
        if (axes == RotationAxes.MouseXAndY)
        {
            //Debug.Log (Input.GetJoystickNames ().Length);

            // If a controller is plugged in
            if (Input.GetJoystickNames().Length != 0)
            {
                rigid.MoveRotation(rigid.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("JoyX") * joySensitivityX, 0)));
            }
            else
            {
                //rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                rigid.MoveRotation(rigid.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * sensitivityX, 0)));
            }
        }


        // Calculate how fast we should be moving
        targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        velocity = rigid.velocity;
        velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        if (!isSliding)
        {
            rigid.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        // Jump
        if (canJump && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Sprint
        if (Input.GetButtonDown("Sprint"))
        {
            Sprint();
        }
        else if (Input.GetAxis("Vertical") <= 0)
        {
            isSprinting = false;

            if (!isCrouched)
            {
                speed = moveSpeed;
            }
        }

        // Crouch
        if (Input.GetButtonDown("Slide"))
        {
            if (isCrouched)
            {
                isCrouched = false;
                speed = moveSpeed;
            }
            else if (!isCrouched && !onGround)
            {
                isCrouched = true;
            }
            else
            {
                isCrouched = true;
                speed = crouchSpeed;
            }
        }

        // Slide
        if (onGround && xzVelocity.magnitude > 10 && isCrouched)
        {
            isCrouched = true;
            speed = crouchSpeed;
            Slide();
        }

        if (isSliding)
        {
            if (rigid.velocity.magnitude < 5)
            {
                isSliding = false;
            }
        }

        // We apply gravity manually for more tuning control
        rigid.AddForce(new Vector3(0, -gravity * rigid.mass, 0));
    }


}
