/**
 * File: DelayChaff.cs
 * Author: Patrick Ferguson
 * Maintainers: 
 * Created: 24/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Dpad slowing field weapon.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	/// <summary>
	/// Delay chaff.
	/// </summary>
	//A lot of this script is derived from the Slip Stream script. Same basic function - when player enters trigger zone, effect player's physical movement.
	[RequireComponent(typeof(BoxCollider))]
	public class DelayChaff : MonoBehaviour 
	{
		private BoxCollider myCollider;
		
		Rigidbody[] m_playerRigidBodies = new Rigidbody[4];

		void GetPlayerRigidBodies()
		{
			// Airships are the only things that will have AirshipControlBehaviours, this will only change if we refactor the input system
			AirshipControlBehaviour[] objs = GameObject.FindObjectsOfType<AirshipControlBehaviour>();
			// Use the script to find the rigidbody
			StoreBodyInSlot(objs[0].GetComponent<Rigidbody>());
			StoreBodyInSlot(objs[1].GetComponent<Rigidbody>());
			StoreBodyInSlot(objs[2].GetComponent<Rigidbody>());
			StoreBodyInSlot(objs[3].GetComponent<Rigidbody>());
		}
		
		private void StoreBodyInSlot(Rigidbody a_body)
		{
			// Store in the slot
			if (a_body != null)
			{
				if (a_body.CompareTag("Player1_"))
				{
					m_playerRigidBodies[0] = a_body;
				}
				else if (a_body.CompareTag("Player2_"))
				{
					m_playerRigidBodies[1] = a_body;
				}
				else if (a_body.CompareTag("Player3_"))
				{
					m_playerRigidBodies[2] = a_body;
				}
				else if (a_body.CompareTag("Player4_"))
				{
					m_playerRigidBodies[3] = a_body;
				}
			}
		}


		public void Awake() 
		{
			myCollider = gameObject.GetComponent<BoxCollider> ();
			myCollider.isTrigger = true;

			GetPlayerRigidBodies ();
		}

		public void OnTriggerStay(Collider a_other)
		{
			Rigidbody playerBody = null;
			
			if (m_playerRigidBodies.Length > 0)
			{
				bool isPlayer = false;
				switch (a_other.tag)
				{
				case "Player1_":
					playerBody = m_playerRigidBodies[0];
					isPlayer = true;
					break;
				case "Player2_":
					playerBody = m_playerRigidBodies[1];
					isPlayer = true;
					break;
				case "Player3_":
					playerBody = m_playerRigidBodies[2];
					isPlayer = true;
					break;
				case "Player4_":
					playerBody = m_playerRigidBodies[3];
					isPlayer = true;
					break;
				case "Passengers":
					playerBody = a_other.GetComponentInParent<Rigidbody>();
					break;
				default:
					// Not player
					playerBody = a_other.GetComponent<Rigidbody>();
					return;
				}
				
				// Ensure that player has a rigidbody
				if (playerBody != null)
				{
					//This seems to be the best value for the time being- adjust this as necessary.
					playerBody.velocity *= 0.985f;
				}
			}
		}

	}
}
