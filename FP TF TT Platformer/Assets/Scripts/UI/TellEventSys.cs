using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TellEventSys : MonoBehaviour
{
    public GameObject[] buttons;

    public GameObject eventSys;

    public bool found;

    // Use this for initialization
    void Start()
    {
        eventSys = GameObject.FindGameObjectWithTag("EventSystem");

        found = true;
    }

    public void FindFirstButton()
    {
        Debug.Log("Finding first button in menu");
        buttons = GameObject.FindGameObjectsWithTag("Button");

        GameObject buttonFound = buttons[0];

        while (found)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttonFound.transform.position.y < buttons[i].transform.position.y)
                {
                    found = true;

                    buttonFound = buttons[i];
                }
                else
                {
                    found = false;
                }
            }
        }


        if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject != buttonFound)
        {
            eventSys.GetComponent<EventSystem>().SetSelectedGameObject(buttonFound);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
