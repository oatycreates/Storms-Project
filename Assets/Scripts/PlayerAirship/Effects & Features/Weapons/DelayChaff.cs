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
		public float delayFactor = 0.985f;

		public void Awake() 
		{
			myCollider = gameObject.GetComponent<BoxCollider>();
			myCollider.isTrigger = true;
		}

		public void OnTriggerStay(Collider a_other)
		{
            Rigidbody rigid = a_other.GetComponent<Rigidbody>();

            if (rigid == null)
            {
                rigid = a_other.GetComponentInParent<Rigidbody>();
            }

            // Ensure that player has a rigidbody
            if (rigid != null)
            {
                //This seems to be the best value for the time being- adjust this as necessary.
                //rigid.velocity *= 0.985f;
                rigid.velocity *= (delayFactor/2);
                rigid.angularVelocity *= delayFactor; //-- Too much?
            }
		}
	}
}
