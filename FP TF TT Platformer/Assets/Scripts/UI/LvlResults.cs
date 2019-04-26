using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LvlResults : MonoBehaviour
{
    public GameObject timer;

    public GameObject spawner;

	// Use this for initialization
	void Start ()
    {
        timer = GameObject.FindGameObjectWithTag("Time");

        spawner = GameObject.FindGameObjectWithTag("Spawner");
	}
	
    public string GetResults()
    {
        string text = "LEVEL\n\t\tLevel ";

        Debug.Log(SceneManager.GetActiveScene().name);
        string scene = SceneManager.GetActiveScene().name;

        for (int i = 5; i < scene.Length; i++)
        {
            text += scene[i];
        }

        text += "\n\nTIME\n\t\t";

        text += timer.GetComponent<Text>().text;

        text += "\n\n# OF RESTARTS\n\t\t";

        text += spawner.GetComponent<PlayerSpawner>().respawnCount;

        Debug.Log("Get Results: " + text);

        return text;
    }

    public void UpdateText()
    {
        this.GetComponent<Text>().text = GetResults();
    }

	// Update is called once per frame
	void Update ()
    {

	}
}
