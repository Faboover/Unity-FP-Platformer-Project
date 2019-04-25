using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public string[] scenes = {"Level1", "Level2", "Level3", "Level4", "Level5", "Credits"};

    // Use this for initialization
    void Start ()
    {
		
	}
	
    public void LoadTitle()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }

    public void LoadLvlSelect()
    {
        SceneManager.LoadScene("Level Select", LoadSceneMode.Single);
    }

    // Loads the next level
    public void NextLevel()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            if (SceneManager.GetActiveScene().name == scenes[i])
            {
                SceneManager.LoadScene(scenes[i + 1], LoadSceneMode.Single);
            }
        }
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    // Plays the game starting at level 1
    public void PlayGame()
    {
        SceneManager.LoadScene(scenes[0], LoadSceneMode.Single);
    }

    // Code for loading each level individually
    public void Load2()
    {
        SceneManager.LoadScene(scenes[1], LoadSceneMode.Single);
    }

    public void Load3()
    {
        SceneManager.LoadScene(scenes[2], LoadSceneMode.Single); ;
    }

    public void Load4()
    {
        SceneManager.LoadScene(scenes[3], LoadSceneMode.Single);
    }

    public void Load5()
    {
        SceneManager.LoadScene(scenes[4], LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
