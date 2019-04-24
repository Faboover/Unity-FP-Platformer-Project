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

        status.sprite = images[0];
	}

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        plyrCntrl = player.GetComponent<CharacterControls>();
    }
	
    private void DetermineStatus()
    {
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
