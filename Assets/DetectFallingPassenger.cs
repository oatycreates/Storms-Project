using UnityEngine;
using System.Collections;
using UnityEngine.UI;	
//This script detects falling passenger pirates that enter the triggerzone. It deactivates the passengers and adds to player score(by subtracting from regular score).
public class DetectFallingPassenger : MonoBehaviour 
{
	private Text myText;
	[HideInInspector]
	public int myScore = 50;

	void Start () 
	{
	
	}

	void Update () 
	{
	
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
