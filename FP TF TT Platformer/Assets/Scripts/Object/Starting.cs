using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : MonoBehaviour
{
    public bool start;

    public GameObject timer;

    // Use this for initialization
    void Start()
    {
        start = false;

        timer = GameObject.FindGameObjectWithTag("Time");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !start)
        {
            Debug.Log("Player entered the goal!");
            StartRun();
            return;
        }

        if (other.gameObject.tag == "Player" && start)
        {
            Reset();
            return;
        }
    }

    public void StartRun()
    {
        start = true;
        timer.GetComponent<Timer>().ResumeTimer();
    }

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
