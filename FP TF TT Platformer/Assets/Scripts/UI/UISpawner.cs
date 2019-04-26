using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpawner : MonoBehaviour
{
    public GameObject hud;

    public GameObject pause;

    public GameObject complete;

    public GameObject options;

    public bool paused;


	// Use this for initialization
	void Start ()
    {
        // Get each object in a scene
        hud = GameObject.FindGameObjectWithTag("HUD");

        pause = GameObject.FindGameObjectWithTag("Pause");

        complete = GameObject.FindGameObjectWithTag("Results");

        options = GameObject.FindGameObjectWithTag("Options");

        // If the objects are found, set them to null
        if (pause != null)
        {
            pause.SetActive(false);
        }

        if (complete != null)
        {
            complete.SetActive(false);
        }

        // Set paused to be false, the scene only just started
        paused = false;
    }

    // Function to display HUD by making it active
    public void DisplayHud()
    {
        hud.SetActive(true);
    }

    // Function to display Pause Screen by making it active
    public void DisplayPause()
    {
        if (!complete.activeSelf)
        {
            pause.SetActive(true);

            pause.GetComponent<TellEventSys>().FindFirstButton();

            paused = true;
        }
    }

    // Function to display Results Screen by making it active
    public void DisplayCompletion()
    {
        if (!pause.activeSelf)
        {
            complete.SetActive(true);

            complete.GetComponent<TellEventSys>().FindFirstButton();
        }
    }

    // Function to display Options Menu, but is not implemented yet
    public void DisplayOptions()
    {
        Debug.Log("Options Not Yet Implemented");
    }

    // Funciton to turn off the HUD
    public void TurnOffHud()
    {
        hud.SetActive(false);
    }

    // Function to turn off the Pause Screen
    public void TurnOffPause()
    {
        // Tell the screen's pause component that it is to be turned off
        pause.GetComponent<Pause>().Off();

        pause.SetActive(false);

        paused = false;
    }

    // Function to turn off the Results Screen
    public void TurnOffCompletion()
    {
        // Tell the screen's pause component that it is to be turned off
        complete.GetComponent<Pause>().Off();

        complete.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        // Display and turn off pause screen when given these project inputs
		if (Input.GetButtonDown("Pause") && !paused)
        {
            DisplayPause();
        }
        else if (Input.GetButtonDown("Cancel") && paused)
        {
            Debug.Log("Cancel Pressed");
            TurnOffPause();
        }
	}
}
