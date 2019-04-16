using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusic : MonoBehaviour
{
    public AudioClip[] music;

    public AudioClip prevMusic;

    public int randNum;

    public bool musicPicked;

    private int prevTrack;

	// Use this for initialization
	void Start ()
    {
        musicPicked = false;
        
	}
	
    public void PlaySelectionMusic()
    {
        prevMusic = this.GetComponent<AudioSource>().clip;
        this.GetComponent<AudioSource>().clip = music[1];
    }

    public void PlayCompletionMusic()
    {
        prevMusic = this.GetComponent<AudioSource>().clip;
        this.GetComponent<AudioSource>().clip = music[3];
    }

    public void PlayPrevMusic()
    {
        if (prevMusic != null)
        {
            this.GetComponent<AudioSource>().clip = prevMusic;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);

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
            else
            {
                while (prevTrack == randNum)
                {
                    randNum = Random.Range(4, music.Length - 1);
                }

                this.GetComponent<AudioSource>().clip = music[randNum];
            }
        }

        if (!musicPicked && this.GetComponent<AudioSource>().clip != null && !this.GetComponent<AudioSource>().isPlaying)
        {
            musicPicked = true;

            this.GetComponent<AudioSource>().Play();

            Debug.Log("Audio isPlaying: " + this.GetComponent<AudioSource>().isPlaying);
        }

        if (!this.GetComponent<AudioSource>().isPlaying)
        {
            musicPicked = false;

            prevTrack = randNum;
        }
	}
}
