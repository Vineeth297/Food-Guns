using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGunController : MonoBehaviour
{
	public List<GameObject> myAmmo;

	public HandGunController rightHandGun, leftHandGun;
	
	[SerializeField] private float ammoSize;
	[SerializeField] private GameObject rightHand;
	[SerializeField] private GameObject leftHand;
	[SerializeField] private GameObject leftMuzzle, rightMuzzle;
	
	[SerializeField] private float offsetOnY;
	public GameObject leftMagPos, rightMagPos;

	[HideInInspector] public List<Vector3> leftPositions; 
	
	[HideInInspector] public List<Vector3> rightPositions;

	/*private void OnEnable()
	{
		GameEvents.Ge.onAmmoFound += OnAmmoFound;
	}

	private void OnDisable()
	{
		GameEvents.Ge.onAmmoFound -= OnAmmoFound;
	}*/
	
	private void Start()
	{
		myAmmo = new List<GameObject>();
		rightHandGun = rightHand.GetComponent<HandGunController>();
		leftHandGun = leftHand.GetComponent<HandGunController>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnStartShooting(leftHandGun,leftMuzzle);
			OnStartShooting(rightHandGun,rightMuzzle);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids"))
		{
			//GameEvents.Ge.InvokeOnAmmoFound(leftHandGun, other.gameObject, leftMagPos);
			OnAmmoFound(leftHandGun, other.gameObject, leftMagPos);
		}
	
		if (other.CompareTag("Liquids"))
		{
			//GameEvents.Ge.InvokeOnAmmoFound(rightHandGun, other.gameObject, rightMagPos);
			OnAmmoFound(rightHandGun, other.gameObject, rightMagPos);
		}
	}

	private void OnAmmoFound(HandGunController handGunController, GameObject collectible, GameObject mag)
	{
		collectible.GetComponent<Collider>().enabled = false;
		
		var collectibleTransform = collectible.transform;
		var collectibleComponent = collectible.GetComponent<Collectible>();
		
		var lastAmmoYPos = 0f;
		if(handGunController.myAmmo.Count > 0)
			lastAmmoYPos = handGunController.myAmmo[^1].transform.localPosition.z;

		handGunController.myAmmo.Add(collectible);
		collectibleComponent.ammoFound = true;
		collectibleComponent.transform.rotation = Quaternion.Euler(Vector3.zero);

		if(collectibleTransform.CompareTag("Solids"))
			collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.leftAmmoIndex * offsetOnY);
		else
			collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.rightAmmoIndex * offsetOnY);

		// collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.ammoIndex * offsetOnY);

		if(collectible.CompareTag("Solids"))
			collectibleTransform.localScale = Vector3.one * ammoSize;

		if (collectible.CompareTag("Liquids"))
		{
			collectible.transform.rotation = Quaternion.Euler(-90f,0f,0f);
		}
		/*
		if (handGunController.myAmmo.Count == 1)
			// collectibleTransform.localPosition = new Vector3(0f, 0f, extendedMagPositionOnZ);								// static positioning
			collectibleTransform.DOLocalMove(new Vector3(0f, 0f, extendedMagPositionOnZ),0.15f);
		else
		{
			// var yPos = lastAmmoYPos - handGunController.myAmmo[^1].GetComponent<Renderer>().bounds.size.y + offsetOnY ;
			var zPos = lastAmmoYPos - handGunController.myAmmo[^1].GetComponent<Renderer>().bounds.size.z + offsetOnY ;
			// collectibleTransform.localPosition = new Vector3(0f,yPos , extendedMagPositionOnZ);								// static positioning
			// collectibleTransform.DOLocalMove(new Vector3(0f, yPos, extendedMagPositionOnZ),0.15f);
			collectibleTransform.DOLocalMove(new Vector3(0f, 0f, zPos),0.15f);
			print(zPos);
		}*/
		collectibleComponent.SwingMag(handGunController, mag);

	}

	private void OnStartShooting(HandGunController handGunController,GameObject muzzle)
	{
		//remove first ammo item from myammo list and move the bullets up
		//unparent the first ammo from player
		if (handGunController.myAmmo.Count == 0) return;
		
		StartCoroutine(Shoot(handGunController,muzzle));
		
	}

	IEnumerator Shoot(HandGunController handGunController, GameObject muzzle)
	{
		while (handGunController.myAmmo.Count != 0)
		{
			GameObject bullet = handGunController.myAmmo[0];
			handGunController.myAmmo.RemoveAt(0);
			
			bullet.GetComponent<Collectible>().StartMoving(muzzle.transform.position);

			for (int i = 1; i < handGunController.myAmmo.Count; i++)
			{
				var ammoComponent = handGunController.myAmmo[i].GetComponent<Collectible>();
				if(ammoComponent.CompareTag("Solids"))
					ammoComponent.leftAmmoIndex -= 1;
				else
					ammoComponent.rightAmmoIndex -= 1;
			}
			
			yield return new WaitForSeconds(0.15f);
		}
	}
	
}
