using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    //public Text status;

    public Image status;

    public Sprite[] images = new Sprite[7];

    public string[] state = new string[7];

    public GameObject player;

    public CharacterControls plyrCntrl;

	// Use this for initialization
	void Start ()
    {
        //status.text = state[0];

        // Set the sprite for status to be the 0 element of images
        // The 0 element is the Idle icon
        status.sprite = images[0];
	}

    // Since player is not in the scene to start with, Find the player
    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        plyrCntrl = player.GetComponent<CharacterControls>();
    }
	
    // Function to determine which image sprite should be displayed
    private void DetermineStatus()
    {
        /*
         * 0 - Idle
         * 1 - Walking
         * 2 - Sprinting
         * 3 - Crouching
         * 4 - Sliding
         * 5 - In the Air
         * 6 - Wallrunning
         * 
         * Priotitization
         * Walking > Sprinting > In the Air > Sliding > Crouching > Wallrunning > Idle
         */ 
        if (plyrCntrl.isMoving && !plyrCntrl.isSprinting && !plyrCntrl.isCrouched && plyrCntrl.onGround)
        {
            //status.text = state[1];

            status.sprite = images[1];
        }
        else if (plyrCntrl.isSprinting && plyrCntrl.onGround)
        {
            //status.text = state[2];

            status.sprite = images[2];
        }
        else if (!plyrCntrl.onGround && !plyrCntrl.onWall)
        {
            //status.text = state[5];

            status.sprite = images[5];
        }
        else if (plyrCntrl.isSliding)
        {
            //status.text = state[4];

            status.sprite = images[4];
        }
        else if (plyrCntrl.isCrouched && plyrCntrl.onGround)
        {
            //status.text = state[3];

            status.sprite = images[3];
        }
        else if (plyrCntrl.onWall)
        {
            //status.text = state[6];

            status.sprite = images[6];
        }
        else
        {
            //status.text = state[0];

            status.sprite = images[0];
        }
    }

	// Update is called once per frame
	void Update ()
    {
        // If Player null, find the player
        // Once found, Determine which Status Icon to use
		if (player == null)
        {
            FindPlayer();
        }
        else
        {
            DetermineStatus();
        }
	}
}
