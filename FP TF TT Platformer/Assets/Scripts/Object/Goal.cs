using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool goal;

    public GameObject timer;

    public GameObject musicPlyr;

	// Use this for initialization
	void Start ()
    {
        goal = false;

        timer = GameObject.FindGameObjectWithTag("Time");

        musicPlyr = GameObject.FindGameObjectWithTag("Music");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            goal = true;

            timer.GetComponent<Timer>().StopTimer();

            musicPlyr.GetComponent<SceneMusic>().PlayCompletionMusic();
        }
    }

    public void Reset()
    {
        goal = false;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
