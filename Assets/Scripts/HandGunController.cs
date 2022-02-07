using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class HandGunController : MonoBehaviour
{
	public List<GameObject> myAmmo;

	public HandGunController rightHandController,leftHandController;

	[SerializeField] private float ammoSize;
	[SerializeField] private GameObject rightHand;
	[SerializeField] private GameObject leftHand;
	[SerializeField] private GameObject leftMuzzle, rightMuzzle;
	[SerializeField] private GameObject leftGun, rightGun;
	[SerializeField] private float offsetOnY;
	public GameObject leftMagPos, rightMagPos;

	public int totalLeftCollectibles;
	public int totalRightCollectibles;

	private static int _totalAmmo;

	public bool isLeftHand;

	private RightGun _leftGunScript, _rightGunScript;

	private void Start()
	{
		myAmmo = new List<GameObject>();
		
		rightHandController = rightHand.GetComponent<HandGunController>();
		leftHandController = leftHand.GetComponent<HandGunController>();
		_leftGunScript = leftGun.GetComponent<RightGun>();
		_rightGunScript = rightGun.GetComponent<RightGun>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if(isLeftHand)
				OnStartShooting(leftHandController,leftMuzzle);
			else 
				OnStartShooting(rightHandController,rightMuzzle);
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
			//leftGun.transform.DOShakeScale(1f, 1f);
			_leftGunScript.PickUpReaction();
		}
	
		if (other.CompareTag("Liquids"))
		{
			print("Liquids Triggered");
			print(gameObject);
			GameEvents.Ge.InvokeOnAmmoFound(rightHandController, other.gameObject, rightMagPos);
			OnAmmoFound(rightHandController, other.gameObject, rightMagPos);
			totalRightCollectibles++;
			_totalAmmo++;
			//rightGun.transform.DOShakeScale(1f, 1f);
			// GameEvents.Ge.InvokePickUpRightReaction();
			_rightGunScript.PickUpReaction();

		}
	}

	public void OnAmmoFound(HandGunController handGunController, GameObject collectible, GameObject mag)
	{
		collectible.GetComponent<Collider>().enabled = false;
		//DOVirtual.DelayedCall(0.3f, () => collectible.GetComponent<Collider>().enabled = true);
		
		var collectibleTransform = collectible.transform;
		var collectibleComponent = collectible.GetComponent<Collectible>();

		handGunController.myAmmo.Add(collectible);
		collectibleComponent.ammoFound = true;
		collectibleComponent.transform.rotation = Quaternion.Euler(Vector3.zero);

		if(collectibleTransform.CompareTag("Solids"))
			collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.leftAmmoIndex * offsetOnY);
		else
			collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.rightAmmoIndex * offsetOnY);

		//if(collectible.CompareTag("Solids"))
		//collectibleTransform.localScale = Vector3.one * handGunController.ammoSize;

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

		collectible.GetComponent<Collider>().enabled = true;

		//print("here");
	}

	public void OnStartShooting(HandGunController handGunController,GameObject muzzle)
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

			_totalAmmo--;
			
			if(bullet.CompareTag("Solids"))
				totalLeftCollectibles--;
			else
				totalRightCollectibles--;
			
			//print(_totalAmmo);
			yield return new WaitForSeconds(0.15f);
		}
	}
}
