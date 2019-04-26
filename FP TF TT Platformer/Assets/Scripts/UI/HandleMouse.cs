using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandleMouse : MonoBehaviour, IPointerEnterHandler
{
    public GameObject eventSys;

	// Use this for initialization
	void Start ()
    {
        eventSys = GameObject.FindGameObjectWithTag("EventSystem");
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered a UI element, " + eventData.pointerEnter.name);

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
