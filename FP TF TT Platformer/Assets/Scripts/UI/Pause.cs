using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject player;

    public GameObject cam;

    public GameObject timer;

    public GameObject start;

    // Used for when attached to the Results Screen
    public GameObject textArea;

	// Use this for initialization
	void Start ()
    {
        // Find the Start object of the scene
        start = GameObject.FindGameObjectWithTag("Start");
	}

    // Have all the things paused get un-paused
    public void Off()
    {
        // If the start object started the timer, have the timer resume
        if (start.GetComponent<Starting>().start)
        {
            timer.GetComponent<Timer>().ResumeTimer();
        }

        cam.GetComponent<CameraControl>().UnPause();

        player.GetComponent<CharacterControls>().UnPausePlayer();
    }

    // Function to pause the important objects of a scene
    // Player, Timer, Camera
    // If it is the Result Screen causing this, update the results display as well
    public void On()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        cam = GameObject.FindGameObjectWithTag("Camera");

        timer = GameObject.FindGameObjectWithTag("Time");

        if (this.tag.Equals("Results"))
        {
            textArea.GetComponent<LvlResults>().UpdateText();
        }

        if (timer != null && player != null && cam != null)
        {
            timer.GetComponent<Timer>().StopTimer();

            cam.GetComponent<CameraControl>().PauseCamRot();

            player.GetComponent<CharacterControls>().PausePlayer();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Since the UI element this will most likely be attached to will be turned off and on
        // On needs to be called at all times and will not run if the object is no longer active in a scene
        On();
	}
}
