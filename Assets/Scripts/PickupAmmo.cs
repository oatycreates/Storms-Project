/**
 * File: PickupAmmo.cs
 * Author: RowanDonaldson
 * Maintainers: Patrick Ferguson
 * Created: 14/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Gives the player ammo when they fly through pickup trigger.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public class PickupAmmo : MonoBehaviour
    {
        private SphereCollider myCollider;

        void Awake()
        {
            myCollider = gameObject.GetComponent<SphereCollider>();
        }

        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.transform.root.gameObject.GetComponent<Countermeasures>() as Countermeasures != null)
            {
               if (other.gameObject.transform.root.gameObject.GetComponent<Countermeasures>().gotPickup == false)
               {
                   other.gameObject.transform.root.gameObject.GetComponent<Countermeasures>().gotPickup = true;
                   print("Give ammo");
               }
            }
        }
    }
}
