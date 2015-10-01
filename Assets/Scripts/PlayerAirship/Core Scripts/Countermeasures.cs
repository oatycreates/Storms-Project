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


		void Awake()
		{
			m_trans = gameObject.transform;
		}

		void Start() 
		{
			//Create a gameobject as a spawn point?
			/*
			port = new GameObject("CannonPort")as GameObject;
			port.transform.position = new Vector3 (m_trans.position.x, m_trans.position.y, m_trans.position.z+20);
			port.transform.parent = gameObject.transform;
			*/

			//Missiles
			missiles = new List<GameObject> ();

			for (int i = 0; i < pooledMissiles; i++)
			{
				GameObject singleMissile = Instantiate(missilePrefab, m_trans.position, Quaternion.identity)as GameObject;
				singleMissile.SetActive(false);
				missiles.Add(singleMissile);
			}


			//Chaff
			chaff = new List<GameObject> ();

			for (int i = 0; i < pooledChaff; i++)
			{
				GameObject singleChaff = Instantiate(chaffPrefab, m_trans.position, Quaternion.identity)as GameObject;
				singleChaff.SetActive(false);
				chaff.Add(singleChaff);
			}


			//SkyMines
			skyMines = new List<GameObject> ();

			for (int i = 0; i < pooledSkyMines; i++)
			{
				GameObject singleMine = Instantiate(skyMinePrefab, m_trans.position, Quaternion.identity)as GameObject;
				singleMine.SetActive(false);
				skyMines.Add(singleMine);
			}
		}


		public void DPad(bool a_down, bool a_up, bool a_left, bool a_right)
		{
			if (!buttonDown)
			{
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
					print ("Nothing yet");
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

	}
}
