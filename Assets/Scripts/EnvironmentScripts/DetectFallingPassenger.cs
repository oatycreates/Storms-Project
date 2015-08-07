using UnityEngine;
using System.Collections;
using UnityEngine.UI;	
//This script detects falling passenger pirates that enter the triggerzone. It deactivates the passengers and adds to player score(by subtracting from regular score).
public class DetectFallingPassenger : MonoBehaviour 
{
	public Text scoreText;
	[HideInInspector]
	public int myScore = 50;

	public Color textColour;

	void Start()
	{
		textColour = Color.black;
	}


	void Update () 
	{
		scoreText.color = textColour;
		scoreText.text = ("" + myScore);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Passengers")
		{
			myScore -= 1;
			other.gameObject.SetActive(false);
		}
	}
}
