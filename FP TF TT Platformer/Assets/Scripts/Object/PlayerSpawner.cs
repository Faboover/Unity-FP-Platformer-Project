using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;

    public int respawnCount = 0;

	// Use this for initialization
	void Start ()
    {
        // Spawn the player object at the positon of the spawner with the spawner's rotation
        Instantiate(player, this.transform.position, this.transform.rotation);

        player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<CharacterControls>().spawner = this.gameObject;
    }
	

    // Place player back at the respawn point during gameplay
    // Make it so player matches the position and rotation of the spawn object
    // Make it so Velocity of the rigid body is zeroed out
    // Tell Player Object that this object is the spawner to utilize
    public void Respawn()
    {
        player.transform.position = this.transform.position;

        player.transform.rotation = this.transform.rotation;

        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        respawnCount++;
    }

    // Funciton to be called once level is restarted after being completed (Goal Reached)
    // Sets teh respawn count back to 0
    // Respawn count is for seeing how many times a player restarted/respawned during the level
    public void ResetCount()
    {
        respawnCount = 0;
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
