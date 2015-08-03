using UnityEngine;
using System.Collections;

public class TagChildren : MonoBehaviour //This script will pass itself through EVERY child object of the Airship 
{										// This can be useful for collisions, inputs and cam references. 
										//I.e. The Cannonball collisions will check for the object's tag. If all objects, have the same tag as parent, then there is no confusion.

	void Start()
	{
		foreach (Transform child in gameObject.transform)
		{
			child.gameObject.tag = gameObject.tag;	// make the child tag == my tag
			child.gameObject.AddComponent<TagChildren>();
		}
	}
}
