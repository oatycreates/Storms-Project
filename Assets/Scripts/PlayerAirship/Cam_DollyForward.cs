using UnityEngine;
using System.Collections;
//This script makes the camers 'Dolly' forward or backwards in Local Space (translate forward or back). This is a quick fix to the 'Zoom behind ship issue'.
public class Cam_DollyForward : MonoBehaviour 
{
	//Get movement
	public AirshipControlBehaviour myController;
	
	private float forwardSpeed;

	private float myLocalZ;	
	
	public float camLerpSpeed = 25.0f;
	
	public float distanceOne = -25.0f;
	
	public float distanceTwo = - 10.0f;

	
	void Update () 
	{
		myLocalZ = gameObject.transform.localPosition.z;
	
		forwardSpeed = myController.throttle;
	
		if (forwardSpeed < 0)
		{
			SlideForward();
		}
		else
		if (forwardSpeed == 0)
		{
			ReturnToNormal();
		}
		else
		if (forwardSpeed > 0)
		{
			SlideBack();
		}
		
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, myLocalZ);
	}
	
	void SlideForward()
	{
		myLocalZ = Mathf.Lerp(myLocalZ, distanceOne, Time.deltaTime * camLerpSpeed);
	}
	
	void ReturnToNormal()
	{
		myLocalZ = Mathf.Lerp(myLocalZ, 0.0f, Time.deltaTime * camLerpSpeed);
	}
	
	void SlideBack()
	{
		myLocalZ = Mathf.Lerp(myLocalZ, distanceTwo, Time.deltaTime * camLerpSpeed);
	}
	
	
}
