using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HandGunController : MonoBehaviour
{
	public List<GameObject> myAmmo;
	
	public HandGunController rightHandController,leftHandController;

	[SerializeField] private float ammoSize;
	[SerializeField] private GameObject rightHand;
	[SerializeField] private GameObject leftHand;
	public Animator handAnimator;
	[SerializeField] private GameObject leftMuzzle, rightMuzzle;
	[SerializeField] private GameObject leftGun, rightGun;
	[SerializeField] private float offsetOnY;
	public GameObject leftMagPos, rightMagPos;

	public int totalLeftCollectibles;
	public int totalRightCollectibles;

	private static int _totalAmmo;

	public bool isLeftHand;

	private RightGun _leftGunScript, _rightGunScript;

	private static readonly int HoldGunHash = Animator.StringToHash("ToHoldGun");
	private static readonly int ToShoot = Animator.StringToHash("ToShoot");

	public bool canShoot;

	private AimModeSwitch _aimModeSwitch;

	private bool isEnabled;

	[SerializeField] private ParticleSystem leftMuzzleParticle, rightMuzzleParticle;
	
	[SerializeField] private GameObject deActivator;

	[SerializeField] private GameObject loadingBurgerAmmo;
	[SerializeField] private GameObject loadingCokeAmmo;
	private Vector3 _burgerAmmoPos,_cokeAmmoPos;
	
	
	private Tweener _tweener;

	private float _fireSpeed = 0.3f;
	private float _nextFire = 0.0f;
	
	private void OnEnable()
	{
		GameEvents.Ge.aimModeSwitch += AdiosCollider;
	//	GameEvents.Ge.aimModeSwitch += SpawnGuns;
		
	}

	private void OnDisable()
	{
		GameEvents.Ge.aimModeSwitch -= AdiosCollider;
	//	GameEvents.Ge.aimModeSwitch -= SpawnGuns;
	}
	
	private void Start()
	{
		myAmmo = new List<GameObject>();
		
		_leftGunScript = leftGun.GetComponent<RightGun>();
		_rightGunScript = rightGun.GetComponent<RightGun>();
		
		handAnimator = GetComponent<Animator>();
		
		_aimModeSwitch = FindObjectOfType<AimModeSwitch>();

	}

	private void Update()
	{
		// if (_aimModeSwitch.canShoot)
		// {
		// 	if (Input.GetMouseButtonDown(0))
		// 	{
		// 		if(isLeftHand)
		// 			OnStartShooting(leftHandController,leftMuzzle);
		// 		else 
		// 			OnStartShooting(rightHandController,rightMuzzle);
		// 	}
		// }
		
		/*
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if(isLeftHand)
				OnStartShooting(leftHandController,leftMuzzle);
			else 
				OnStartShooting(rightHandController,rightMuzzle);
		}
		else */
		if (Input.GetKey(KeyCode.Space) && Time.time > _nextFire)
		{
			_nextFire = Time.time + _fireSpeed;
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
		}
	
		if (other.CompareTag("Liquids"))
		{
			print("Liquids Triggered");
			print(gameObject);
			GameEvents.Ge.InvokeOnAmmoFound(rightHandController, other.gameObject, rightMagPos);
			OnAmmoFound(rightHandController, other.gameObject, rightMagPos);
			totalRightCollectibles++;
			_totalAmmo++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("GunSpawner")) return;
		SpawnGuns();
		deActivator.SetActive(false);
	}


	public void OnAmmoFound(HandGunController handGunController, GameObject collectible, GameObject mag)
	{
		if (handGunController.myAmmo.Contains(collectible)) return;

		collectible.GetComponent<Collider>().enabled = false;
		DOVirtual.DelayedCall(0.5f, () => collectible.GetComponent<Collider>().enabled = true);
		
		var collectibleTransform = collectible.transform;
		var collectibleComponent = collectible.GetComponent<Collectible>();
		
		handGunController.myAmmo.Add(collectible);
		collectibleComponent.ammoFound = true;
		collectibleComponent.doSnake = true;
		//collectibleComponent.transform.rotation = Quaternion.Euler(Vector3.zero);

		if(collectibleTransform.CompareTag("Solids"))
			collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.leftAmmoIndex * offsetOnY);
		else
			collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.rightAmmoIndex * offsetOnY);

		collectibleComponent.SwingMag(handGunController, mag);

		//collectible.GetComponent<Collider>().enabled = true;

		//print("here");
	}

	public void OnStartShooting(HandGunController handGunController,GameObject muzzle)
	{
		//remove first ammo item from myammo list and move the bullets up
		//unparent the first ammo from player
		if (handGunController.myAmmo.Count == 0) return;
		
		StartCoroutine(Shoot(handGunController,muzzle,handAnimator));
	}

	IEnumerator Shoot(HandGunController handGunController, GameObject muzzle,Animator anim)
	{
		var bullet = handGunController.myAmmo[0];
		bullet.GetComponent<Collectible>().ammoToFollow = bullet.transform;

		handGunController.myAmmo.RemoveAt(0);

		bullet.SetActive(true);
		bullet.GetComponent<Collectible>().StartMoving(muzzle.transform.position);

		if (bullet.CompareTag("Solids"))
		{
			leftMuzzleParticle.Play();
			LoadingAmmoIllusion(loadingBurgerAmmo,_burgerAmmoPos);
		}
		else
		{
			rightMuzzleParticle.Play();
			LoadingAmmoIllusion(loadingCokeAmmo,_cokeAmmoPos);
		}
		
		anim.SetTrigger(ToShoot);
		
		yield return null;
		

		/*while (handGunController.myAmmo.Count != 0)
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
			
		}*/
	}
	
	

	private void AdiosCollider()
	{
		gameObject.GetComponent<Collider>().enabled = false;
		rightGun.transform.parent = rightHand.transform;
		leftGun.transform.parent = leftHand.transform;
		_burgerAmmoPos = loadingBurgerAmmo.transform.position;
		_cokeAmmoPos = loadingCokeAmmo.transform.position;
	}

	private void SpawnGuns()
	{
		//transform.parent.GetComponent<PlayerControl>().movementSpeed = 0f;
		rightGun.SetActive(true);
		leftGun.SetActive(true);

		handAnimator.SetTrigger(HoldGunHash);
		
		// for (int i = 0; i < myAmmo.Count; i++)
		// {
		// 	if (isLeftHand)
		// 	{
		// 		yield return new WaitForSeconds(0.1f);
		// 		leftHandController.myAmmo[i].SetActive(false);
		// 	}
		// 	else
		// 	{
		// 		yield return new WaitForSeconds(0.1f);
		// 		rightHandController.myAmmo[i].SetActive(false);
		// 	}
		// }
	}

	private void LoadingAmmoIllusion(GameObject illusionAmmo,Vector3 initPos)
	{

		if (_tweener.IsActive())
		{
			_tweener.Kill();
			illusionAmmo.transform.position = initPos;
		}
		_tweener = illusionAmmo.transform.DOMove(initPos + Vector3.forward * 0.5f, 1f).OnComplete(() =>
		{
			illusionAmmo.transform.position = initPos;
		});
	}
	
}
