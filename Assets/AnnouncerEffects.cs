/**
 * File: Controls and keeps track of all the announcer text - children gameobject.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 15/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Create, set text and keep track of text objects.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
    public class AnnouncerEffects : MonoBehaviour
    {
        public GameObject textPrefab;
        private Vector3 randomPos;

        void Start()
        {
            InvokeRepeating("SpawnText", 1, 1);
        }

        void Update()
        {

        }

        void SpawnText()
        {
            RandomPos();

            Instantiate(textPrefab, gameObject.transform.position + randomPos, Quaternion.identity);
        }

        void RandomPos()
        {
            randomPos = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);

            print(randomPos);
        }
    }

}