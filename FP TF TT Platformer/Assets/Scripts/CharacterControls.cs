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

    private Vector3 prevWallDir = new Vector3(0, 0, 0);

    public Text test;

    public Vector3 velocity;
    private Vector3 velocityChange;
    private Vector3 targetVelocity;
    private Vector3 wallDir;

    public float moveSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float wallSpeed;
    public float speed = 10.0f;

    public float slideMultiplier;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;

    public float smoothing;

    public bool canJump = true;

    public bool onGround = false;
    public bool onWall = false;

    public bool isCrouched = false;
    public bool isSprinting = false;
    public bool isSliding = false;
    public bool isMoving = false;
    public bool isTurning = false;

    public bool newWall = false;

    public float jumpHeight = 2.0f;
    

    public float sensitivityX;
    public float joySensitivityX;

    void Awake()
    {
        AdjustSpeed(moveSpeed);

        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
        rigid.useGravity = false;
    }

    void OnCollisionEnter(Collision obj)
    {
        //Debug.Log("Rigid Velocity before set to -relative: " + rigid.velocity + 
            //"\nXZMagnitude: " + xzVelocity.magnitude);

        // Stops Rigidbody from being zeroed out for On Collision Enter
        // Now able to Slide after landing on the ground
        rigid.velocity = -obj.relativeVelocity;

        //Debug.Log("Rigid Velocity after set to -relative: " + rigid.velocity +
            //"\nXZMagnitude: " + xzVelocity.magnitude);

        // Goes through all contacts with rigidbody
        foreach (ContactPoint contact in obj.contacts)
        {
            if (contact.normal.y > 0.85)
            {
                onGround = true;
                canJump = true;

                xzVelocity = new Vector2(rigid.velocity.x, rigid.velocity.z);

               // Debug.Log("XZVelocity after set to rigid vel: " + xzVelocity +
                    //"\nXAMagnitude: " + xzVelocity.magnitude);

                if (isCrouched && xzVelocity.magnitude <= 10)
                {
                    //Debug.Log("Collision: CROUCHED, LANDED, AND MAG <= 10, speed = crouchSpeed");
                    AdjustSpeed(crouchSpeed);
                }
            }

            if (contact.normal.x != 0 || contact.normal.z != 0)
            {
                if (!onGround && contact.normal.y < 0.1)
                {
                    onWall = true;

                    canJump = true;

                    wallNormal = new Vector2(contact.normal.x, contact.normal.z);

                    wallDir = Vector3.Cross(contact.normal, Vector3.up);
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
            if (contact.normal.y > 0.85)
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

                    wallDir = Vector3.Cross(contact.normal, Vector3.up);
                }
            }
            
            if (onGround)
            {
                onWall = false;
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

        /*
        if (onWall)
        {
            onWall = false;
        }
        */
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

    private void Check4Wall()
    {
        RaycastHit hit;

        float distance = 1f;

        if (Physics.Raycast(this.transform.position, -this.transform.right, out hit, distance))
        {
            //Debug.Log("Raycast hit something on the left, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, this.transform.right, out hit, distance))
        {
            //Debug.Log("Raycast hit something on the right, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, this.transform.right + this.transform.forward, out hit, distance))
        {
            //Debug.Log("Raycast hit something 45 deg on the right, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, -this.transform.right + this.transform.forward, out hit, distance))
        {
            //Debug.Log("Raycast hit something 45 deg on the left, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, this.transform.right + -this.transform.forward, out hit, distance))
        {
            //Debug.Log("Raycast hit something 135 deg on the right, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, -this.transform.right + -this.transform.forward, out hit, distance))
        {
            //Debug.Log("Raycast hit something 135 deg on the left, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, (Mathf.Cos(60f) * this.transform.right) + (Mathf.Sin(60f) * this.transform.forward), out hit, distance))
        {
            //Debug.Log("Raycast hit something 60 deg on the right, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, (Mathf.Cos(60f) * -this.transform.right) + (Mathf.Sin(60f) * this.transform.forward), out hit, distance))
        {
            //Debug.Log("Raycast hit something 60 deg on the left, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, (Mathf.Cos(60f) * this.transform.right) + (Mathf.Sin(60f) * -this.transform.forward), out hit, distance))
        {
            //Debug.Log("Raycast hit something 120 deg on the right, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else if (Physics.Raycast(this.transform.position, (Mathf.Cos(60f) * -this.transform.right) + (Mathf.Sin(60f) * -this.transform.forward), out hit, distance))
        {
            //Debug.Log("Raycast hit something 120 deg on the left, " + hit.normal);

            if (hit.normal.y <= 0.1)
            {
                if (!onGround)
                {
                    onWall = true;
                }
                wallDir = Vector3.Cross(hit.normal, Vector3.up);

                if (wallDir != prevWallDir)
                {
                    newWall = true;

                    prevWallDir = wallDir;
                }
            }
        }
        else
        {
            onWall = false;
        }
    }

    void Jump()
    {
        if (!onWall)
        {
            if (onGround)
            {
                //Debug.Log("On Ground Jump");
                rigid.velocity = new Vector3(rigid.velocity.x, CalculateJumpVerticalSpeed(), rigid.velocity.z);
            }
            else if (!onGround && canJump)
            {
                //Debug.Log("In Air Jump:");
                canJump = false;
                rigid.velocity = new Vector3(rigid.velocity.x, CalculateJumpVerticalSpeed(), rigid.velocity.z);
            }
        }
        else
        {
            //Debug.Log("On Wall Jump, Wall Normal = " + wallNormal + "Current Velocity: " + rigid.velocity + "\nCurrent Speed = " + speed);
            
            rigid.velocity = new Vector3(rigid.velocity.x + (wallNormal.x * 10), CalculateJumpVerticalSpeed(), rigid.velocity.z + (wallNormal.y * 10));

            onWall = false;
            /*
            if ((wallNormal.x < 0 && rigid.velocity.x > 0) || (wallNormal.x > 0 && rigid.velocity.x < 0))
            {
                rigid.velocity = new Vector3 (-rigid.velocity.x, rigid.velocity.y, rigid.velocity.z);
            }

            if ((wallNormal.y < 0 && rigid.velocity.z > 0) || (wallNormal.y > 0 && rigid.velocity.z < 0))
            {
                wallNormal.y *= -1;
            }
            */

            Debug.Log("Velocity After Wall Jump: " + rigid.velocity);
        }

        //rigid.AddForce(0, CalculateJumpVerticalSpeed(), 0);
    }

    void Sprint()
    {
        // Cancels Slide
        isCrouched = false;
        isSliding = false;

        AdjustSpeed(sprintSpeed);
        isSprinting = true;
    }

    void Slide()
    {
        isSliding = true;
        Vector2 slideVelocity = xzVelocity * slideMultiplier;

        //Debug.Log("Slide Force Not Added, RigidVel: " + rigid.velocity + "\tRigidMag: " + rigid.velocity.magnitude);

        rigid.AddForce(new Vector3(slideVelocity.x, 0, slideVelocity.y));

        //Debug.Log("Slide Force Added, RigidVel: " + rigid.velocity + "\tRigidMag: " + rigid.velocity.magnitude +
            //"\n XZVel: " + xzVelocity + "\tXZMag: " + xzVelocity.magnitude);
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
    
    void WallMove()
    {
        // Direction player is trying to move towards, to be used for learning the angle between this direction and the wall
        Vector3 targetAngle = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));

        bool rayBack = Physics.Raycast(this.transform.position, -this.transform.forward, 0.6f);
        bool rayForward = Physics.Raycast(this.transform.position, this.transform.forward, 0.6f);

        // The angle between the vector the player is facing and the angle of the wall
        float angleFacing = Vector3.Angle(wallDir, transform.forward);
        float angleDif = Vector3.Angle(wallDir, targetAngle);
        
        // Need to check where the player is facing in regards to the wall
        // The wall value we are using is a vector (a direction), thus when the player moves in relation to the wall
        // we will need to adjust the wall vector to follow the players input.
        if (angleFacing >= 90 && angleFacing <= 180)
        {
            Debug.Log("Flipping Wall Direction from " + wallDir + " to " + -wallDir);
            wallDir *= -1;
        }
        
        // To allow movement away from the wall in the given range
        if (angleDif > 60 && angleDif < 120 && isMoving)
        {
            AirMove();

            return;
        }

        // Calculate how fast we should be moving in along the wall

        //Debug.Log("Angle Facing = " + angleFacing);
        // How should we use the players input based on the angle of the character from the wall.
        if (angleFacing <= 45 || angleFacing >= 135)
        {
            //Debug.Log("Using Vertical Input for Target Velocity");
            targetVelocity = Vector3.Scale(wallDir, new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Vertical")));
        }
        else if (rayBack)
        {
            //Debug.Log("RayBack is true");
            //Debug.Log("WallDir: " + wallDir + "\tTransform Right: " + this.transform.right);
            //Debug.Log("Angel Between: " + Vector3.Angle(wallDir, this.transform.right));
            if (Vector3.Angle(wallDir, this.transform.right) > 90)
            {
                wallDir *= -1;
            }

            targetVelocity = Vector3.Scale(wallDir, new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Horizontal")));
        }
        else if (rayForward)
        {
            //Debug.Log("RayForward is true");
            //Debug.Log("WallDir: " + wallDir + "\tTransform Right: " + this.transform.right);
            //Debug.Log("Angel Between: " + Vector3.Angle(wallDir, this.transform.right));
            if (Vector3.Angle(wallDir, this.transform.right) > 90)
            {
                wallDir *= -1;
            }

            targetVelocity = Vector3.Scale(wallDir, new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Horizontal")));
        }
        else
        {
            targetVelocity = Vector3.Scale(wallDir, new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        }

        //targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        //Debug.Log("TargetVel = " + targetVelocity);

        // Apply a force that attempts to reach our target velocity
        velocity = rigid.velocity;
        velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        
        if (newWall && !Input.GetButtonDown("Jump"))
        { 
            rigid.AddForce(velocityChange, ForceMode.VelocityChange);
            newWall = false;
        }
        else
        {
            rigid.AddForce(velocityChange * speed);
        }

        if (!isTurning && isMoving)
        {
            AdjustRotation();
        }
    }

    private void AdjustSpeed(float s)
    {
        if (onWall)
        {
            speed = Mathf.Lerp(speed, s, smoothing * Time.deltaTime);
        }
        else
        {
            speed = s;
        }
    }

    private void AdjustRotation()
    {
        // The angle between the vector the player is facing and the angle of the wall
        float angleFacing = Vector3.Angle(wallDir, transform.forward);

        float h = Mathf.Sqrt(Mathf.Pow(wallDir.x, 2) + Mathf.Pow(wallDir.z, 2));

        float angleofWall;
        if (wallDir.x > -0.001 && wallDir.x < 0.001)
        {
            angleofWall = Mathf.Rad2Deg * Mathf.Acos(wallDir.z / h);
        }
        else if (wallDir.z > -0.001 && wallDir.z < 0.001)
        {
            angleofWall = Mathf.Rad2Deg * Mathf.Asin(wallDir.x / h);
        }
        else if (wallDir.x > 0)
        {
            if (wallDir.x < 0 || wallDir.z < 0)
            {
                h *= -1;

                angleofWall = Mathf.Rad2Deg * Mathf.Asin(wallDir.x / h);

                angleofWall += 180;
            }
            else
            {
                angleofWall = Mathf.Rad2Deg * Mathf.Asin(wallDir.x / h);
            }
        }
        else
        {
            if (wallDir.x < 0 || wallDir.z < 0)
            {
                h *= -1;
                angleofWall = Mathf.Rad2Deg * Mathf.Acos(wallDir.z / h);

                angleofWall += 180;
            }
            else
            {
                angleofWall = Mathf.Rad2Deg * Mathf.Acos(wallDir.z / h);
            }
        }

        float turningRate = 150f;

        /*
        if (wallDir.z > -1.01 && wallDir.z < -0.99 && wallDir.x == 0)
        {
            angleofWall = 180;
        }

        
        if (flip)
        {
            if (angleofWall != 0 && angleofWall != 180)
            {
                angleofWall += 180;
            }
        }
        */
        Debug.Log("Angleof Wall: " + angleofWall + " Wallx: " + wallDir.x + " Wallz: " + wallDir.z + " H: " + h);

        Quaternion quatRot = Quaternion.Euler(0, angleofWall, 0);

        if (angleFacing <= 45)
        {
            //transform.rotation = Quaternion.LookRotation(wallDir);

            // Turn towards our target rotation.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, quatRot, turningRate * Time.deltaTime);
        }
    }

    void Update()
    {
        // Helps keep track of current player xz velocity and magnitude
        xzVelocity = new Vector2(rigid.velocity.x, rigid.velocity.z);

        // Text to display that helps debug
        test.text = "OnWall: " + onWall +
            "\nWall Direction: " + wallDir +
            "\nPrev Wall Dir: " + prevWallDir + 
            "\nNew Wall: " + newWall +
            "\nCrrnt Velocity: " + rigid.velocity +
            "\nMagnitude: " + rigid.velocity.magnitude +
            "\nXZMagnitude: " + xzVelocity.magnitude;
            
        // Turning the Player left or right
        if (axes == RotationAxes.MouseXAndY)
        {
            //Debug.Log (Input.GetJoystickNames ().Length);

            // Value to be used when Wallrunning, will see if rotation is being controlled by the player so that player can be rotated towards a wall
            if (Input.GetAxis("JoyX") != 0 || Input.GetAxis("Mouse X") != 0)
            {
                isTurning = true;
            }
            else
            {
                isTurning = false;
            }

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

        // To see if a player is moving purposely
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // If Player is on a wall, adjust gravity and speed for how to handle while on a wall.
        // If player isn't make gravity 10
        if (onWall)
        {
            gravity = 1.5f;
            AdjustSpeed(wallSpeed);
        }
        else
        {
            gravity = 10.0f;
        }

        // What type of move should be used?
        if (onGround)
        {
            if (!isSliding)
            {
                Move();
            }
        }
        else if (!onGround && onWall)
        {
            Check4Wall();

            WallMove();
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
        }

        // Adjust speed back to regular move speed, sprint speed
        if (!isCrouched && onGround && !isSprinting)
        {
            AdjustSpeed(moveSpeed);
        }
        if (!isCrouched && onGround && isSprinting)
        {
            AdjustSpeed(sprintSpeed);
        }

        // Crouch
        if (Input.GetButtonDown("Slide"))
        {
            if (isCrouched)
            {
                isCrouched = false;
                AdjustSpeed(moveSpeed);
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
                //Debug.Log("SLIDE BUTTON PRESSED ELSE, speed = crouchSpeed");
                if (isSprinting && onGround)
                {
                    isSprinting = false;
                }
                isCrouched = true;
                AdjustSpeed(crouchSpeed);
            }
        }
        
        // Slide
        if (onGround && (xzVelocity.magnitude > 10 && isCrouched) && !isSliding)
        {
            if (isSprinting)
            {
                isSprinting = false;
            }

            //Debug.Log("Calling Slide()");
            Slide();
        }

        // While sliding is still true, check if magnitude goes below 5. If so, set sliding to false and set speed to crouch speed
        if (isSliding)
        {
            if (rigid.velocity.magnitude < 5)
            {
                isSliding = false;
                AdjustSpeed(crouchSpeed);
            }
        }
        
        if (!onGround)
        {
            isSliding = false;
        }

        // Apply manual hard-coded gravity
        rigid.AddForce(new Vector3(0, -gravity * rigid.mass, 0));
    }


}
