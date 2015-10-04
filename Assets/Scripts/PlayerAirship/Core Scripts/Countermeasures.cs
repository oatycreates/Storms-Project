/**
 * File: Countermeasures.cs
 * Author: RowanDonaldson
 * Maintainers: Patrick Ferguson
 * Created: 29/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Pool, activate and keep track of the different countermeasures/weapons.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
	{
	/// <summary>
	/// Pool, activate and keep track of the different countermeasures/weapons.
	/// </summary>
	public class Countermeasures : MonoBehaviour 
	{
		//Cached variables
		private Transform m_trans = null;
		private bool buttonDown = false;

		//CannonPort
		/*
		private GameObject port;
		*/

		public GameObject missilePrefab;
		public int pooledMissiles = 3;
		List<GameObject> missiles;

		public GameObject chaffPrefab;
		public int pooledChaff = 3;
		List<GameObject> chaff;

		public GameObject skyMinePrefab;
		public int pooledSkyMines = 3;
		List<GameObject> skyMines;

		public GameObject pinwheelPrefab;
		public int pooledPinwheels = 2;
		List<GameObject> pinwheels;

        /// <summary>
        /// For the homing missile's target.
        /// </summary>
        private TargetLock m_tarLock = null;

		void Awake()
		{
			m_trans = gameObject.transform;
            m_tarLock = gameObject.GetComponent<TargetLock>();
		}

		void Start() 
		{
            string myTag = gameObject.tag;

			// Missiles
			missiles = new List<GameObject> ();

			for (int i = 0; i < pooledMissiles; i++)
			{
				GameObject singleMissile = Instantiate(missilePrefab, m_trans.position, Quaternion.identity)as GameObject;
				singleMissile.SetActive(false);
                singleMissile.tag = myTag;
				missiles.Add(singleMissile);
			}


			// Chaff
			chaff = new List<GameObject> ();

			for (int i = 0; i < pooledChaff; i++)
			{
				GameObject singleChaff = Instantiate(chaffPrefab, m_trans.position, Quaternion.identity)as GameObject;
                singleChaff.SetActive(false);
                singleChaff.tag = myTag;
				chaff.Add(singleChaff);
			}


			// SkyMines
			skyMines = new List<GameObject> ();

			for (int i = 0; i < pooledSkyMines; i++)
			{
				GameObject singleMine = Instantiate(skyMinePrefab, m_trans.position, Quaternion.identity)as GameObject;
                singleMine.SetActive(false);
                singleMine.tag = myTag;
				skyMines.Add(singleMine);
			}

			// Pinwheels
			pinwheels = new List<GameObject> ();

			for (int i = 0; i < pooledPinwheels; i++)
			{
				GameObject singlePinwheel = Instantiate(pinwheelPrefab, m_trans.position, Quaternion.identity)as GameObject;
                singlePinwheel.SetActive(false);
                singlePinwheel.tag = myTag;
				pinwheels.Add(singlePinwheel);
			}
		}


		public void DPad(bool a_down, bool a_up, bool a_left, bool a_right)
		{
			if (!buttonDown)
			{
				//Dunno why these buttons don't map exactly to the right axis... ? Missile is UP, Chaff is DOWN, Mine is RIGHT, Pinwheel is LEFT
				if (a_left)
				{
					SpawnChaff ();
					buttonDown = true;
				}

				if (a_right)
				{
					SpawnMissile ();
					buttonDown = true;
				}

				if (a_up)
				{
					SpawnSkyMine ();
					buttonDown = true;
				}

				if (a_down) 
				{
					SpawnPinwheel();
					buttonDown = true;
				}
			}
	
			//Reset button
			if (!a_down && !a_up && !a_left && !a_right)
			{
				buttonDown = false;
			}
		}


		void SpawnMissile()
		{
			//Local offset
			Vector3 localOffset = new Vector3 (0, 0, 20);
			Vector3 worldOffest = m_trans.rotation * localOffset;
			Vector3 spawnPos = m_trans.position + worldOffest;

			//Loop and find the first non-active missile
			for (int i = 0; i < missiles.Count; i++)
			{
				if (!missiles[i].activeInHierarchy)
				{
					//missiles[i].transform.position = spawnOffset;
					//missiles[i].transform.position = port.transform.position;
					missiles[i].transform.position = spawnPos;
					missiles[i].transform.rotation = m_trans.rotation;
					missiles[i].SetActive(true);
                    if (m_tarLock != null)
                    {
                        MissileFlight missileScript = missiles[i].GetComponent<MissileFlight>();
                        missileScript.SetTarget(m_tarLock.GetTarget());
                    }

					//Don't forget to break loop
					break;
				}
			}
		}

		void SpawnChaff()
		{
			//Local offset
			Vector3 localOffset = new Vector3 (0, 5, -18);
			Vector3 worldOffest = m_trans.rotation * localOffset;
			Vector3 spawnPos = m_trans.position + worldOffest;

			//Loop and find the first non active Chaff
			for (int i = 0; i < chaff.Count; i++)
			{
				if (!chaff[i].activeInHierarchy)
				{
					chaff[i].transform.position = spawnPos;
					chaff[i].transform.rotation = m_trans.rotation;
					chaff[i].SetActive(true);

					//Dont forget t break loop
					break;
				}	
			}
		}

		void SpawnSkyMine()
		{
			//Local offset
			Vector3 localOffset = new Vector3 (8, 10, -25);
			Vector3 worldOffest = m_trans.rotation * localOffset;
			Vector3 spawnPos = m_trans.position + worldOffest;

			for (int i = 0; i < skyMines.Count; i++)
			{
				if (!skyMines[i].activeInHierarchy)
				{
					//skyMines[i].transform.position = m_trans.position;
					skyMines[i].transform.position = spawnPos;
					skyMines[i].transform.rotation = m_trans.rotation;
					skyMines[i].SetActive(true);

					break;
				}
			}
		}

		void SpawnPinwheel()
		{
			//Spawn offset
			Vector3 localOffset = new Vector3 (0, 0, 0);
			Vector3 worldOffset = m_trans.rotation * localOffset;
			Vector3 spawnPos = m_trans.position + worldOffset;

			for (int i = 0; i < pinwheels.Count; i++)
			{
				if (!pinwheels[i].activeInHierarchy)
				{
					pinwheels[i].transform.position = spawnPos;
					pinwheels[i].transform.rotation = m_trans.rotation;
					pinwheels[i].SetActive(true);
					//break the loop
					break;
				}
			}
		}

	}
}
