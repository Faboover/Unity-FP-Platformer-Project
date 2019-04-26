using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusic : MonoBehaviour
{
    // Array of music tracks
    public AudioClip[] music;

    // The previous music that was playing
    public AudioClip prevMusic;

    public int randNum;

    // Bool to know when music has been picked and played
    public bool musicPicked;

    // Array value of a previously played track
    private int prevTrack;

	// Use this for initialization
	void Start ()
    {
        musicPicked = false;
	}
	
    // Play the Selection Music track
    public void PlaySelectionMusic()
    {
        prevMusic = this.GetComponent<AudioSource>().clip;
        this.GetComponent<AudioSource>().clip = music[1];
        this.GetComponent<AudioSource>().Play();
        musicPicked = true;
    }

    // Play the Level Completed Music track
    public void PlayCompletionMusic()
    {
        if (this.GetComponent<AudioSource>().clip != music[3])
        {
            Debug.Log("Completion Music Played");
            prevMusic = this.GetComponent<AudioSource>().clip;
            this.GetComponent<AudioSource>().clip = music[3];
            this.GetComponent<AudioSource>().Play();
            musicPicked = true;
        }
    }

    // Play the previously played music track
    public void PlayPrevMusic()
    {
        if (prevMusic != null)
        {
            this.GetComponent<AudioSource>().clip = prevMusic;
            this.GetComponent<AudioSource>().Play();
            musicPicked = true;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);

        // Based on the scene pick a music track to play
        if (!musicPicked)
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name.Equals("Title"))
            {
                Debug.Log("Scene is the Title Screen");
                this.GetComponent<AudioSource>().clip = music[0];
            }
            else if (SceneManager.GetActiveScene().name.Equals("Credits"))
            {
                this.GetComponent<AudioSource>().clip = music[2];
            }
            else if (SceneManager.GetActiveScene().name.Equals("Level Select"))
            {
                PlaySelectionMusic();
            }
            else
            {
                while (prevTrack == randNum)
                {
                    randNum = Random.Range(4, music.Length - 1);
                }

                this.GetComponent<AudioSource>().clip = music[randNum];
            }
        }

        // If The clip is not null and is currently not playing, play the clip
        if (!musicPicked && this.GetComponent<AudioSource>().clip != null && !this.GetComponent<AudioSource>().isPlaying)
        {
            musicPicked = true;

            this.GetComponent<AudioSource>().Play();

            Debug.Log("Audio isPlaying: " + this.GetComponent<AudioSource>().isPlaying);
        }

        // If the clip is no longer playing, set musicPicked back to false and the prevTrack to be the random number generated
        if (!this.GetComponent<AudioSource>().isPlaying)
        {
            musicPicked = false;

            prevTrack = randNum;
        }
	}
}
