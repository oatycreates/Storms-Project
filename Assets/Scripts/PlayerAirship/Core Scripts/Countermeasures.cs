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

        /// <summary>
        /// Time between each powerup.
        /// </summary>
        public float powerUpCooldown = 1.5f;
        /// <summary>
        /// Current powerup cooldown.
        /// </summary>
        private float m_currPowerupCooldown = 0.0f;

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

        public bool weaponsActive
        {
            get
            {
                return m_currPowerupCooldown <= 0.0f;
            }
        }

        /// <summary>
        /// For the homing missile's target.
        /// </summary>
        private TargetLock m_tarLock = null;

        private static GameObject ms_powerupHolder = null;

        //A public bool that determines whether or not I can fire a countermeasure
       // [HideInInspector]
        public bool gotPickup = true;

        //Call the AnnouncerText
        public UI_Controller announcerText;

		void Awake()
		{
			m_trans = gameObject.transform;
            m_tarLock = gameObject.GetComponent<TargetLock>();

            // Find the powerup holder object
            if (ms_powerupHolder == null)
            {
                ms_powerupHolder = GameObject.FindGameObjectWithTag("PowerupHolder");
                if (ms_powerupHolder == null)
                {
                    ms_powerupHolder = new GameObject();
                    ms_powerupHolder.name = "PowerupHolder";
                    ms_powerupHolder.tag = "PowerupHolder";
                }
            }


            //Find the announcer text
            announcerText = GameObject.FindObjectOfType<UI_Controller>();
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
                singleMissile.transform.parent = ms_powerupHolder.transform;
				missiles.Add(singleMissile);
			}


			// Chaff
			chaff = new List<GameObject> ();

			for (int i = 0; i < pooledChaff; i++)
			{
				GameObject singleChaff = Instantiate(chaffPrefab, m_trans.position, Quaternion.identity)as GameObject;
                singleChaff.SetActive(false);
                singleChaff.tag = myTag;
                singleChaff.transform.parent = ms_powerupHolder.transform;
				chaff.Add(singleChaff);
			}


			// SkyMines
			skyMines = new List<GameObject> ();

			for (int i = 0; i < pooledSkyMines; i++)
			{
				GameObject singleMine = Instantiate(skyMinePrefab, m_trans.position, Quaternion.identity)as GameObject;
                singleMine.SetActive(false);
                singleMine.tag = myTag;
                singleMine.transform.parent = ms_powerupHolder.transform;
				skyMines.Add(singleMine);
			}

			// Pinwheels
			pinwheels = new List<GameObject> ();

			for (int i = 0; i < pooledPinwheels; i++)
			{
				GameObject singlePinwheel = Instantiate(pinwheelPrefab, m_trans.position, Quaternion.identity)as GameObject;
                singlePinwheel.SetActive(false);
                singlePinwheel.tag = myTag;
                singlePinwheel.transform.parent = ms_powerupHolder.transform;
				pinwheels.Add(singlePinwheel);
			}

            // Start on cooldown
            m_currPowerupCooldown = powerUpCooldown;
		}

        void Update()
        {
            // Decrease powerup cooldown if waiting
            if (m_currPowerupCooldown > 0)
            {
                m_currPowerupCooldown -= Time.deltaTime;
            }
        }

		//public void DPad(bool a_down, bool a_up, bool a_left, bool a_right)
        public void FacePad(bool a_faceDown, bool a_faceUp, bool a_faceLeft, bool a_faceRight)
		{
			if (!buttonDown)
			{
                // Check to see if I can fire any countermeasures
                //if (gotPickup)
                {
                    if (m_currPowerupCooldown <= 0)
                    {
                        // Missile is UP, Chaff is DOWN, Mine is RIGHT, Pinwheel is LEFT
                        if (a_faceDown)//(a_down)
                        {
                            SpawnChaff();
                            buttonDown = true;
                            gotPickup = false;
                        }

                        if (a_faceUp) //(a_up)
                        {
                            SpawnMissile();
                            buttonDown = true;
                            gotPickup = false;
                        }

                        if (a_faceRight) //(a_right)
                        {
                            SpawnSkyMine();
                            buttonDown = true;
                            gotPickup = false;
                        }

                        if (a_faceLeft) //(a_left)
                        {
                            SpawnPinwheel();
                            buttonDown = true;
                            gotPickup = false;
                        }
                    }
                }

			}
	
			//Reset button
			//if (!a_down && !a_up && !a_left && !a_right)
            if (!a_faceDown && !a_faceUp && !a_faceLeft && !a_faceRight)
			{
				buttonDown = false;
			}
		}


		void SpawnMissile()
		{
			//Local offset
			Vector3 localOffset = new Vector3 (0, 0, 22);
			Vector3 worldOffest = m_trans.rotation * localOffset;
			Vector3 spawnPos = m_trans.position + worldOffest;

			//Loop and find the first non-active missile
			for (int i = 0; i < missiles.Count; i++)
			{
				if (!missiles[i].activeInHierarchy)
				{
                    // Rumble the controller
                    InputManager.SetControllerVibrate(gameObject.tag, 0.3f, 0.3f, 0.2f, false);

                    // Go on cooldown
                    m_currPowerupCooldown = powerUpCooldown;

                    //missiles[i].transform.position = spawnOffset;
					//missiles[i].transform.position = port.transform.position;
					missiles[i].transform.position = spawnPos;
					missiles[i].transform.rotation = m_trans.rotation;
					missiles[i].SetActive(true);
                    if (m_tarLock != null)
                    {
                        MissileFlight missileScript = missiles[i].GetComponent<MissileFlight>();
                        missileScript.SetTarget(m_tarLock.GetTarget(), m_tarLock.maximumTarDist);
                        
                        //Send the Return to sender value
                        missileScript.WhoShotMe(gameObject);


                        //Trigger the announcer text
                        string faction = "NONAME";
                        string targetTag = null;

                        Transform warnTarget = m_tarLock.GetTarget();

                        //Check the target Airship's faction

						if (warnTarget != null)
						{
	                        if (warnTarget.gameObject.GetComponent<FactionIndentifier>() != null)
	                        {
	                            faction = warnTarget.gameObject.GetComponent<FactionIndentifier>().factionName;
	                            targetTag = warnTarget.gameObject.GetComponent<FactionIndentifier>().gameObject.tag;
	
	                            announcerText.LockOn(faction, targetTag);
	                        }
                        }
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
                    // Rumble the controller
                    InputManager.SetControllerVibrate(gameObject.tag, 0.3f, 0.3f, 0.2f, false);

                    // Go on cooldown
                    m_currPowerupCooldown = powerUpCooldown;

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
			Vector3 localOffset = new Vector3 (0, 10, -45);
			Vector3 worldOffest = m_trans.rotation * localOffset;
			Vector3 spawnPos = m_trans.position + worldOffest;

			for (int i = 0; i < skyMines.Count; i++)
			{
				if (!skyMines[i].activeInHierarchy)
				{
                    // Rumble the controller
                    InputManager.SetControllerVibrate(gameObject.tag, 0.3f, 0.3f, 0.2f, false);

                    // Go on cooldown
                    m_currPowerupCooldown = powerUpCooldown;

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
			Vector3 localOffset = new Vector3 (-10, 7.5f, 0);
			Vector3 worldOffset = m_trans.rotation * localOffset;
			Vector3 spawnPos = m_trans.position + worldOffset;

			for (int i = 0; i < pinwheels.Count; i++)
			{
				if (!pinwheels[i].activeInHierarchy)
				{
                    // Rumble the controller
                    InputManager.SetControllerVibrate(gameObject.tag, 0.3f, 0.3f, 0.2f, false);

                    // Go on cooldown
                    m_currPowerupCooldown = powerUpCooldown;

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
