using UnityEngine;
using System.Collections;
//This script takes input from the input manager, and passes the movement into an empty game object with an attached camera.
//Most of this script was derived from the Unity Example for transform.rotate
public class RotateCam : MonoBehaviour 
{
	private StateManager m_referenceStateManager;

	// The rotate cam is the centre GameObject - not the Camera itself.
	public GameObject rotateCam;
	
	public bool invertUpDown = false;
	public bool invertLeftRight = false;
	
	public float horizontalTiltAngle = 360.0f;
	public float verticalTiltAngle = 90.0f;
	public float smooth = 2.0f;
	public float deadZoneFactor = 0.25f;
	
	private float m_tiltAroundY;
	private float m_tiltAroundX;
	
	// Move the target object
	
	public GameObject lookyHereTarget;
	public float targetHeightFactor = 5.0f;
	private float yPos = 0;
	
	// Move the camera directly
	public GameObject camProxyTarget;
	private float m_xPos;
	public float camPositionFactor = 2.0f;
	private float m_zPos;
	public float camDistanceFactor = 15.0f;

	// Link to Cannons
	public GameObject[] cannons;
	
	// Fix cam to position
	public float camTurnMultiplier = 1.0f;
	public float totalVert = 0;
	public float totalHori = 0;

    /// <summary>
    /// Time to wait before beginning to interp the camera back when moving,
    /// </summary>
    public float movingCamResetTime = 1.5f;
    /// <summary>
    /// Lerp speed for resetting the camera
    /// </summary>
    public float camResetMoveSpeed = 3.0f;
    /// <summary>
    /// For resetting the camera when moving a few seconds after the last look.
    /// </summary>
    private float m_lastCamLookTime = 0;
	
    // Cached variables
    private Transform camRotTrans;
    private Transform lookTarTrans;
    private Transform camProxyTrans;

	void Start()
	{
        // Cache variables
		m_referenceStateManager = GetComponent<StateManager>();
        camRotTrans = rotateCam.transform;
        lookTarTrans = lookyHereTarget.transform;
        camProxyTrans = camProxyTarget.transform;
	}

	public void ResetCamRotation(bool a_snap)
	{
        if (a_snap)
        {
            totalHori = 0;
            totalVert = 0;
        }
        else
        {
            // Interp the look back to neutral
            totalHori = Mathf.Lerp(totalHori, 0, camResetMoveSpeed * Time.deltaTime);
            totalVert = Mathf.Lerp(totalVert, 0, camResetMoveSpeed * Time.deltaTime);

            // Snap the last leg
            if (Mathf.Abs(totalHori) < 0.01f)
            {
                totalHori = 0;
            }
            if (Mathf.Abs(totalVert) < 0.01f)
            {
                totalVert = 0;
            }
        }
	}
	
	void Update()
	{
		// Clamp the totalVert values
		totalVert = Mathf.Clamp(totalVert, -1.25f, 1.25f);
		totalHori = Mathf.Clamp(totalHori, -1.85f, 1.85f);
	}

	public void PlayerInputs(float a_camVertical, float a_camHorizontal, float a_triggerAxis, bool a_leftBumper, bool a_rightBumper, bool a_leftClick, bool a_rightClick)
    {
		// Reset on click
		if (a_leftClick || a_rightClick)
		{
			ResetCamRotation(true);
		}
		
		// Reset on accelerate only a few seconds after the last input
        m_lastCamLookTime -= Time.deltaTime;
        if (a_triggerAxis > 0 && m_lastCamLookTime < 0)
		{
            // Check for direct cam input first
            ResetCamRotation(false);
		}
	
		// Lock up/down
		if (a_camVertical > 0)
		{
			totalVert -= 0.01f * camTurnMultiplier;
		}	
		
		if (a_camVertical < 0)
		{
			totalVert += 0.01f * camTurnMultiplier;
		}
		
		// Lock left/right
		if (a_camHorizontal > 0)
		{
			totalHori += 0.01f * camTurnMultiplier;
		}
		
		if (a_camHorizontal < 0)
		{	
			totalHori -= 0.01f * camTurnMultiplier;
		}

        // Record last camera movement time for when to reset the looking
        if (!Mathf.Approximately(a_camHorizontal, 0) || !Mathf.Approximately(a_camVertical, 0))
        {
            m_lastCamLookTime = movingCamResetTime;
        }

		m_tiltAroundX = totalVert * verticalTiltAngle * deadZoneFactor;
		m_tiltAroundY = totalHori * verticalTiltAngle * deadZoneFactor;
		
		//tiltAroundY = camHorizontal * horizontalTiltAngle * deadZoneFactor;
		//tiltAroundX = camVertical * verticalTiltAngle * deadZoneFactor;
		
		if (invertUpDown)
		{
			m_tiltAroundX *= -1;
		}
		
		if (invertLeftRight)
		{
			
			m_tiltAroundY *= -1;
		}
		
		
		Quaternion target =  Quaternion.Euler(m_tiltAroundX, m_tiltAroundY, 0);

        EPlayerState currState = m_referenceStateManager.GetPlayerState();
        if (currState == EPlayerState.Control || currState == EPlayerState.Suicide)
		{
			camRotTrans.localRotation = Quaternion.Slerp(camRotTrans.localRotation, target, Time.deltaTime * smooth);
		}

		//Move lookTarget around.
		float internalCamYRotation = camRotTrans.localEulerAngles.y;
		//Debug.Log(internalCamYRotation);
	
		
		if (internalCamYRotation <= 315 && internalCamYRotation > 225)
		{
			//print ("Left");
			//Move the target
			yPos = Mathf.Lerp(yPos, targetHeightFactor, Time.deltaTime * smooth/2);
			
			//Move the cam
			m_xPos = Mathf.Lerp(m_xPos, camPositionFactor, Time.deltaTime * smooth/2);
			m_zPos = Mathf.Lerp(m_zPos, camDistanceFactor, Time.deltaTime * smooth/2);
			
			//Allow CannonFire
			if (a_leftBumper || a_rightBumper)
			{
				Cannons(ECannonPos.Port);
			}
			
		}
		else if (internalCamYRotation <= 135 && internalCamYRotation > 45)
		{
			//print ("Right");
			
			//Move the target
			yPos = Mathf.Lerp(yPos, targetHeightFactor, Time.deltaTime * smooth/2);
			
			
			//Move the cam
			m_xPos = Mathf.Lerp(m_xPos, -camPositionFactor, Time.deltaTime * smooth/2);
			m_zPos = Mathf.Lerp(m_zPos, camDistanceFactor, Time.deltaTime * smooth/2);
			
			//Allow CannonFire
			if (a_leftBumper || a_rightBumper)
			{
				Cannons(ECannonPos.Starboard);
			}
		
		}
		else if ( internalCamYRotation <= 225 && internalCamYRotation > 135)
		{
		 	//print ("Back");
			//Move the target
		 	yPos = Mathf.Lerp(yPos, 0, Time.deltaTime * smooth/2);
		 	
		 	
			//Move the cam
			m_xPos = Mathf.Lerp(m_xPos, 0, Time.deltaTime * smooth/2);
			m_zPos = Mathf.Lerp(m_zPos, 20, Time.deltaTime * smooth/2);
		}
		else
		{
			//print ("Forward");
			//Move the target
			yPos = Mathf.Lerp(yPos, 0, Time.deltaTime * smooth/2);
			
			
			//Move the cam
			m_xPos = Mathf.Lerp(m_xPos, 0, Time.deltaTime * smooth/2);
			m_zPos = Mathf.Lerp(m_zPos, 20, Time.deltaTime * smooth/2);
			
			//Allow CannonFire
			if (a_leftBumper || a_rightBumper)
			{
				Cannons(ECannonPos.Forward);
			}
			
		}
		
		lookTarTrans.localPosition = new Vector3(lookTarTrans.localPosition.x, yPos, lookTarTrans.localPosition.z);
		camProxyTrans.localPosition = new Vector3(m_xPos, camProxyTrans.localPosition.y, -m_zPos);
	}
	
	void Cannons(ECannonPos a_angle)
    {
        CannonFire script; 
	
        for (int i = 0; i < cannons.Length; i++)
        {
            script = cannons[i].GetComponent<CannonFire>();

            // Fire the cannons situated in the requested direction
            if (a_angle == script.cannon)
            {
                script.Fire();
            }
        }
	}
}
