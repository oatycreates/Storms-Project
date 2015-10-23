/**
 * File: SpawnPassengers.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson, Andrew Barbour
 * Created: 23/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Activates and deactivates passenger spawner. - Only use this if powerups are enabled
 **/


using UnityEngine;
using System.Collections;

namespace ProjectStorms
{


    public class SpawnDelayInterval : MonoBehaviour
    {
        public SpawnPassengers spawnPassengersScript;

        public float delayTimer = 10.0f;
        private float internalDelay = 0.0f;

        public float durationTimer = 5.0f;
        private float internalDuration = 1.0f;

        private bool waiting = true;

        void Start()
        {
            internalDelay = delayTimer;

            internalDuration = durationTimer;

            DisableSpawn();
        }

        void Update()
        {
             if (waiting)
             {
                 internalDelay -= Time.deltaTime;
             }
             else
             if (!waiting)
             {
                 internalDuration -= Time.deltaTime;
             }


            //turn off
            if (internalDuration < 0)
            {
                DisableSpawn();
            }

            //turn on
            if (internalDelay < 0)
            {
                EnableSpawn();
            }
        }

        void EnableSpawn()
        {
            spawnPassengersScript.currentlySpawning = true;

            internalDelay = delayTimer;
            internalDuration = durationTimer;
            waiting = false;

            print("Start spawn");
        }

        void DisableSpawn()
        {
            spawnPassengersScript.currentlySpawning = false;

            internalDelay = delayTimer;
            internalDuration = durationTimer;
            waiting = true;

            print("Start wait");
        }
    }
}
