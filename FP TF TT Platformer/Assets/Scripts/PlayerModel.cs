using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : MonoBehaviour {

    public GameObject playerModel;

    public GameObject cam;

    CharacterControls controller;

    public Vector3 crouchPos, normalPos;

    // Was used for rotating the camera through code, still here if I can find a new implementation
    public float smoothing, wallcamRot;

    public bool transformed, wallChange;

    // Hash numbers for easily referencing animator values that dictate animation
    public int wallLeftHash;
    public int wallRightHash;

    // Animator that holds animations
    Animator camAnim;

    // Use this for initialization
    void Start ()
    {
        playerModel = GameObject.FindGameObjectWithTag("Player");

        controller = playerModel.GetComponent<CharacterControls>();

        cam = GameObject.FindGameObjectWithTag("MainCamera");

        camAnim = cam.GetComponent<Animator>();
        wallLeftHash = Animator.StringToHash("wallOnLeft");
        wallRightHash = Animator.StringToHash("wallOnRight");

        crouchPos = new Vector3(0f, 0f, 0f);

        normalPos = new Vector3(0f, 0.25f, 0f);

        smoothing = 0.5f;

        wallcamRot = 10f;

        transformed = false;
        wallChange = false;
    }

    // Funciton for Adjusting the Player's model when crouched and not crouched
    void HandleCrouch()
    {
        // Crouching and Uncrouching
        if (controller.onGround && controller.isCrouched && !transformed)
        {
            //Vector3 camcrouchPos = playerModel.transform.position;

            //Debug.Log(playerModel.transform.localScale.y + ", " + playerModel.transform.localScale.y / 2);
            // Immediately set the scale and position of the character
            playerModel.transform.localScale = new Vector3(1, playerModel.transform.localScale.y / 2, 1);
            playerModel.transform.position = new Vector3(playerModel.transform.position.x, playerModel.transform.position.y - 0.5f, playerModel.transform.position.z);

            //cam.transform.position = Vector3.Lerp(cam.transform.position, camcrouchPos, smoothing * Time.deltaTime);

            // Character has been changed to crouch form
            transformed = true;
        }
        else if(!controller.onGround && controller.isCrouched && !transformed)
        {
            // When not on ground, only adjust the scale for crouching
            playerModel.transform.localScale = new Vector3(1, playerModel.transform.localScale.y / 2, 1);

            // Character has been changed to crouch form
            transformed = true;
        }
        else if (controller.onGround && !controller.isCrouched && transformed)
        {
            //Vector3 camnormalPos = playerModel.transform.position + normalPos;

            // Immediately set the scale and position of the character
            playerModel.transform.localScale = new Vector3(1, playerModel.transform.localScale.y * 2, 1);
            playerModel.transform.position = new Vector3(playerModel.transform.position.x, playerModel.transform.position.y + 0.5f, playerModel.transform.position.z);

            //cam.transform.position = Vector3.Lerp(cam.transform.position, camnormalPos, smoothing * Time.deltaTime);
            
            // Character has been changed back to normal form
            transformed = false;
        }
        else if (!controller.onGround && !controller.isCrouched && transformed)
        {
            // When in the air, only adjust the scale of the player
            playerModel.transform.localScale = new Vector3(1, playerModel.transform.localScale.y * 2, 1);

            // Character has been changed back to normal form
            transformed = false;
        }

        
        
        /*
        if (playerModel.GetComponent<PlayerMovement>().isCrouched)
        {
            Debug.Log(playerModel.transform.localScale.y);
            transformed = true;
            playerModel.transform.localScale = Vector3.Lerp(playerModel.transform.localScale, new Vector3(1, 1, 1), smoothing * Time.deltaTime);

            cam.transform.position = Vector3.Lerp(cam.transform.position, camcrouchPos, smoothing * Time.deltaTime);
        }
        else if (transformed && !playerModel.GetComponent<PlayerMovement>().isCrouched)
        {
            Debug.Log(playerModel.transform.localScale.y);
            playerModel.transform.localScale = Vector3.Lerp(playerModel.transform.localScale, new Vector3(1, 2, 1), smoothing * Time.deltaTime);

            cam.transform.position = Vector3.Lerp(camcrouchPos, camnormalPos, smoothing * Time.deltaTime);
        }
        */
    }

    // Function to handle how to adjust the player model when on a wall
    void HandleWall()
    {
        // Find the trigonometry angles for where the player's immediate right currently is
        // There can be a better option by just using transform.right
        float radRight = Mathf.Deg2Rad * (playerModel.transform.localEulerAngles.y);
        
        float xRayDir = Mathf.Cos(-radRight);
        float zRayDir = Mathf.Sin(-radRight);

        // Create a vector for those anlges
        Vector3 rayDir = new Vector3(xRayDir, 0, zRayDir);

        //Debug.Log("Player Rotation: " + playerModel.transform.localEulerAngles.y +
            //"\nXRayDir: " + xRayDir + "ZRayDir: " + zRayDir);
        
        // Using the rayDir vector check and see if there is a wall to the immediate left or right of the player
        bool rayRight = Physics.Raycast(playerModel.transform.position, rayDir, 0.8f);
        bool rayLeft = Physics.Raycast(playerModel.transform.position, -rayDir, 0.8f);
        
        // Tell the animator boolean values what they should be
        // If the ray hit something while player is on a wall, the animation to handle it will become true and the animator will run accordingly through transitions
        camAnim.SetBool(wallLeftHash, rayLeft);

        camAnim.SetBool(wallRightHash, rayRight);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Only Call HandleWall if the player is currently on a wall
        // Else, the animator values will be false
        if (controller.onWall)
        {
            HandleWall();
        }
        else
        {
            camAnim.SetBool(wallLeftHash, false);

            camAnim.SetBool(wallRightHash, false);
        }

        // Call handle crouch at all times
        HandleCrouch();

        //if (controller.onGround && controller.isCrouched && !transformed)
        //{
            
        //}
        //else if(transformed && !controller.isCrouched && transBack)
        //{
        //    Vector3 camnormalPos = playerModel.transform.position + normalPos;

        //    playerModel.transform.localScale = new Vector3(1, playerModel.transform.localScale.y * 2, 1);
        //    playerModel.transform.position = new Vector3(playerModel.transform.position.x, playerModel.transform.position.y + 0.5f, playerModel.transform.position.z);

        //    //cam.transform.position = Vector3.Lerp(cam.transform.position, camnormalPos, smoothing * Time.deltaTime);

        //    transformed = false;
        //    transBack = false;
        //}
	}
}
