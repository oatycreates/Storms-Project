using UnityEngine;
using System.Collections;

public class PassengerDestroyScript : MonoBehaviour //This script deactivates the Pirate Passengers as soon as they go below a certain height. The name of this script is a Misnomer.
{
	public float heightTillDeath = -2000.0f;
	

	void Update () 
	{
		if (gameObject.activeInHierarchy)
		{
			if (gameObject.transform.position.y < heightTillDeath)
			{
				gameObject.SetActive(false); //Check timeout
				Debug.Log("Gone to sleep");
			}
		}
	}
}
