using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour {

    public GameObject playerModel;

    public GameObject cam;

    public Vector3 crouchPos, normalPos;

    public float smoothing;

    public bool transformed;

    // Use this for initialization
    void Start ()
    {
        playerModel = GameObject.FindGameObjectWithTag("Player");

        cam = GameObject.FindGameObjectWithTag("MainCamera");

        crouchPos = new Vector3(0f, 0f, 0f);

        normalPos = new Vector3(0f, 0.25f, 0f);

        smoothing = 5f;

        transformed = false;
    }
	
	// Update is called once per frame
	void Update ()
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
}
