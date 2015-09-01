/**
 * File: AirshipControlBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Allows the player to control their airship via the InputManager class.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
    /// <summary>
    /// This is the basic airship control. It manages pitch, yaw, and roll. Syncs up to ship animations. In time this script will probably be changed a lot.
    /// Many of the movement features here were expanded upon and derived from the Unity Standard Vehicle Assets - Plane Controller.
    /// Unlike the old Storms Project, this game moves Non-Kinematic Rigidbodies through Physics (unlike Kinematic Rigidbody via direct control).
    /// </summary>
    public class AirshipControlBehaviour : MonoBehaviour
    {
        /// <summary>
        /// A collection object to contain the impact that losing/damaging a part has on the ship.
        /// </summary>
        [System.Serializable]
        public class ShipPartInputConnection
        {
            /// <summary>
            /// What the part is, affects how the ship's handling will be hampered.
            /// </summary>
            public ShipPartDestroy.EShipPartType partType = ShipPartDestroy.EShipPartType.INVALID;
            /// <summary>
            /// Primary multiplier for the control value this part alters.
            /// E.g. Losing the balloons hampers roll amounts.
            /// </summary>
            public float partValueMult = 1.0f;
            /// <summary>
            /// Secondary multiplier for any auxiliary control value this part alters.
            /// E.g. Losing the balloons also causes this amount of constant input in that given direction.
            /// </summary>
            public float partAuxValueMult = 1.0f;
        }

        /// <summary>
        /// Change the mass of the Airship in editor here
        /// </summary>
        public float adjustableMass = 100.0f;

        /// <summary>
        /// Movement speed of the airship.
        /// Try 50 here and see how we go.
        /// </summary>
        public float generalSpeed = 200.0f;

        /// <summary>
        /// Speed multiplier for the reverse throttle value.
        /// </summary>
        public float reverseSpeedMult = 0.5f;

        /// <summary>
        /// How much pitching the vehicle will affect its facing.
        /// </summary>
        public float pitchForce = 1000.0f;
        /// <summary>
        /// How much yawing the vehicle will affect its facing.
        /// </summary>
        public float yawForce = 1000.0f;
        /// <summary>
        /// How much rolling the vehicle will affect its facing.
        /// </summary>
        public float rollForce = 1000.0f;

        /// <summary>
        /// Multiplier for the pitch auto-level force.
        /// </summary>
        public float pitchLimitMult = 0.7f;

        /// <summary>
        /// Multiplier for the pitch auto-level force.
        /// </summary>
        public float rollLimitMult = 0.8f;

        /// <summary>
        /// Percentage of the base propeller animation speed to apply when moving.
        /// </summary>
        public float animThrottleMult = 2.5f;

        /// <summary>
        /// Percentage of the base propeller animation speed to apply when braking.
        /// </summary>
        public float animLowThrottleMult = 0.1f;

        /// <summary>
        /// Handle to the airship camera script.
        /// </summary>
        public AirshipCamBehaviour airshipMainCam;

        /// <summary>
        /// Bounce the input variables to the Audio Controller script under 'Particles and Effects' hierarchy branch.
        /// </summary>
        public AirshipAudio audioControl;
        public EngineAudio engineAudioControl;

        /// <summary>
        /// Reference to rudder joint script, for inversing rudder rotation while reversing.
        /// </summary>
        public HingeJointScript rudderJoint;

        /// <summary>
        /// If the player ship goes above this, they will stall.
        /// </summary>
        public float stallY = 1000.0f;

        /// <summary>
        /// If the player ship goes below this, they will die.
        /// </summary>
        public float killY = -2000.0f;

        /// <summary>
        /// Multiplier values for when various ship parts get destroyed.
        /// </summary>
        public ShipPartInputConnection[] shipPartConns;

        /// <summary>
        /// Cached mass for the ship at the start of the game.
        /// </summary>
        private float m_startShipMass = 0;

        // Animation trigger hashes
        private int m_animHatchOpen = Animator.StringToHash("HatchOpen");
        private int m_animTrapdoorOpen = Animator.StringToHash("TrapdoorOpen");
        private int m_animPropellerMult = Animator.StringToHash("PropellerMult");

        // Cached variables
        private Rigidbody m_myRigid = null;            // Rigidbody of player
        private Transform m_trans = null;         // Transform for above rigidbody
        private Animator m_anim = null;
        private ShipPartDestroy m_shipPartDestroy = null;
        private StateManager m_shipStates = null;
        private AirshipStallingBehaviour m_shipStallScript = null;

        public bool isReversing
        {
            get
            {
                return m_isReversing;
            }
        }
        private bool m_isReversing = false;

        [HideInInspector]
        public float roll;
        [HideInInspector]
        public float pitch;
        [HideInInspector]
        public float yaw;
        [HideInInspector]
        public float throttle;
        [HideInInspector]
        public bool openHatch;
        void Awake()
        {
            // Get Rigidbody variables
            m_myRigid = GetComponent<Rigidbody>();
            m_trans = transform;

            m_myRigid.mass = adjustableMass;
            m_startShipMass = adjustableMass;

            m_anim = GetComponent<Animator>();
            m_shipPartDestroy = GetComponent<ShipPartDestroy>();
            m_shipStallScript = GetComponent<AirshipStallingBehaviour>();
            m_shipStates = GetComponent<StateManager>();
        }

        void Start()
        {
            // Zero out the rigid body's velocities on start
            m_myRigid.velocity = Vector3.zero;
            m_myRigid.angularVelocity = Vector3.zero;
        }

        void Update()
        {
            // Set cam stuff
            airshipMainCam.camFollowPlayer = true;

            // Set rigidbody basics
            m_myRigid.useGravity = false;
            m_myRigid.isKinematic = false;	// We want physics collisions!

            //m_myRigid.mass = 10.0f;
            m_myRigid.drag = 2.0f;
            m_myRigid.angularDrag = 2.0f;

            TestVerticalLimits();
        }

        private void TestVerticalLimits()
        {
            // Stall above stallY if moving up
            if (m_trans.position.y > stallY && m_myRigid.velocity.y > 0)
            {
                m_shipStates.SetPlayerState(EPlayerState.Stalling);
                if (m_shipStallScript != null)
                {
                    m_shipStallScript.SetAboveStallY(stallY);
                }
            }

            // Kill below killY if moving down
            if (m_trans.position.y < killY && m_myRigid.velocity.y < 0)
            {
                m_shipStates.SetPlayerState(EPlayerState.Dying);
            }
        }

        public void PlayerInputs(
            float a_Vertical,
            float a_Horizontal,
            float a_camVertical,
            float a_camHorizontal,
            float a_triggers,
            bool a_bumperLeft,
            bool a_bumperRight,
            bool a_faceUp,      // Y - Open hatch
            bool a_faceDown,    // A - Fire cannon forwards
            bool a_faceLeft,    // X - Fire broadside left
            bool a_faceRight)   // B - Fire broadside right
        {
            if (this.isActiveAndEnabled)
            {
                // Use this to convert buttonpresses to axis input;
                float rollFloat = 0;

                if (a_faceLeft)
                {
                    rollFloat = -1;
                }
                else if (a_faceRight)
                {
                    rollFloat = 1;
                }

                //roll = 0.25f * a_Horizontal + a_camHorizontal;
                roll = 0.25f * a_Horizontal + rollFloat;
                pitch = a_Vertical;
                throttle = a_triggers;

                // Reverse yaw if play is moving backwards
                if (a_triggers < 0)
                {
                    // Ensure player is actually moving backwards
                    if (Vector3.Dot(m_myRigid.velocity, m_trans.forward) < 0)
                    {
                        m_isReversing = true;
                        yaw = a_Horizontal;
                    }
                }
                else
                {
                    // Player is moving forwards, or reverse trigger not held
                    yaw = a_Horizontal;
                    m_isReversing = false;
                }

                // Check buttonPresses
                openHatch = a_faceUp;//(a_faceUp || a_faceDown);

                // Keep the inputs in reasonable ranges, see the standard asset examples for more
                ClampInputs();

                /*
                // Open/close the hatches
                m_anim.SetBool(m_animHatchOpen, a_faceUp);
                m_anim.SetBool(m_animTrapdoorOpen, a_faceUp);
                */

                // Spin the propeller
                float animThrottle = throttle * 2.0f; // [-1, 1] to [-2, 2], 50% now maps to 100% anim throttle
                float animThrottleSign = animThrottle >= 0 ? 1 : -1;
                animThrottle = Mathf.Abs(animThrottle);
                if (animThrottle > 1)
                {
                    // Scale up to the animation throttle multiplier
                    float boundedThrottle = animThrottle * 0.5f + 0.5f;
                    animThrottle = boundedThrottle * (animThrottleMult - 1) + 1;
                }
                m_anim.SetFloat(m_animPropellerMult, animThrottleSign * animThrottle);
            }
        }

        void FixedUpdate()
        {
            // Doing this here makes it easier on the physics
            ConstantForwardMovement();
            CalculateTorque();
            CalculateRightingForce();

            //Pass values to AudioController
            if (audioControl != null)
            {
                audioControl.AudioInputs(pitch, yaw, roll);
            }

            if (engineAudioControl != null)
            {
                engineAudioControl.AudioInput(throttle);
            }
        }

        /// <summary>
        /// Clamps the input values into the [-1, 1] range.
        /// </summary>
        private void ClampInputs()
        {
            roll = Mathf.Clamp(roll, -1, 1);
            pitch = Mathf.Clamp(pitch, -1, 1);
            yaw = Mathf.Clamp(yaw, -1, 1);
            throttle = Mathf.Clamp(throttle, -1, 1);
        }

        /// <summary>
        /// Moves the player's ship forward with either their constant movement speed or their inputted throttle.
        /// </summary>
        private void ConstantForwardMovement()
        {
            // 				general speed 	half or double general speed	+ 25% of general speed == Always a positive value
            float speedMod = (throttle * generalSpeed) /*+ (generalSpeed/4)*/;

            if (throttle < 0 && m_myRigid.velocity.sqrMagnitude < 0)
            {
                // Slow down when reversing
                speedMod *= reverseSpeedMult;
            }

            // Slow down when laden
            speedMod *= CalcHandlingMassMult();

            // Slow down when mast is gone
            float tempNotUsed = 0.0f;
            float mastSpeedMult = 0.0f;
            GetPartInputMults(ShipPartDestroy.EShipPartType.MAST, out mastSpeedMult, out tempNotUsed);
            speedMod *= (1.0f - mastSpeedMult);

            m_myRigid.AddRelativeForce(Vector3.forward * speedMod, ForceMode.Acceleration);

            // This finds the 'up' vector. It was a cool trick from The Standard Vehicle Assets
            var liftDirection = Vector3.Cross(m_myRigid.velocity, m_myRigid.transform.right).normalized;

            m_myRigid.AddForce(liftDirection);
        }

        /// <summary>
        /// Calculates the multiplier for ship handling values (throttle, roll, pitch, yaw, etc.).
        /// </summary>
        /// <returns>Throttle multiplier, apply that to each quantity.</returns>
        private float CalcHandlingMassMult()
        {
            return m_startShipMass / m_myRigid.mass;
        }

        /// <summary>
        /// Calculates the rotation forces on the ship, see the standard assets example for more.
        /// </summary>
        private void CalculateTorque()
        {
            var torque = Vector3.zero;

            // Handle worse when laden
            float handleMod = 1.0f; // CalcHandlingMassMult();

            // Reverse 
            float reverseMult = 1;
            if (throttle < 0 && m_myRigid.velocity.sqrMagnitude < 0)
            {
                reverseMult = -1;
            }

            // Part damage modifiers
            float tempNotUsed = 0.0f;
            float leftBallRollMult = 0.0f, leftBallPopVal = 0.0f,
                rightBallRollMult = 0.0f, rightBallPopVal = 0.0f;
            bool leftBallDest = GetPartInputMults(ShipPartDestroy.EShipPartType.LEFT_BALLOON, out leftBallRollMult, out leftBallPopVal);
            bool rightBallDest = GetPartInputMults(ShipPartDestroy.EShipPartType.RIGHT_BALLOON, out rightBallRollMult, out rightBallPopVal);

            float rudderYawMult = 0.0f;
            GetPartInputMults(ShipPartDestroy.EShipPartType.RUDDER, out rudderYawMult, out tempNotUsed);

            // Roll as if the only balloon left is pulling up
            if (leftBallDest && !rightBallDest)
            {
                // Make the player still able to roll all the way
                roll *= 1.5f;

                // Right balloon pulling up, left popped
                roll -= leftBallPopVal;
            }
            else if (!leftBallDest && rightBallDest)
            {
                // Make the player still able to roll all the way
                roll *= 1.5f;

                // Left balloon pulling up, right popped
                roll += rightBallPopVal;
            }

            torque += handleMod * -pitch * reverseMult * m_myRigid.transform.right * pitchForce;
            torque += handleMod * yaw * (1.0f - rudderYawMult) * reverseMult * m_myRigid.transform.up * yawForce;
            torque += handleMod * -roll * (1.0f - leftBallRollMult - rightBallRollMult) * m_myRigid.transform.forward * rollForce;

            // Add all the torque forces together
            m_myRigid.AddTorque(torque);
        }

        /// <summary>
        /// Attempts to auto-level the player's ship. Counters roll more-so than pitch, ignores yaw.
        /// </summary>
        private void CalculateRightingForce()
        {
            // Calculate a few useful vectors relative to the ship and the world
            Vector3 worldUp = new Vector3(0, 1, 0); // Up relative to the world
            Vector3 shipForward = transform.forward; // Front relative to the ship
            Vector3 shipRight = transform.right; // Right relative to the ship

            var torque = Vector3.zero;

            // Roll
            float rollDot = Vector3.Dot(worldUp, shipRight);
            torque += -rollDot * shipForward * rollForce * rollLimitMult;

            // Pitch
            float pitchDot = Vector3.Dot(-worldUp, shipForward);
            torque += -pitchDot * shipRight * pitchForce * pitchLimitMult;

            // Add all the torque forces together
            m_myRigid.AddTorque(torque);
        }

        /// <summary>
        /// Returns the part input multipliers for the input ship part type.
        /// </summary>
        /// <param name="a_partType">Part type to find.</param>
        /// <param name="ao_primMult">Primary input multiplier. E.g. Balloon affecting roll.</param>
        /// <param name="ao_auxMult">Auxiliary input multiplier. E.g. Balloon causing constant roll input.</param>
        /// <returns>True if the part has been destroyed, false if not.</returns>
        private bool GetPartInputMults(ShipPartDestroy.EShipPartType a_partType, out float ao_primMult, out float ao_auxMult)
        {
            ao_primMult = 0;
            ao_auxMult = 0;
            if (m_shipPartDestroy.IsPartTypeDestroyed(a_partType))
            {
                foreach (ShipPartInputConnection part in shipPartConns)
                {
                    // Part of the same type?
                    if (part.partType == a_partType)
                    {
                        ao_primMult = part.partValueMult;
                        ao_auxMult = part.partAuxValueMult;
                        return true;
                    }
                }
            }
            return false;
        }
    } 
}
