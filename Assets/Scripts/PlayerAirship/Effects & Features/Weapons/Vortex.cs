/**
 * File: Vortex.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 01/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Drags rigidbodies towards gameobject. - Code converted from RoDo's old 2Wicky project. 
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
	/// <summary>
	/// Vortex - basically a physics black hole - if a rigidbody is inside the trigger, it is dragged towards the gameobject's center.
	/// </summary>
	public class Vortex : MonoBehaviour 
	{
		public float maxGravityPull = 5000.0f;
		public float rotateForce = 50.0f;

		private GameObject airship = null;

		//Cheap workarounds
		private GameObject airship2 = null;
		private GameObject airship3 = null;
		private GameObject airship4 = null;

		void FixedUpdate()
		{
			//Make gravity always negative value
			maxGravityPull = -(Mathf.Abs (maxGravityPull));

			/*
			if (Application.isEditor)
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					gameObject.SetActive(false);
				}
			}
			*/

			if (airship != null)
			{
				Attract(airship);
			}

			if (airship2 != null)
			{
				Attract(airship2);
			}

			if (airship3 != null)
			{
				Attract(airship3);
			}

			if (airship4 != null)
			{
				Attract(airship4);
			}

		}

		void OnTriggerEnter(Collider other)
		{
			//A temporary workaround.
			if (airship == null)
			{
				//if (other.gameObject.tag == "Player1_" || other.gameObject.tag == "Player2_"  || other.gameObject.tag == "Player3_"  || other.gameObject.tag == "Player4_" )
				if (other.gameObject.tag == "Player1_")
				{
					if (other.gameObject.transform.root.gameObject.GetComponent<Rigidbody>() != null)
					{
						airship = other.gameObject.transform.root.gameObject;
					}
				}
			}

			if (airship2 == null)
			{
				if (other.gameObject.tag == "Player2_")
				{
					if (other.gameObject.transform.root.gameObject.GetComponent<Rigidbody>() != null)
					{
						airship2 = other.gameObject.transform.root.gameObject;
					}
				}
			}
			
			if (airship3 == null)
			{
				if (other.gameObject.tag == "Player3_")
				{
					if (other.gameObject.transform.root.gameObject.GetComponent<Rigidbody>() != null)
					{
						airship3 = other.gameObject.transform.root.gameObject;
					}
				}
			}
			
			if (airship4 == null)
			{
				if (other.gameObject.tag == "Player4_")
				{
					if (other.gameObject.transform.root.gameObject.GetComponent<Rigidbody>() != null)
					{
						airship4 = other.gameObject.transform.root.gameObject;
					}
				}
			}
		}

		void OnTriggerExit(Collider other)
		{
			//Detach airship
			if (airship != null)
			{
				if (other.gameObject.transform.root.gameObject == airship)
				{
					airship = null;
				}
			}

			if (airship2 != null)
			{
				if (other.gameObject.transform.root.gameObject == airship2)
				{
					airship2 = null;
				}
			}

			if (airship3 != null)
			{
				if (other.gameObject.transform.root.gameObject == airship3)
				{
					airship3 = null;
				}
			}

			if (airship4 != null)
			{
				if (other.gameObject.transform.root.gameObject == airship4)
				{
					airship4 = null;
				}
			}
		}


		public void Attract(GameObject attractedBody)
		{
			//Attraction
			Vector3 gravityUp = (attractedBody.transform.position - gameObject.transform.position).normalized;
			Vector3 bodyback = -(attractedBody.transform.forward);

			attractedBody.GetComponent<Rigidbody> ().AddForce (gravityUp * maxGravityPull);

			//Rotation

			Quaternion targetRotation = Quaternion.FromToRotation (bodyback, gravityUp) * attractedBody.transform.rotation;

			attractedBody.transform.rotation = Quaternion.Slerp (attractedBody.transform.rotation, targetRotation, rotateForce * Time.deltaTime);

		}

	}
}
