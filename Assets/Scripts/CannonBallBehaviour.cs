using UnityEngine;
using System.Collections;
//This has all the behaviour for the cannonballs.
//[RequireComponent(typeof(AudioSource))]
//[RequireComponent(typeof(SphereCollider))]
public class CannonBallBehaviour : MonoBehaviour 
{
	private float timer;

	void OnEnable()
	{
		timer = 5.0f;
	}
	
	void Update()
	{	
		timer -= Time.deltaTime;
		
		if (timer < 0)
		{
			gameObject.SetActive(false);
		}
	}	

	/*
	private AudioSource mySource;
	public AudioClip explosionNoise;

	private float timeOut = 5.0f;
	private float rememberTimeOut;
	
	private SphereCollider myCollider;
	
	void Awake()
	{
		mySource = gameObject.GetComponent<AudioSource>();
		myCollider = gameObject.GetComponent<SphereCollider>();
		
		mySource.clip = explosionNoise;
	}
	
	void Start()
	{
		rememberTimeOut = timeOut;
		
	}
	

	void OnEnable () 
	{
		timeOut = rememberTimeOut;	
		
		myCollider.isTrigger = true;
		
		// Turn the trigger back on once the cannonball has spawned.
		Invoke("TriggerOff", 1.0f);
	}
	
	
	void Update () 
	{
		timeOut -= Time.deltaTime;
		
		if (timeOut < 0)
		{
			gameObject.SetActive(false);
		}
		
		// Or if the cannonball has somehow fallen too far
		if (gameObject.transform.position.y < -2000)
		{
			gameObject.SetActive(false);
		}
	}
	
	void TriggerOff()
	{
		myCollider.isTrigger = false;
	}
	
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag != gameObject.tag)
		{	
			//Sound Stuff!!!
			if (explosionNoise != null)
			{
				mySource.Play();
			}
			else
			{
				Debug.Log("No sound on Cannonball!");
			}
			
			//Leave an impact
			if (other.gameObject.GetComponent<Rigidbody>() != null)
			{
				other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(5, gameObject.transform.position, 5.0f);
			}
			
			
			//Wipe me out!
			myCollider.isTrigger = true;
			gameObject.SetActive(false);
			
		}
	}
	*/
}
