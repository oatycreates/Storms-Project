/**
 * File: StallLightScript.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 20/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: A visual indicator on when the player can use the Stall feature.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// A visual indicator on when the player can use the Stall feature.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class StallLightScript : MonoBehaviour
    {
        public StateManager stateManager;

        private Light myLight;
        private float checkStateManager;

        void Start()
        {
            myLight = gameObject.GetComponent<Light>();
            myLight.color = Color.red;
        }

        void Update()
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
}
