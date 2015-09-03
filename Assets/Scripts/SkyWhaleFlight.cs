/**
 * File: ScoreManager.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 03/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Determines how the Skywhale Flys through the game world, using ridigbody physics
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{

	[RequireComponent(typeof(Rigidbody))]
	public class SkyWhaleFlight : MonoBehaviour 
	{
		private Rigidbody myRigid;
		public GameObject whaleTarget;

		public Vector3 eulerAngularVelocity;

		void Awake () 
		{
			myRigid = gameObject.GetComponent<Rigidbody> ();
		}

		void FixedUpdate () 
		{
			//myRigid.MovePosition (myRigid.transform.position + transform.forward * Time.deltaTime * 20);


			//myRigid.AddRelativeForce (Vector3.forward * 5);

			//myRigid.AddRelativeTorque (Vector3.forward * 1);

			Quaternion deltaRotation = Quaternion.Euler (eulerAngularVelocity * Time.fixedDeltaTime);

			myRigid.MoveRotation (myRigid.rotation * deltaRotation);
		}
	}
}
