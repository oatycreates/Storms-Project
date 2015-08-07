using UnityEngine;
using System.Collections;
//This script keeps track of the Pirate Base and Identity - checks Current Score and Tags
public class PirateBaseIdentity : MonoBehaviour 
{

	public DetectFallingPassenger baseTriggerZone;

	[HideInInspector]
	public int baseScore;
	[HideInInspector]
	public int teamNumber;
	[HideInInspector]
	public Color baseColour;

	public Renderer myRenderer;

	void Start () 
	{
		baseColour = Color.clear;
	}

	void Update () 
	{

		if (gameObject.tag == "Player1_")
		{
			teamNumber = 1;
			myRenderer.material.color = Color.magenta;
			baseColour = myRenderer.material.color;
		}
		else
		if (gameObject.tag == "Player2_")
		{
			teamNumber = 2;
			myRenderer.material.color = Color.cyan;
			baseColour = myRenderer.material.color;
		}
		else
		if (gameObject.tag == "Player3_")
		{
			teamNumber = 3;
			myRenderer.material.color = Color.green;
			baseColour = myRenderer.material.color;
		}
		else
		if (gameObject.tag == "Player4_")
		{
			teamNumber = 4;
			myRenderer.material.color = Color.yellow;
			baseColour = myRenderer.material.color;
		}
		else
		if (gameObject.tag == "Untagged")
		{
			Debug.Log("NO TAG!!");
		}

		baseScore = baseTriggerZone.myScore;
	}
}
