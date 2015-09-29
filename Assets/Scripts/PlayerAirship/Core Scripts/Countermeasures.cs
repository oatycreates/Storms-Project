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

		public GameObject missilePrefab;
		public int pooledMissiles = 3;
		List<GameObject> missiles;
		/*
		public GameObject chaffPrefab;
		public int pooledChaff = 3;
		List<GameObject> chaff;

		//Not ready for these yet

		public GameObject skyMinePrefab;
		public int pooledSkyMines = 3;
		List<GameObject> skyMines;
		*/

		void Awake()
		{
			m_trans = gameObject.transform;
		}

		void Start() 
		{

			//Missiles
			missiles = new List<GameObject> ();

			for (int i = 0; i < pooledMissiles; i++)
			{
				GameObject singleMissile = Instantiate(missilePrefab, m_trans.position, Quaternion.identity)as GameObject;
				singleMissile.SetActive(false);
				missiles.Add(singleMissile);
			}

			/*
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
			*/
		}

		void Update () 
		{
		
		}

		public void DPad(bool a_down, bool a_up, bool a_left, bool a_right)
		{
			if (!buttonDown)
			{
				if (a_down)
				{
					SpawnChaff ();
					buttonDown = true;
				}

				if (a_up)
				{
					SpawnMissile ();
					buttonDown = true;
				}

				if (a_left)
				{
					SpawnSkyMine ();
					buttonDown = true;
				}

				if (a_right) 
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
			Vector3 spawnOffset = new Vector3 (8, 10, 0);

			//Loop and find the first non-active missile
			for (int i = 0; i < missiles.Count; i++)
			{
				if (!missiles[i].activeInHierarchy)
				{
					missiles[i].transform.position = m_trans.position + spawnOffset;
					//missiles[i].transform.position = new Vector3(0,0,0);
					missiles[i].transform.rotation = Quaternion.identity;
					missiles[i].SetActive(true);


					//Don't forget to break loop
					break;
				}
			}
		}

		void SpawnChaff()
		{
			print ("Chaff");
		}

		void SpawnSkyMine()
		{
			print ("SkyMine");
		}

	}
}
