﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandleMouse : MonoBehaviour, IPointerEnterHandler
{
    // The Evenet System of the Scene
    public GameObject eventSys;

	// Use this for initialization
	void Start ()
    {
        // Find the Event System
        eventSys = GameObject.FindGameObjectWithTag("EventSystem");
	}

    // Check the events generated by the mouse
    // If the mouse hovers over a UI object we would want selected, set it to be so for the Event System
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered a UI element, " + eventData.pointerEnter.name);

        // If the event involves a button, set that button as selected in the Event System
        // Will overwrite the previously selected item
        if (eventData.pointerEnter.tag.Equals("Button"))
        {
            Debug.Log("The UI element is a button");
            eventSys.GetComponent<EventSystem>().SetSelectedGameObject(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}