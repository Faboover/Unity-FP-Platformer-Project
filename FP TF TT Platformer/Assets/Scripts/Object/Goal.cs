using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool goal;

    public GameObject timer;

	// Use this for initialization
	void Start ()
    {
        goal = false;

        timer = GameObject.FindGameObjectWithTag("Time");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered the goal!");
            goal = true;

            timer.GetComponent<Timer>().StopTImer();
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
