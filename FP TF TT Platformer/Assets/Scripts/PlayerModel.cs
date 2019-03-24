using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : MonoBehaviour {

    public GameObject playerModel;

    public Text test;

    public GameObject cam;

    CharacterControls controller;

    public Vector3 crouchPos, normalPos;

    public float smoothing, wallcamRot;

    public bool transformed, wallChange;

    // Use this for initialization
    void Start ()
    {
        playerModel = GameObject.FindGameObjectWithTag("Player");

        controller = playerModel.GetComponent<CharacterControls>();

        cam = GameObject.FindGameObjectWithTag("MainCamera");

        crouchPos = new Vector3(0f, 0f, 0f);

        normalPos = new Vector3(0f, 0.25f, 0f);

        smoothing = 5f;

        wallcamRot = 10f;

        transformed = false;
        wallChange = false;
    }

    void HandleCrouch()
    {
        Vector3 camnormalPos = playerModel.transform.position + normalPos;

        Vector3 camcrouchPos = playerModel.transform.position;

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
    }

    void HandleWall()
    {
        if (controller.onWall)
        {
            float radRight = Mathf.Deg2Rad * (playerModel.transform.localEulerAngles.y);


            float xRayDir = Mathf.Cos(-radRight);
            float zRayDir = Mathf.Sin(-radRight);

            Vector3 rayDir = new Vector3(xRayDir, 0, zRayDir);

            //Debug.Log("Player Rotation: " + playerModel.transform.localEulerAngles.y +
                //"\nXRayDir: " + xRayDir + "ZRayDir: " + zRayDir);

            bool rayRight = Physics.Raycast(playerModel.transform.position, rayDir, 0.6f);
            bool rayLeft = Physics.Raycast(playerModel.transform.position, -rayDir, 0.6f);

            test.text = "Player Rotation: " + playerModel.transform.localEulerAngles.y +
                "\nCamZRot: " + cam.transform.localEulerAngles.z +
                "\nXRayDir: " + xRayDir + "\nZRayDir: " + zRayDir +
                "\nLeft Ray: " + rayDir +
                "\nRight Ray: " + -rayDir + 
                "\nLeft: " + rayLeft + 
                "\nRight: " + rayRight;

            if (rayRight)
            {
                cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, wallcamRot), smoothing * Time.deltaTime);
            }
            else if(rayLeft)
            {
                if (cam.transform.localEulerAngles.z > 359)
                {
                    cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 359);
                }
                else if(cam.transform.localEulerAngles.z < 1)
                {
                    cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 359);
                }

                cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 360 - (wallcamRot)), smoothing * Time.deltaTime);
            }
            else
            {
                if (cam.transform.localEulerAngles.z != 0)
                {
                    if (cam.transform.localEulerAngles.z >= 355)
                    {
                        //Debug.Log("On Wall, Angle >= 355: Hard coding z angle to 0");
                        cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 0);
                    }
                    else if (cam.transform.localEulerAngles.z < 355 && cam.transform.localEulerAngles.z >= 340)
                    {
                        //Debug.Log("On Wall, CamZ is between 359 and 340");
                        cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 359), (smoothing * 9) * Time.deltaTime);
                    }
                    else if (cam.transform.localEulerAngles.z <= 5)
                    {
                        //Debug.Log("On Wall, Angle <= 5: Hard coding z angle to 0");
                        cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 0);
                    }
                    else if (cam.transform.localEulerAngles.z > 5) //&& cam.transform.localEulerAngles.z <= 0)
                    {
                        //Debug.Log("On Wall, CamZ is > 5");
                        cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 4), (smoothing * 9) * Time.deltaTime);
                    }
                    else
                    {
                        cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 0), (smoothing * 9) * Time.deltaTime);
                    }
                }
            }
        }
        else
        {
            if (cam.transform.localEulerAngles.z != 0)
            {
                if (cam.transform.localEulerAngles.z >= 359 - 0.5)    
                {
                    //Debug.Log("Off Wall, Angle >= 359 - 0.5: Hard coding z angle to 0");
                    cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 0);
                }
                else if (cam.transform.localEulerAngles.z < 359 && cam.transform.localEulerAngles.z >= 340)
                {
                    //Debug.Log("Off Wall, CamZ is between 359 and 340");
                    cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 359), smoothing * Time.deltaTime);
                }
                else
                {
                    //Debug.Log("Off Wall, Normal Lerp to rotation of 0");
                    cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 0), smoothing * Time.deltaTime);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

        HandleWall();
		
	}
}
