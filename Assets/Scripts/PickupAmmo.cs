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
        void Awake()
        {

        }

        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            Countermeasures counter = other.gameObject.transform.root.gameObject.GetComponent<Countermeasures>() as Countermeasures;
            if (counter != null)
            {
                if (counter.gotPickup == false)
                {
                    counter.gotPickup = true;
                    print("Give ammo");
                }
            }
        }
    }
}
