using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject player;

    public GameObject timer;
	// Use this for initialization
	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        timer = GameObject.FindGameObjectWithTag("Timer");

        timer.GetComponent<Timer>().StopTimer();
	}

    public void Off()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
