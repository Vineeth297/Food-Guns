using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandController : HandGunController
{
	public List<GameObject> myAmmo;

	[SerializeField] private float ammoSize;
	
	[SerializeField] private GameObject leftMuzzle;
	
	[SerializeField] private float offsetOnY;

	private static int _totalAmmo;
	
	private void Start()
	{
		myAmmo = new List<GameObject>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnStartShooting(leftHandController,leftMuzzle);
		}
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids"))
		{
			print("Solids Triggered");
			print(gameObject);
			GameEvents.Ge.InvokeOnAmmoFound(leftHandController, other.gameObject, leftMagPos);
			OnAmmoFound(leftHandController, other.gameObject, leftMagPos);
			totalLeftCollectibles++;
			_totalAmmo++;
		}
	
		if (other.CompareTag("Liquids"))
		{
			print("Liquids Triggered");
			print(gameObject);
			GameEvents.Ge.InvokeOnAmmoFound(rightHandController, other.gameObject, rightMagPos);
			OnAmmoFound(rightHandController, other.gameObject, rightMagPos);
			totalRightCollectibles++;
		}
	}
}
