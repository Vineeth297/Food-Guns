using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Collectible : MonoBehaviour
{
	public bool ammoFound;
	public bool startShooting;
	
	private void Update()
	{
		if(!ammoFound)
			transform.Rotate(45f * Time.deltaTime, 45f * Time.deltaTime, 45f * Time.deltaTime);
		
		if(startShooting)
			Shoot();
	}

	private void Shoot()
	{
		transform.Translate(transform.forward,Space.World);
	}
	
}
