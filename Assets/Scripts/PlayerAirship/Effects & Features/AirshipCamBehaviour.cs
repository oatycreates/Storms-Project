using UnityEngine;
using System.Collections;

public class AirshipCamBehaviour : MonoBehaviour //Basic lerp follow airship
{												// Don't change the cam height/width/ pixel position here - do that in the master cam controller
												 //However, we can tell the camera where to go and what positiont to take here.

	[HideInInspector]
	public bool camFollowPlayer = true;

	public GameObject camPosTarget;
	public GameObject camLookTarget;
	
	public float camSpeed = 5.0f;
	
	//Keep a reference to the start position, so we can reset to the Roulette Position
	private Vector3 myStartPos;
	private Quaternion myStartRot;

	void Start () 
	{
		gameObject.transform.parent = null; //detach from parent on start!
		
		myStartPos = gameObject.transform.position;
		myStartRot = gameObject.transform.rotation;
	}
	
	void Update () 
	{
		if (camFollowPlayer)
		{
			FollowCam();
		}
		else
		if (!camFollowPlayer)
		{
			WatchCam();
		}
	}
	
	public void FollowCam()
	{
		if (camPosTarget != null)
		{
			gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, camPosTarget.transform.rotation, Time.deltaTime * camSpeed);
		}
		
		if (camPosTarget != null)
		{
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, camPosTarget.transform.position, Time.deltaTime * camSpeed);
		}
	}
	
	
	public void WatchCam()
	{
				
		if (camLookTarget != null)
		{
			gameObject.transform.LookAt(camLookTarget.transform.position);
		}
	}
	
	public void RouletteCam()	//reset me
	{
		gameObject.transform.position = myStartPos;
		gameObject.transform.rotation = myStartRot;
	}
}
