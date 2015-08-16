using UnityEngine;
using System.Collections;
//A visual indicator on when the player can use the Stall feature.
[RequireComponent(typeof(Light))]
public class StallLightScript : MonoBehaviour 
{
	public StateManager stateManager;
	
	private Light myLight;
	private float checkStateManager;

	void Start () 
	{
		myLight = gameObject.GetComponent<Light>();
		myLight.color = Color.red;
	}
	
	void Update () 
	{
		checkStateManager = stateManager.timeBetweenStall;
		
		if (checkStateManager > 0)
		{
			myLight.color = Color.red;
		}
		else
		if (checkStateManager <= 0)
		{
			myLight.color = Color.green;
		}
	}
}
