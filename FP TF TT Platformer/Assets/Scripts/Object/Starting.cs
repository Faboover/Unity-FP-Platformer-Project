using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : MonoBehaviour
{
    // Bool to know when the player starts the time by going through the trigger
    public bool start;

    public GameObject timer;

    // Use this for initialization
    void Start()
    {
        // Make start false to begin with
        start = false;

        timer = GameObject.FindGameObjectWithTag("Time");
    }

    // If a Player enters the trigger, either start the run or reset it
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !start)
        {
            StartRun();
            return;
        }

        if (other.gameObject.tag == "Player" && start)
        {
            Reset();
            return;
        }
    }

    // Starts the timer of the level
    public void StartRun()
    {
        start = true;
        timer.GetComponent<Timer>().ResumeTimer();
    }

    // Stops and resets the timer of the level
    public void Reset()
    {
        start = false;
        timer.GetComponent<Timer>().ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
