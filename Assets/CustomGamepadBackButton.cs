using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomGamepadBackButton : MonoBehaviour 
{
	public Button backButton;
	
	void Update () 
	{
		if ((Input.GetButtonDown("Player1_FaceRight")) || (Input.GetKeyDown(KeyCode.Escape)))
		{
			backButton.interactable = true;
			Invoke("TurnOff", 1.0f);
			backButton.Select();
			backButton.onClick.Invoke();
		}/*
		else
		if (Input.GetButtonDown("Player2_FaceRight"))
		{
			backButton.interactable = true;
			Invoke("TurnOff", 1.0f);
			backButton.Select();
			backButton.onClick.Invoke();
		}
		else
		if (Input.GetButtonDown("Player3_FaceRight"))
		{
			backButton.interactable = true;
			Invoke("TurnOff", 1.0f);
			backButton.Select();
			backButton.onClick.Invoke();
		}
		else
		if (Input.GetButtonDown("Player4_FaceRight"))
		{
			backButton.interactable = true;
			Invoke("TurnOff", 1.0f);
			backButton.Select();
			backButton.onClick.Invoke();
		}*/
	}
	
	void TurnOff()
	{
		backButton.interactable = false;
	}
}
