using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Know if the goal has been reached
    public bool goal;

    public GameObject timer;

    // Music player of the scene
    public GameObject musicPlyr;

    // UI handler of teh scene
    public GameObject ui;

	// Use this for initialization
	void Start ()
    {
        // Set goal to false and find the GameObjects
        goal = false;

        timer = GameObject.FindGameObjectWithTag("Time");

        musicPlyr = GameObject.FindGameObjectWithTag("Music");

        ui = GameObject.FindGameObjectWithTag("UIHandler");
    }

    // If a player collider enters the trigger, the goal has been reached
    // Set goal to true, stop the timer, play level complete music, and display the results screen
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            goal = true;

            timer.GetComponent<Timer>().StopTimer();

            musicPlyr.GetComponent<SceneMusic>().PlayCompletionMusic();

            ui.GetComponent<UISpawner>().DisplayCompletion();
        }
    }

    // Make goal false
    public void Reset()
    {
        goal = false;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
