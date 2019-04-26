using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TellEventSys : MonoBehaviour
{
    // Array of buttons
    public GameObject[] buttons;

    // The Event System of the scene
    public GameObject eventSys;

    // UI Element to select found
    public bool found;

    // Use this for initialization
    void Start()
    {
        // Find the Event System in the Scene
        eventSys = GameObject.FindGameObjectWithTag("EventSystem");

        // Have found to be true
        found = true;
    }

    // Function to sort through a list of buttons and have the one that is highest on the canvas be selected by the Event System
    public void FindFirstButton()
    {
        Debug.Log("Finding first button in menu");
        // Get all buttons in the scene
        buttons = GameObject.FindGameObjectsWithTag("Button");

        // Make first instance be the first button found to start with
        // Will use it to compare
        GameObject buttonFound = buttons[0];

        // Found means a new button with a higher Y position has been found, make sure it should be the first selected
        while (found)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                // If an instance of a button found is higher that the current button, make that button be the one found
                // Else, the current button has been compared against all others and has remained. It should be the one selected then
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

        found = true;

        // As long as the button isn't already selected by the Event System, set the button to be selected
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
