using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ECannonPos
{
	Forward,
	Port,
	Starboard
}

//This script pools the cannonball prefab objects, and fires them when triggerd.
public class CannonFire : MonoBehaviour 
{
	public GameObject parentAirship;

	public ECannonPos cannon;
	
	public int pooledAmount = 1;
	
	List<GameObject> cannonBalls;
	
	public GameObject cannonBallPrefab;
	
	public float cannonBallForce = 50.0f;
	
	public GameObject lookAtTarget;
	
	private Vector3 relativeForward;

    // Cached variables
    private Rigidbody m_shipRB;

    void Awake()
    {
        m_shipRB = parentAirship.GetComponent<Rigidbody>();

        cannonBalls = new List<GameObject>();

        for (int i = 0; i < pooledAmount; i++)
        {
            // Pooled object details
            GameObject singleBall = Instantiate(cannonBallPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

            // Tag the cannonball
            singleBall.tag = parentAirship.tag;

            singleBall.SetActive(false);

            // Add the singleBall to the list
            cannonBalls.Add(singleBall);
        }
    }

	void Start() 
	{

	}
	
	void Update()
	{
		gameObject.transform.LookAt(lookAtTarget.transform.position);
		
		relativeForward = gameObject.transform.TransformDirection(Vector3.forward);
		
		Ray ray = new Ray(gameObject.transform.position, relativeForward);
		Debug.DrawRay(ray.origin, ray.direction * 5, Color.red);
	}
	
	
	public void Fire()
	{
		Vector3 relativeSpace;
		Rigidbody rigidBall;

		for (int i = 0; i < cannonBalls.Count; i++)
		{
			// Find only inactive cannonballs
			if (!cannonBalls[i].activeInHierarchy)
			{
				cannonBalls[i].transform.position = gameObject.transform.position;
				cannonBalls[i].transform.rotation = Quaternion.identity;
				
				cannonBalls[i].SetActive(true);
				
				relativeSpace = gameObject.transform.TransformDirection(Vector3.forward);
				
				rigidBall = cannonBalls[i].GetComponent<Rigidbody>();

                // Inherit the parent's velocity
                rigidBall.velocity = m_shipRB.velocity;

                // Fire off the cannonball
				rigidBall.AddRelativeForce(relativeSpace * cannonBallForce, ForceMode.Impulse);
				
				//Don't forget! Every once in a while, you deserve a...
				break;
			}
		}
	}
	
}
