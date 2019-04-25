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

	// Use this for initialization
	void Start ()
    {
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
        }
    }

    public void DisplayCompletion()
    {
        if (!pause.activeSelf)
        {
            complete.SetActive(true);
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
        pause.SetActive(false);
    }

    public void TurnOffCompletion()
    {
        complete.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
