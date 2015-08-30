/**
 * File: RouletteSpinWheel.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script manages the 'Spinning Wheel' effect on the Roulette Wheel.
 *              Inputs are passed in from Input Manager script via the Roulette Behaviour State script.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script manages the 'Spinning Wheel' effect on the Roulette Wheel.
    /// Inputs are passed in from InputManager script via the Roulette Behaviour State script.
    /// </summary>
    public class RouletteSpinWheel : MonoBehaviour
    {
        /// <summary>
        /// The various buffers that the player can get on start.
        /// </summary>
        enum ERouletteBuffs
        {
            SPEED_BOOST,
            MEGA_BOOST,
            HEALTH_BOOST,
            DAMAGE_BOOST
        };

        /// <summary>
        /// How long to wait after the choice was picked before starting.
        /// </summary>
        public float rouletteEndWait = 0.5f;

        private float changeAngularDrag;

        public GameObject rotatorJoint;
        private bool pullHandle = false;
        private float rotateAmount;

        private bool inputStop;
        private bool inputSpeedUp;

        // For delaying the end of the roulette wheel state
        private float m_currEndWait;
        private bool m_rouletteDone = false;

        /// <summary>
        /// For making the roulette wheel's spin finish on a 90 degree boundary. 
        /// </summary>
        private Quaternion targetFinalRotation = Quaternion.identity;

        /// <summary>
        /// How fast to increment the roulette wheel's angular drag per second.
        /// </summary>
        private float m_rouletteSlowRate = 25.0f;

        // Cached variables
        private Rigidbody m_myRigid;
        private Transform m_trans;
        private Transform m_rotatorTrans;
        private StateManager m_parentStateManager = null;

        void Awake()
        {
            m_trans = transform;
            m_myRigid = GetComponent<Rigidbody>();
        }

        void Start()
        {
            m_rotatorTrans = rotatorJoint.transform;
        }

        void OnEnable()
        {
            if (m_parentStateManager == null)
            {
                m_parentStateManager = gameObject.transform.parent.transform.GetComponentInParent<StateManager>();
            }

            // Reset Position
            m_trans.localPosition = Vector3.zero;
            m_trans.localRotation = Quaternion.Euler(Vector3.zero);
            m_trans.localScale = new Vector3(4, 1, 1);	// Try not to change these

            // Set Components of the rigidbody at the start
            m_myRigid.isKinematic = false;
            m_myRigid.useGravity = false;

            m_myRigid.drag = 0;
            m_myRigid.angularDrag = 0.001f;	// Low on start

            m_myRigid.maxAngularVelocity = 14;

            targetFinalRotation = Quaternion.identity;

            // Get the start rotation of the handle joint
            rotateAmount = 0;

            // Reset variables
            m_currEndWait = rouletteEndWait;
            m_rouletteDone = false;

            // Make the wheel spin
            Spin();
        }

        void Update()
        {
            // For testing- make the roulette wheel spin faster
            if (inputSpeedUp)
            {
                Spin();
            }

            // Slow the roulette wheel by increasing the rigidbody drag
            if (inputStop)
            {
                m_myRigid.angularDrag = Mathf.Lerp(m_myRigid.angularDrag, 1, m_rouletteSlowRate * Time.deltaTime);

                pullHandle = true;
            }

            // Handle effect
            if (pullHandle)
            {
                rotateAmount = Mathf.Lerp(rotateAmount, -90, Time.deltaTime * 5.0f);
            }

            // Make handle return to original position
            if (m_rotatorTrans.localEulerAngles.x == 270)
            {
                pullHandle = false;
            }

            // Make handle return to original position
            if ((!pullHandle) && (!inputStop))
            {
                rotateAmount = Mathf.Lerp(rotateAmount, 0, Time.deltaTime * 10.0f);
            }

            // Lock the rotation of the handle to One Axis
            float rotationY = m_rotatorTrans.localEulerAngles.y;
            float rotationZ = m_rotatorTrans.localEulerAngles.z;
            // Move the joint
            m_rotatorTrans.localRotation = Quaternion.Euler(rotateAmount, rotationY, rotationZ);

            // Slerp into the final position if applicable
            if (targetFinalRotation != Quaternion.identity)
            {
                m_trans.localRotation = Quaternion.Slerp(m_trans.localRotation, targetFinalRotation, Time.deltaTime * 5.0f);
                // Stop physics spinning when lerping
                m_myRigid.angularVelocity = Vector3.zero;
            }

            //NOTE: Right now- this just triggers the State Manager to change the object from ROULETE to NORMAL CONTROL
            int selectedIndex = Mathf.RoundToInt(m_myRigid.rotation.eulerAngles.x / 90.0f);
            if (!m_rouletteDone && m_myRigid.angularVelocity.x > -1.0f && m_myRigid.angularVelocity.x < 1.0f)
            {
                int roundAngle = Mathf.RoundToInt(m_myRigid.rotation.eulerAngles.x % 90.0f);
                if (roundAngle >= 85 || roundAngle <= 5)
                {
                    // End the roulette state
                    targetFinalRotation = Quaternion.identity;
                    m_currEndWait = rouletteEndWait;
                    m_rouletteDone = true;
                }
                else
                {
                    // Not aligned to the final spin down position, slerp to it
                    targetFinalRotation = Quaternion.AngleAxis(selectedIndex * 90.0f, new Vector3(1, 0, 0));
                }
            }

            // For waiting before leaving the control state
            if (m_rouletteDone)
            {
                m_currEndWait -= Time.deltaTime;

                if (m_currEndWait <= 0)
                {
                    RouletteDone(selectedIndex);
                    m_rouletteDone = false;
                }
            }
        }

        void OnDisable()
        {
            m_myRigid.angularVelocity = Vector3.zero;
        }

        public void ChangeSpeed(bool a_slow, bool a_faster)
        {
            inputStop = a_slow;
            inputSpeedUp = a_faster;
        }

        void Spin()
        {
            m_myRigid.angularDrag = 0.001f;
            m_myRigid.AddRelativeTorque(Vector3.left * 100, ForceMode.Impulse);
        }

        /// <summary>
        /// Called when the roulette wheel finishes spinning.
        /// </summary>
        /// <param name="a_finalSelectionIndex">Index of the roulette wheel once the spin has finished.</param>
        void RouletteDone(int a_finalSelectionIndex)
        {
            // Apply roulette reward buff
            ERouletteBuffs currBuff = (ERouletteBuffs)a_finalSelectionIndex;

            ApplyBuff(currBuff);

            // Switch to the gameplay state
            m_parentStateManager.SetPlayerState(EPlayerState.Control);
        }

        /// <summary>
        /// Applies the input buff.
        /// </summary>
        /// <param name="a_buff">Buff to apply.</param>
        void ApplyBuff(ERouletteBuffs a_buff)
        {
            Debug.LogWarning("TODO Finish implementing the roulette buffs!");

            // Apply the input buff
            switch (a_buff)
            {
                case ERouletteBuffs.SPEED_BOOST:
                    {
                        Debug.Log("Speed boost!");
                        break;
                    }
                case ERouletteBuffs.MEGA_BOOST:
                    {
                        Debug.Log("Mega boost!");
                        break;
                    }
                case ERouletteBuffs.HEALTH_BOOST:
                    {
                        Debug.Log("Health boost!");
                        break;
                    }
                case ERouletteBuffs.DAMAGE_BOOST:
                    {
                        Debug.Log("Damage boost!");
                        break;
                    }
            }
        }
    } 
}
