/**
 * File: MineBehaviours.cs
 * Author: RowanDonaldson
 * Maintainers: Patrick Ferguson
 * Created: 1/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Mine behaviour - spawns 3 explosion prefabs on collision with player ship.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
	[RequireComponent(typeof(SphereCollider))]
	[RequireComponent(typeof(Renderer))]
	/// <summary>
	/// On collision with airship, trigger three waves of explosives.
	/// </summary>
	public class MineBehaviour : MonoBehaviour 
	{
		private Renderer m_myRenderer;
		private SphereCollider m_myCollider;

		public GameObject explosionPrefab;
		public int numberOfExplosions;
		private int numberExplosionStartReference;
		List<GameObject> explosions;

		private float delayTimer = 0.1f;
		private float explosionScale = 0.0f;
		//private float explosionScaleStartReference;

		private bool bang = false;

		//Scale mine on activation
		private float scaleFactor = 1;
		public float maxScaleSize = 25;
		public float scaleSpeed = 1.0f;
		
		private WeaponSFX sfx;
		private AudioSource m_Audio;
		//private float startVolume;
		public AudioClip extraSound;

		void Awake() 
		{
			m_myRenderer = gameObject.GetComponent<Renderer> ();
			m_myCollider = gameObject.GetComponent<SphereCollider> ();


			numberExplosionStartReference = numberOfExplosions;
			//Take a reference of this number.
			//explosionScaleStartReference = explosionScale;

			if (gameObject.GetComponent<WeaponSFX>() != null)
			{
				sfx = gameObject.GetComponent<WeaponSFX>();
				m_Audio = gameObject.GetComponent<AudioSource>();
				//startVolume = m_Audio.volume;
			}
		}

		void Start()
		{
			//Pool the explosions
			explosions = new List<GameObject> ();

			for (int i = 0; i < numberOfExplosions; i++)
			{
				GameObject singleExplosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
				singleExplosion.SetActive(false);
				explosions.Add(singleExplosion);
			}
		}

		void OnEnable()
		{
			//Turn everything on when I start;
			m_myCollider.enabled = true;
			m_myRenderer.enabled = true;

			//numberOfExplosions = numberExplosionStartReference;

			//Reset the explosion values every time the mine is activated.s
			delayTimer = 0.1f;
			//explosionScale = explosionScaleStartReference;

			//Start the mine small
			scaleFactor = 0.01f;
			gameObject.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);

			//Reset the number of explosions
			numberOfExplosions = numberExplosionStartReference;
			
			//Take a reference of audio level
			
		}
		
		void OnDisable()
		{
			bang = false;
		}

		void Update () 
		{
			//The scale behaviour
			scaleFactor = Mathf.Clamp (scaleFactor, 0, maxScaleSize);

			if (scaleFactor < maxScaleSize)
			{
				scaleFactor += scaleSpeed * Time.deltaTime;
			}

			gameObject.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);


			//should I be exploding?
		 	if (bang == true)
			{
				//Cap the number of explosion waves.
				if (numberOfExplosions > 0)
				{

					delayTimer -= Time.deltaTime;

					// This funciton makes the explosion get bigger with every wave;
					if (delayTimer < 0)
					{
						explosionScale += 20;
						SpawnExplosion(explosionScale);
						//delayTimer += 0.75f;
						delayTimer = 0.25f;
						numberOfExplosions -= 1;
					}
				}
				else
				if (numberOfExplosions <= 0)
				{
					//Remember to turn Mine object off - but let the explosions kill themselves.
					Invoke("KillMine", 1.5f);
				}
			}
			
			//AUdio stuff
			if (bang == false)
			{
				if (!m_Audio.isPlaying)
				{
					if (extraSound != null)
					{
						sfx.SetSound(extraSound, true, false);
						
						//The beeping noise is very loud - lower volume here, and reset it on collision (under Spawn Explosion)
						m_Audio.volume = 0.1f;
						m_Audio.pitch = 0.25f;
						
					}
				}
			}
		}

		void OnCollisionEnter(Collision other)
		{
			//Make the mine 'dissapear', but hang around to keep track of the explosion objects.
			m_myCollider.enabled = false;
			m_myRenderer.enabled = false;

			bang = true;
			//Kill sounds
			m_Audio.Stop();
		}

		void SpawnExplosion(float explosionScale)
		{
			/*
			if (explosionPrefab != null)
			{
				GameObject explosion = Instantiate (explosionPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
				explosion.GetComponent<ExplosionTrigger> ().maxSize = explosionScale;
			}*/

			//Loop to find inactive mines
			for (int i = 0; i < explosions.Count; i++)
			{
				if (!explosions[i].activeInHierarchy)
				{
					explosions[i].transform.position = gameObject.transform.position;
					explosions[i].transform.rotation = gameObject.transform.rotation;
					explosions[i].GetComponent<ExplosionTrigger>().maxSize = explosionScale;
					explosions[i].SetActive(true);

					//Don't forget to break the loop
					break;
				}
			}
			
			//Reset Audio volume -- nah do this in the WeaponSFX script
			//m_Audio.volume = startVolume;
		}

		void KillMine()
		{
			//Go back to dormant state
			gameObject.SetActive (false);
		}
	}
}
