using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControls : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 };

    public RotationAxes axes = RotationAxes.MouseXAndY;

    private Rigidbody rigid;

    private Vector2 xzVelocity;
    private Vector2 wallNormal;

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
    public bool onWall = false;

    public bool isCrouched = false;
    public bool isSprinting = false;
    public bool isSliding = false;
    public bool isMoving = false;

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
        Debug.Log("Rigid Velocity before set to -relative: " + rigid.velocity + 
            "\nXZMagnitude: " + xzVelocity.magnitude);

        // Stops Rigidbody from being zeroed out for On Collision Enter
        // Now able to Slide after landing on the ground
        rigid.velocity = -obj.relativeVelocity;

        Debug.Log("Rigid Velocity after set to -relative: " + rigid.velocity +
            "\nXZMagnitude: " + xzVelocity.magnitude);

        // Goes through all contacts with rigidbody
        foreach (ContactPoint contact in obj.contacts)
        {
            if (contact.normal.y == 1)
            {
                onGround = true;
                canJump = true;

                xzVelocity = new Vector2(rigid.velocity.x, rigid.velocity.z);

                Debug.Log("XZVelocity after set to rigid vel: " + xzVelocity +
                    "\nXAMagnitude: " + xzVelocity.magnitude);

                if (isCrouched && xzVelocity.magnitude <= 10)
                {
                    Debug.Log("Collision: CROUCHED, LANDED, AND MAG <= 10, speed = crouchSpeed");
                    speed = crouchSpeed;
                }
            }

            if (contact.normal.x != 0 || contact.normal.z != 0)
            {
                if (!onGround && contact.normal.y < 0.1)
                {
                    onWall = true;

                    canJump = true;

                    wallNormal = new Vector2(contact.normal.x, contact.normal.z);
                }
            }
        }

        /*
        if (obj.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            canJump = true;

            xzVelocity = new Vector2(rigid.velocity.x, rigid.velocity.z);
            
            if (isCrouched && xzVelocity.magnitude <= 10)
            {
                Debug.Log("CROUCHED, LANDED, AND MAG <= 10, speed = crouchSpeed");
                speed = crouchSpeed;
            }
        }
        */
    }

    void OnCollisionStay(Collision obj)
    {
        // Goes through all contacts with rigidbody
        foreach (ContactPoint contact in obj.contacts)
        {
            if (contact.normal.y == 1)
            {
                onGround = true;
                canJump = true;
            }

            if (contact.normal.x != 0 || contact.normal.z != 0)
            {
                if (!onGround && contact.normal.y < 0.1)
                {
                    onWall = true;

                    canJump = true;

                    wallNormal = new Vector2(contact.normal.x, contact.normal.z);
                }
            }
        }
    }

    void OnCollisionExit(Collision obj)
    {
        //Debug.Log("CollisionExit Detected: Contacts are - " + obj.contacts);
        if (onGround)
        {
            onGround = false;
        }

        if (onWall)
        {
            onWall = false;
        }
    }

        private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Point hit - " + hit.point + "\nHit Normal - " + hit.normal);
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
        if (!onWall)
        {
            if (onGround)
            {
                rigid.velocity = new Vector3(rigid.velocity.x, CalculateJumpVerticalSpeed(), rigid.velocity.z);
            }
            else if (!onGround && canJump)
            {
                Debug.Log("Air Jump:");
                canJump = false;
                rigid.velocity = new Vector3(rigid.velocity.x, CalculateJumpVerticalSpeed(), rigid.velocity.z);
            }
        }
        else
        {
            Debug.Log("Wall Jump:");

            rigid.velocity = new Vector3(rigid.velocity.x + wallNormal.x * speed, CalculateJumpVerticalSpeed(), rigid.velocity.z + wallNormal.y * speed);
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
        isSliding = true;
        Vector2 slideVelocity = xzVelocity * slideMultiplier;

        Debug.Log("Slide Force Not Added, RigidVel: " + rigid.velocity + "\tRigidMag: " + rigid.velocity.magnitude);

        rigid.AddForce(new Vector3(slideVelocity.x, 0, slideVelocity.y));

        Debug.Log("Slide Force Added, RigidVel: " + rigid.velocity + "\tRigidMag: " + rigid.velocity.magnitude +
            "\n XZVel: " + xzVelocity + "\tXZMag: " + xzVelocity.magnitude);
    }

    void Move()
    {

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

        rigid.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void AirMove()
    {
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

        rigid.AddForce(velocityChange * 10);
    }
    

    void Update()
    {
        //Debug.Log("CharController isGrounded = " + controller.isGrounded);

        xzVelocity = new Vector2(rigid.velocity.x, rigid.velocity.z);

        /*test.text = "Target Velocity: " + targetVelocity +
            "\nVelocity Change: " + velocityChange +
            "\nCrrnt Velocity: " + rigid.velocity +
            "\nMagnitude: " + rigid.velocity.magnitude +
            "\nXZMagnitude: " + xzVelocity.magnitude +
            "\nHorizontal: " + Input.GetAxis("Horizontal") + 
            "\nVertical: " + Input.GetAxis("Vertical");
            */
        // Turning the Player left or right
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

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (onWall)
        {
            gravity = 1.5f;

            // PUsh Player towards wall they are colliding with for wall run
            rigid.AddForce(-wallNormal.x * 5, 0, -wallNormal.y * 5);
        }
        else
        {
            gravity = 10.0f;
        }


        if (onGround)
        {
            if (!isSliding)
            {
                Move();
            }
        }
        else if(!onGround && isMoving)
        {
            AirMove();
        }

        // Jump
        if (canJump && Input.GetButtonDown("Jump"))
        {
            isCrouched = false;
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
            else if (isCrouched && !onGround)
            {
                isCrouched = false;
            }
            else
            {
                Debug.Log("SLIDE BUTTON PRESSED ELSE, speed = crouchSpeed");
                if (isSprinting && onGround)
                {
                    isSprinting = false;
                }
                isCrouched = true;
                speed = crouchSpeed;
            }
        }

        if (isCrouched && onGround)
        {
            Debug.Log("IsCrouched: " + isCrouched + "OnGround: " + onGround + "IsSliding: " + isSliding +
                "\nXZVel: " + xzVelocity + "\tXZMag: " + xzVelocity.magnitude);
        }
        // Slide
        if (onGround && (xzVelocity.magnitude > 10 && isCrouched) && !isSliding)
        {
            if (isSprinting)
            {
                isSprinting = false;
            }

            Debug.Log("Calling Slide()");
            Slide();
        }

        if (isSliding)
        {
            if (rigid.velocity.magnitude < 5)
            {
                isSliding = false;
                speed = crouchSpeed;
            }
        }
        
        if (!onGround)
        {
            isSliding = false;
        }

        // We apply gravity manually for more tuning control
        rigid.AddForce(new Vector3(0, -gravity * rigid.mass, 0));
    }


}
