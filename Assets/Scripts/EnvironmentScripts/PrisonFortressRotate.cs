using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class PrisonFortressRotate : MonoBehaviour 	//This script rotates the Prison Fortress ship in local space
{
	private Rigidbody myRigid;
	public float rotateForce = 10.0f;

	void Start () 
	{
		myRigid = gameObject.GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () 
	{
		myRigid.AddRelativeTorque(Vector3.up * rotateForce);
	}
}
