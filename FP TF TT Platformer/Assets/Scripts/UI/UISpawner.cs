using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpawner : MonoBehaviour
{
    public GameObject hud;

    public GameObject pause;

    public GameObject complete;

	// Use this for initialization
	void Start ()
    {
        if (hud != null)
        {
            hud.SetActive(false);
        }

        if (pause != null)
        {
            pause.SetActive(false);
        }

        if (complete != null)
        {
            complete.SetActive(false);
        }
    }

    void DisplayHud()
    {
        hud.SetActive(true);
    }

    void DisplayPause()
    {
        if (!complete.activeSelf)
        {
            pause.SetActive(true);
        }
    }

    void DisplayCompletion()
    {
        if (!pause.activeSelf)
        {
            complete.SetActive(true);
        }
    }

    void TurnOffHud()
    {
        hud.SetActive(false);
    }

    void TurnOffPause()
    {
        pause.SetActive(false);
    }

    void TurnOffCompletion()
    {
        complete.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
