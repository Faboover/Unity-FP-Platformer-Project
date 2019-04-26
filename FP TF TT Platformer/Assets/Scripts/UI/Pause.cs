using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject player;

    public GameObject cam;

    public GameObject timer;

    public GameObject start;

    public GameObject textArea;

	// Use this for initialization
	void Start ()
    {
        start = GameObject.FindGameObjectWithTag("Start");
	}

    public void Off()
    {
        if (start.GetComponent<Starting>().start)
        {
            timer.GetComponent<Timer>().ResumeTimer();
        }

        cam.GetComponent<CameraControl>().UnPause();

        player.GetComponent<CharacterControls>().UnPausePlayer();
    }

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
        On();
	}
}
