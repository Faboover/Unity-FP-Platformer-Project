using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public enum Type {Title, Credits, Select, Resume, Restart, Next, Options};

    public Type type;

    public Sprite[] images = new Sprite[2];

    public bool isSelected = false;

    public bool pressed = false;

    public GameObject eventHandler;

	// Use this for initialization
	void Start ()
    {
        eventHandler = GameObject.FindGameObjectWithTag("Handler");
	}
	
    private void Highlight()
    {
        this.GetComponent<Image>().sprite = images[1];
    }

    private void Deselect()
    {
        this.GetComponent<Image>().sprite = images[0];
    }

    private void ButtonPressed()
    {
        pressed = false;
    }

	// Update is called once per frame
	void Update ()
    {
		if (isSelected)
        {
            Highlight();
        }
        else
        {
            Deselect();
        }

        if (pressed)
        {
            ButtonPressed();
        }
	}
}
