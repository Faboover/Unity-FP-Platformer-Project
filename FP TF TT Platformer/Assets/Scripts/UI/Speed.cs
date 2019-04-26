using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speed : MonoBehaviour
{
    public GameObject player;

    public Text speed;

	// Use this for initialization
	void Start ()
    {

	}
	
    // Find the player, not in start because player isn't in the scene at start
    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	// Update is called once per frame
	void Update ()
    {
        // If player is null, find them
        // Else, set the speed text of the player by calling GetXZMag
        if (player == null)
        {
            FindPlayer();
        }
        else
        {
            speed.text = "" + player.GetComponent<CharacterControls>().GetXZMag();
        }
	}
}
