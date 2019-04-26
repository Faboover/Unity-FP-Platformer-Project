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
        hud = GameObject.FindGameObjectWithTag("HUD");

        pause = GameObject.FindGameObjectWithTag("Pause");

        complete = GameObject.FindGameObjectWithTag("Results");

        options = GameObject.FindGameObjectWithTag("Options");

        if (pause != null)
        {
            pause.SetActive(false);
        }

        if (complete != null)
        {
            complete.SetActive(false);
        }

        paused = false;
    }

    public void DisplayHud()
    {
        hud.SetActive(true);
    }

    public void DisplayPause()
    {
        if (!complete.activeSelf)
        {
            pause.SetActive(true);

            pause.GetComponent<TellEventSys>().FindFirstButton();

            paused = true;
        }
    }

    public void DisplayCompletion()
    {
        if (!pause.activeSelf)
        {
            complete.SetActive(true);

            complete.GetComponent<TellEventSys>().FindFirstButton();
        }
    }

    public void DisplayOptions()
    {
        Debug.Log("Options Not Yet Implemented");
    }

    public void TurnOffHud()
    {
        hud.SetActive(false);
    }

    public void TurnOffPause()
    {
        pause.GetComponent<Pause>().Off();

        pause.SetActive(false);

        paused = false;
    }

    public void TurnOffCompletion()
    {
        complete.GetComponent<Pause>().Off();

        complete.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
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
