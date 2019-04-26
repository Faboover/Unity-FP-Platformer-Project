using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
    // UI Object to display Time on
    public Text time;

    float minutes = 0f;
	float seconds = 0f;
	float milliseconds = 0f;
	string minutesS = "";
	string secondsS = "";
	string millisecondsS = "";

    public bool stop;

	// Use this for initialization
	void Start ()
    {
        stop = true;

        minutesS = "00";
        secondsS = "00";
        millisecondsS = "00";
    }

    // Set stop to be true
    public void StopTimer()
    {
        //Debug.Log("Stopping Timer!!");

        stop = true;
    }

    // Set stop to be false
    public void ResumeTimer()
    {
        //Debug.Log("RESUMING TIMER");
        stop = false;
    }

    // Set Stop to be true and reset all the values
    public void ResetTimer()
    {
        stop = true;

        minutes = 0f;
        seconds = 0f;
        milliseconds = 0f;

        minutesS = "00";
        secondsS = "00";
        millisecondsS = "00";
    }

	void Update()
    {
        // If the stop is false, the timer can count up normally
        // Else, do nothing
        if (!stop)
        {
            if (milliseconds >= 100)
            {
                if (seconds >= 59)
                {
                    minutes++;
                    seconds = 0;
                }
                else if (seconds < 59)
                {
                    seconds++;
                }
                milliseconds = 0;
            }

            milliseconds += Time.deltaTime * 100;

            if (minutes < 10)
            {
                minutesS = "0" + minutes;
            }
            else
            {
                minutesS = "" + minutes;
            }

            if (seconds < 10)
            {
                secondsS = "0" + seconds;
            }
            else
            {
                secondsS = "" + seconds;
            }

            // Convert to integers to make them whole numbers
            if ((int)milliseconds < 10)
            {
                millisecondsS = "0" + (int)milliseconds;
            }
            else
            {
                millisecondsS = "" + (int)milliseconds;
            }
        }

        // Always have the text for the timer to be set by the calculated values
        time.text = minutesS + ":" + secondsS + ":" + millisecondsS;
	}
}
