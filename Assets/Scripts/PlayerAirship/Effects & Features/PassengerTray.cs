/**
 * File: PassengerTray.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Attempts to keep the passengers in the ship by passing any forces applied to the ship onto the passengers.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
    /// <summary>
    /// Attempts to keep the passengers in the ship by passing any forces applied to the ship onto the passengers.
    /// </summary>
    public class PassengerTray : MonoBehaviour
    {
        /// <summary>
        /// List of  game object tags to try to keep in the tray.
        /// </summary>
        public string[] trayPassengerTags = { "Passengers" };

        /// <summary>
        /// Mass to add to the ship per prisoner in the body.
        /// </summary>
        public float prisonerMassAdd = 0.1f;

        /// <summary>
        /// Mass to add to the ship as a result of part mass changes.
        /// </summary>
        public float shipPartMassAdd = 0.0f;

        /// <summary>
        /// How long to keep the tray powered down for.
        /// </summary>
        public float trayPowerDownTime = 0.2f;

        /// <summary>
        /// How hard to explode the ship's contents away.
        /// </summary>
        public float explosionForce = 10.0f;

        /// <summary>
        /// How big to make the ship's explosion.
        /// </summary>
        public float explosionRadius = 3.0f;

        /// <summary>
        /// Where to centre the explosion from, this should be a transform relative to the ship prefab.
        /// </summary>
        public Transform explosionCentreTrans;

        /// <summary>
        /// Radius to horizontally suck prisoners in from.
        /// </summary>
        public float horizVacuumRadius = 50.0f;

        /// <summary>
        /// Force to suck prisoners in from.
        /// </summary>
        public float horizVacuumForce = 0.1f;

        /// <summary>
        /// Radius to force the passengers to match vacuum.
        /// </summary>
        public float superVacuumRadius = 3.0f;

        /// <summary>
        /// Percentage of the player's velocity to match each second when in the super vacuum radius.
        /// </summary>
        public float superVacuumStrength = 1.0f;

        /// <summary>
        /// Velocity of the ship last tick.
        /// </summary>
        private Vector3 m_lastShipVel = Vector3.zero;

        /// <summary>
        /// Angular velocity of the ship last tick.
        /// </summary>
        private Vector3 m_lastShipAngVel = Vector3.zero;

        /// <summary>
        /// Set to true when the players ship actually starts moving.
        /// </summary>
        private bool m_hasStarted = false;

        /// <summary>
        /// Start mass of the ship. 
        /// </summary>
        private float m_shipStartMass = 0;

        /// <summary>
        /// List of objects in the tray that match the passenger tag type.
        /// </summary>
        private List<GameObject> m_trayContents = new List<GameObject>();
        public List<GameObject> trayContents
        {
            get
            {
                return m_trayContents;
            }
        }

        /// <summary>
        /// Time until the tray powers back up.
        /// </summary>
        private float m_trayPowerDownCooldown = 0.0f;

        /// <summary>
        /// Whether the tray has been powered down.
        /// </summary>
        private bool m_trayIsPoweredDown = false;

        // Cached variables
        private Rigidbody m_shipRb;
        private Transform m_trans;

        /// <summary>
        /// The current amount of passengers within the tray
        /// trigger volume
        /// </summary>
        public int passengerCount
        {
            get
            {
                return m_trayContents.Count;
            }
        }
        
        
        /// <summary>
        /// Get reference UI_Controller Script
        /// </summary>
        private GameObject scoreTextController;
        private UI_Controller scoreMangager;
        private string factionName;
		private int noOfPassengers = 0;
		private string airshipTag;
		
		//A particle system for visual feedback
		public ParticleSystem m_system;
		private string parentFaction;

        void Awake()
        {
            // Zero variables
            m_lastShipVel = Vector3.zero;
            shipPartMassAdd = 0.0f;

            // Cache variables
            m_shipRb = gameObject.GetComponentInParent<Rigidbody>();
            m_trans = gameObject.transform;
            m_shipStartMass = 0.0f;
            
            //Find score manager
            scoreTextController = GameObject.FindWithTag("UIScoreManager");
            if (scoreTextController != null)
            {
            	scoreMangager = scoreTextController.GetComponent<UI_Controller>()as UI_Controller;
            }
            else
            {
            	Debug.Log("No score manager in this scene.");
            }
            
            
            //Particle stuff
            m_system.Stop();
            
			parentFaction = gameObject.GetComponentInParent<FactionIndentifier>().factionName;
			
			if (parentFaction == "PIRATES")
			{
				m_system.startColor = Color.red;
			}
			else
			if (parentFaction == "NAVY")
			{
				m_system.startColor = Color.blue;
			}
			else
			if (parentFaction == "TINKERERS")
			{
				m_system.startColor = Color.green;
			}
			else
			if (parentFaction == "VIKINGS")
			{
				m_system.startColor = Color.yellow;
			}
			else
			if (parentFaction == "NONE")
			{
				m_system.startColor = Color.white;
			}
           
        }

        /// <summary>
        /// Use this for initialisation.
        /// </summary>
        void Start()
        {
            if (explosionCentreTrans == null)
            {
                Debug.LogError("The explosion centre transform is not set!");
            }
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        void Update()
        {
        	//Check for changes in the number of passengers in the tray.
        	ContactScoreManager();
        	
        	noOfPassengers = m_trayContents.Count;
        
        
        //Tray stuff
            if (m_trayIsPoweredDown)
            {
                if (m_trayPowerDownCooldown <= 0.0f)
                {
                    enabled = true;
                    m_trayIsPoweredDown = false;
                }
                else
                {
                    m_trayPowerDownCooldown -= Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// Called once per physics tick.
        /// </summary>
        void FixedUpdate()
        {
            if (!m_trayIsPoweredDown)
            {
                Vector3 currShipVel = m_shipRb.velocity;

                // Only start applying velocity forces when the player starts moving, this avoids passengers in the first tick being launched
                if (!m_hasStarted && currShipVel.magnitude > 0)
                {
                    m_hasStarted = true;
                }

                if (m_shipStartMass == 0)
                {
                    m_shipStartMass = m_shipRb.mass;
                }
                else
                {
                    //Debug.Log(m_shipRb.mass + " " + m_shipStartMass);

                    // Set the mass
                    m_shipRb.mass = m_shipStartMass + shipPartMassAdd + m_trayContents.Count * prisonerMassAdd;
                }

                if (m_hasStarted)
                {
                    // Calculate ship velocity over the past tick. a = (v - u) / t
                    //m_currShipAccel = (m_shipRb.velocity - m_lastShipVel) / Time.deltaTime;

                    // Store ship velocity for the next tick
                    m_lastShipVel = m_shipRb.velocity;
                    m_lastShipAngVel = m_shipRb.angularVelocity;
                }

                // Suck in nearby prisoners
                GameObject[] prisoners = GameObject.FindGameObjectsWithTag("Passengers");
                Transform tempTrans = null;
                Vector3 offsetVec = Vector3.zero;
                Rigidbody tempRb = null;
                foreach (GameObject prisoner in prisoners)
                {
                    tempTrans = prisoner.transform;
                    offsetVec = m_trans.position - tempTrans.position;

                    // If in range
                    if (offsetVec.magnitude <= horizVacuumRadius && offsetVec.y < 0)
                    {
                        // Make force horizontal
                        offsetVec.y = 0;
                        offsetVec.Normalize();

                        // Apply force
                        tempRb = prisoner.GetComponent<Rigidbody>();
                        tempRb.AddForce(offsetVec * horizVacuumForce, ForceMode.Force);

                        // Stick above player if close enough
                        if (offsetVec.magnitude <= superVacuumRadius)
                        {
                            // Match player velocity
                            tempRb.velocity = Vector3.Lerp(tempRb.velocity, m_shipRb.velocity, superVacuumStrength * Time.deltaTime);
                        }
                    }
                }
            }

            m_trayContents.Clear();
        }

        /// <summary>
        /// Called each physics tick that other objects are colliding with this trigger.
        /// </summary>
        /// <param name="a_other"></param>
        void OnTriggerStay(Collider a_other)
        {
            if (!m_trayIsPoweredDown)
            {
                if (IsTrayObject(a_other.tag))
                {
                    // Apply the cumulative ship force for the tick to this object
                    Rigidbody rb = a_other.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // Add force
                        //rb.AddForce(m_currShipAccel, ForceMode.Acceleration);

                        // Stick the passenger in the ship tray
                        rb.velocity = m_lastShipVel;
                        rb.angularVelocity = m_lastShipAngVel;
                        rb.AddForce(Physics.gravity, ForceMode.VelocityChange);

                        // Cumulate mass
                        m_trayContents.Add(rb.gameObject);
                    }

                    PassengerDestroyScript pass = a_other.GetComponent<PassengerDestroyScript>();
                    if (pass != null)
                    {
                        pass.ResetExpireTimer();
                    }
                }
            }
        }
        
        /// <summary>
        /// Applies a force to all objects in this tray.
        /// </summary>
        /// <param name="a_force">Force vector.</param>
        /// <param name="a_forceType">Unity force type.</param>
        public void ApplyTrayForce(Vector3 a_force, ForceMode a_forceType)
        {
            if (!m_trayIsPoweredDown)
            {
                Rigidbody rb;
                foreach (GameObject go in m_trayContents)
                {
                    rb = go.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // First match ship velocity
                        rb.velocity = m_shipRb.velocity;

                        // Then apply the force
                        rb.AddForce(a_force, a_forceType);
                    }
                }
            }
        }

        /// <summary>
        /// Causes the tray to explode its contents outwards.
        /// </summary>
        public void ExplodeTray()
        {
            // Explode the ship tray
            foreach (GameObject go in m_trayContents)
            {
                go.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionCentreTrans.position, explosionRadius);
               // print("Rowan! It works!!!");
            }
        }

        /// <summary>
        /// Powers down the tray for a short duration, will make the contents fly out.
        /// </summary>
        public void PowerDownTray()
        {
            m_trayPowerDownCooldown = trayPowerDownTime;
            m_trayIsPoweredDown = true;
        }

        /// <summary>
        /// Returns whether the input tag is for an object that should be kept in the tray.
        /// </summary>
        /// <param name="a_otherTag">Tag of the other game object.</param>
        /// <returns>True if it is a tray object, false if not.</returns>
        private bool IsTrayObject(string a_otherTag)
        {
            bool outIsTrayObj = false;

            for (uint i = 0; i < trayPassengerTags.Length; ++i)
            {
                if (trayPassengerTags[i].CompareTo(a_otherTag) == 0)
                {
                    outIsTrayObj = true;
                    break;
                }
            }

            return outIsTrayObj;
        }


		void ContactScoreManager()
		{
			//Check faction
			factionName = gameObject.GetComponentInParent<FactionIndentifier>().factionName;
			airshipTag = gameObject.transform.root.gameObject.tag;
			
			//print (m_trayContents.Count); //Yay - this works
			//Check tray contents against the previous number of passengers.
			
			if (noOfPassengers != m_trayContents.Count)
			{
				//check to see if there are more or less passengers in the tray
				if (m_trayContents.Count > noOfPassengers)
				{
					scoreMangager.PassengersInTray(factionName, noOfPassengers, true, airshipTag);
					
				
					
					if (!m_system.isPlaying)
					{
						m_system.Play();
					}
				}
				else
				if (m_trayContents.Count < noOfPassengers)
				{
					scoreMangager.PassengersInTray(factionName, noOfPassengers, false, airshipTag);
				}
				
			}
			
		}
    } 
}
