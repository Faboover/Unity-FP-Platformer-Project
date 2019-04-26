using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    // Used to load levels as the player plays the game
    // If the player plays all the way through, they should reach the Credits scene
    public string[] scenes = {"Level1", "Level2", "Level3", "Level4", "Level5", "Credits"};

    // Use this for initialization
    void Start ()
    {
		
	}
	
    // End the game by closing the application
    public void CloseGame()
    {
        Application.Quit();
    }

    // Load the Title Scene
    public void LoadTitle()
    {
        // LoadSceneMode.Single makes it so only one scene loaded at a time and not multiple
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }

    // Load the Level Select Scene
    public void LoadLvlSelect()
    {
        // LoadSceneMode.Single makes it so only one scene loaded at a time and not multiple
        SceneManager.LoadScene("Level Select", LoadSceneMode.Single);
    }

    // Loads the next level
    public void NextLevel()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            if (SceneManager.GetActiveScene().name == scenes[i])
            {
                // LoadSceneMode.Single makes it so only one scene loaded at a time and not multiple
                SceneManager.LoadScene(scenes[i + 1], LoadSceneMode.Single);
            }
        }
    }

    public void LoadCredits()
    {
        // LoadSceneMode.Single makes it so only one scene loaded at a time and not multiple
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    // Plays the game starting at level 1
    public void PlayGame()
    {
        // LoadSceneMode.Single makes it so only one scene loaded at a time and not multiple
        SceneManager.LoadScene(scenes[0], LoadSceneMode.Single);
    }

    // Code for loading each level individually
    public void Load2()
    {
        // LoadSceneMode.Single makes it so only one scene loaded at a time and not multiple
        SceneManager.LoadScene(scenes[1], LoadSceneMode.Single);
    }

    public void Load3()
    {
        // LoadSceneMode.Single makes it so only one scene loaded at a time and not multiple
        SceneManager.LoadScene(scenes[2], LoadSceneMode.Single); ;
    }

    public void Load4()
    {
        // LoadSceneMode.Single makes it so only one scene loaded at a time and not multiple
        SceneManager.LoadScene(scenes[3], LoadSceneMode.Single);
    }

    public void Load5()
    {
        // LoadSceneMode.Single makes it so only one scene loaded at a time and not multiple
        SceneManager.LoadScene(scenes[4], LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
