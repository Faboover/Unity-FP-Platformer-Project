using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public Text status;

    public string[] state = new string[7];

    public GameObject player;

    public CharacterControls plyrCntrl;

	// Use this for initialization
	void Start ()
    {
        status.text = state[0];


        plyrCntrl = player.GetComponent<CharacterControls>();

        Debug.Log(player.GetComponent<CharacterControls>());

	}
	
	// Update is called once per frame
	void Update ()
    {
		if (plyrCntrl.isMoving && !plyrCntrl.isSprinting && !plyrCntrl.isCrouched && plyrCntrl.onGround)
        {
            status.text = state[1];
        }
        else if (plyrCntrl.isSprinting && plyrCntrl.onGround)
        {
            status.text = state[2];
        }
        else if (plyrCntrl.isCrouched && plyrCntrl.onGround)
        {
            status.text = state[3];
        }
        else if (plyrCntrl.isSliding)
        {
            status.text = state[4];
        }
        else if (!plyrCntrl.onGround && !plyrCntrl.onWall)
        {
            status.text = state[5];
        }
        else if (plyrCntrl.onWall)
        {
            status.text = state[6];
        }
        else
        {
            status.text = state[0];
        }
	}
}
