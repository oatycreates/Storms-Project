using UnityEngine;
using System.Collections;
//This script makes the airship camera rotate to look at an invisible target, regardless of the camera's actual position.
public class CamLookDirection : MonoBehaviour 
{
	public GameObject lookTarget;
	
	private float distanceToTarget;
	private Ray myRay;

	
	void Update () 
	{
		gameObject.transform.LookAt(lookTarget.transform.position);
	
	
		DebugMe();
	}
	
	
	void DebugMe()
	{
		//Raycast
		distanceToTarget = Vector3.Distance(lookTarget.transform.position, gameObject.transform.position);
		
		Vector3 relativeForward = gameObject.transform.TransformDirection(Vector3.forward);
		
		myRay = new Ray(gameObject.transform.position, relativeForward);
		
		Debug.DrawRay(myRay.origin, myRay.direction * distanceToTarget, Color.green);
	}
}
