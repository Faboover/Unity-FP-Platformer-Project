using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : MonoBehaviour {

    public GameObject playerModel;

    public GameObject cam;

    CharacterControls controller;

    public Vector3 crouchPos, normalPos;

    public float smoothing, wallcamRot;

    public bool transformed, wallChange;

    public int wallLeftHash;
    public int wallRightHash;

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

    void HandleCrouch()
    {
        // Crouching and Uncrouching
        if (controller.onGround && controller.isCrouched && !transformed)
        {
            //Vector3 camcrouchPos = playerModel.transform.position;

            //Debug.Log(playerModel.transform.localScale.y + ", " + playerModel.transform.localScale.y / 2);
            playerModel.transform.localScale = new Vector3(1, playerModel.transform.localScale.y / 2, 1);
            playerModel.transform.position = new Vector3(playerModel.transform.position.x, playerModel.transform.position.y - 0.5f, playerModel.transform.position.z);

            //cam.transform.position = Vector3.Lerp(cam.transform.position, camcrouchPos, smoothing * Time.deltaTime);

            transformed = true;
        }
        else if(!controller.onGround && controller.isCrouched && !transformed)
        {
            playerModel.transform.localScale = new Vector3(1, playerModel.transform.localScale.y / 2, 1);

            transformed = true;
        }
        else if (controller.onGround && !controller.isCrouched && transformed)
        {
            //Vector3 camnormalPos = playerModel.transform.position + normalPos;

            playerModel.transform.localScale = new Vector3(1, playerModel.transform.localScale.y * 2, 1);
            playerModel.transform.position = new Vector3(playerModel.transform.position.x, playerModel.transform.position.y + 0.5f, playerModel.transform.position.z);

            //cam.transform.position = Vector3.Lerp(cam.transform.position, camnormalPos, smoothing * Time.deltaTime);

            transformed = false;
        }
        else if (!controller.onGround && !controller.isCrouched && transformed)
        {
            playerModel.transform.localScale = new Vector3(1, playerModel.transform.localScale.y * 2, 1);

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

    void HandleWall()
    {
        float radRight = Mathf.Deg2Rad * (playerModel.transform.localEulerAngles.y);
        
        float xRayDir = Mathf.Cos(-radRight);
        float zRayDir = Mathf.Sin(-radRight);

        Vector3 rayDir = new Vector3(xRayDir, 0, zRayDir);

        //Debug.Log("Player Rotation: " + playerModel.transform.localEulerAngles.y +
            //"\nXRayDir: " + xRayDir + "ZRayDir: " + zRayDir);

        bool rayRight = Physics.Raycast(playerModel.transform.position, rayDir, 0.8f);
        bool rayLeft = Physics.Raycast(playerModel.transform.position, -rayDir, 0.8f);
        
        camAnim.SetBool(wallLeftHash, rayLeft);

        camAnim.SetBool(wallRightHash, rayRight);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (controller.onWall)
        {
            HandleWall();
        }
        else
        {
            camAnim.SetBool(wallLeftHash, false);

            camAnim.SetBool(wallRightHash, false);
        }

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
