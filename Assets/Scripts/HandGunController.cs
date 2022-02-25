using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HandGunController : MonoBehaviour
{
	public List<GameObject> myAmmo;
	private SoundManager _sound;
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

	public int myCollectibles;

	private static int _totalAmmo;

	public bool isLeftHand;

	private RightGun _rightGunScript;
	private LeftGun _leftGunScript;

	private static readonly int HoldGunHash = Animator.StringToHash("ToHoldGun");
	private static readonly int ToShoot = Animator.StringToHash("ToShoot");

	public bool canShoot;
	public bool arrangementForObstacleAimModeSwitch;

	private AimModeSwitch _aimModeSwitch;

	private bool isEnabled;

	[SerializeField] private ParticleSystem leftMuzzleParticle, rightMuzzleParticle;
	
	[SerializeField] private GameObject deActivator;

	[SerializeField] private GameObject loadingBurgerAmmo;
	[SerializeField] private GameObject loadingCokeAmmo;
	private Vector3 _burgerAmmoPos,_cokeAmmoPos;
	
	
	private Tweener _tweener;
	private Tweener _moveTweener;
	private float _fireSpeed = 0.3f;
	private float _nextFire = 0.0f;
	
	[SerializeField] private TMP_Text comboText;
	[SerializeField] private ComboText comboComponent;
	
	private float _initFontSize;

	public CameraFollowVideoLevel cameraFollowVideoLevel;

	public int shootCount;
	private Collider _collider;

	private List<Tweener> _moveTweeners;
	private List<Tweener> _gulpingTweens;
	private void OnEnable()
	{
		GameEvents.Ge.aimModeSwitch += AdiosCollider;
		GameEvents.Ge.obstacleHeadAimSwitch += AdiosCollider;
		GameEvents.Ge.startShooting += StartShooting;
		GameEvents.Ge.stopShooting += StopShooting;
	}

	private void OnDisable()
	{
		GameEvents.Ge.aimModeSwitch -= AdiosCollider;
		GameEvents.Ge.obstacleHeadAimSwitch -= AdiosCollider;
		GameEvents.Ge.startShooting -= StartShooting;
		GameEvents.Ge.stopShooting -= StopShooting;
	}

	private void Start()
	{
		myAmmo = new List<GameObject>();
		
		_leftGunScript = leftGun.GetComponent<LeftGun>();
		_rightGunScript = rightGun.GetComponent<RightGun>();
		
		handAnimator = GetComponent<Animator>();
		
		_aimModeSwitch = FindObjectOfType<AimModeSwitch>();
		
		comboText.enabled = false;
		_initFontSize = comboText.fontSize;

		_sound = SoundManager.Singleton;

		_collider = GetComponent<Collider>();
		
		_moveTweeners = new List<Tweener>();
		_gulpingTweens = new List<Tweener>();
	}

	private void Update()
	{
		if(canShoot)
		{
			if (Input.GetMouseButton(0) && Time.time > _nextFire)
			{
				_nextFire = Time.time + _fireSpeed;

				_sound.PlaySound(_sound.shootSound);
				
				if (isLeftHand)
				{
					OnStartShooting(leftHandController,leftMuzzle);
					if(GameManager.Singleton.myLeftCollectibles >= 1)
						GameManager.Singleton.myLeftCollectibles--;
				}
				else
				{
					OnStartShooting(rightHandController,rightMuzzle);
					if(GameManager.Singleton.myRightCollectibles >= 1)
						GameManager.Singleton.myRightCollectibles--;
				}
			}
		}
		
		if(shootCount > 0)
		{
			if (Time.time > _nextFire)
			{
				_nextFire = Time.time + _fireSpeed;

				_sound.PlaySound(_sound.shootSound);
				
				if (isLeftHand)
				{
					OnStartShooting(leftHandController,leftMuzzle);
					if(GameManager.Singleton.myLeftCollectibles >= 1)
						GameManager.Singleton.myLeftCollectibles--;
				}
				else
				{
					OnStartShooting(rightHandController,rightMuzzle);
					if(GameManager.Singleton.myRightCollectibles >= 1)
						GameManager.Singleton.myRightCollectibles--;
				}

				shootCount--;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out Collectible collectible))
			if(collectible.isPickedUp) return;
		
		if (other.CompareTag("Solids") && leftHandController.myAmmo.Count >= 0)
		{
			collectible.isPickedUp = true;
			leftMuzzleParticle.Play();
			GameEvents.Ge.InvokeOnAmmoFound(leftHandController, other.gameObject, leftMagPos);
			OnAmmoFound(leftHandController, other.gameObject, leftMagPos);
			leftHandController.comboComponent.ComboSelling(comboText, _initFontSize);
			GameManager.Singleton.myLeftCollectibles++;
			//cameraFollowVideoLevel.distance += 0.75f;
			DOTween.To(() => cameraFollowVideoLevel.distance, value => cameraFollowVideoLevel.distance = value,
				cameraFollowVideoLevel.distance + 0.75f, 0.4f);
			//cameraFollowVideoLevel.height += 0.1f;
			DOTween.To(() => cameraFollowVideoLevel.height, value => cameraFollowVideoLevel.height = value,
				cameraFollowVideoLevel.height + 0.1f, 0.4f);
			
			_sound.PlaySound(_sound.pickupSound);

			_leftGunScript.PickUpReaction();
			StartCoroutine(GulpTheAmmo(leftHandController));
			KillAllGuplingTweens();

		}
	
		if (other.CompareTag("Liquids") && rightHandController.myAmmo.Count >= 0)
		{
			collectible.isPickedUp = true;
			rightMuzzleParticle.Play();
			GameEvents.Ge.InvokeOnAmmoFound(rightHandController, other.gameObject, rightMagPos);
			OnAmmoFound(rightHandController, other.gameObject, rightMagPos);
			rightHandController.comboComponent.ComboSelling(comboText, _initFontSize);
			GameManager.Singleton.myRightCollectibles++;
			
			KillAllGuplingTweens();

			StartCoroutine(GulpTheAmmo(rightHandController));

			_rightGunScript.PickUpReaction();
			
			_sound.PlaySound(_sound.pickupSound);
		}
	}

	private IEnumerator GulpTheAmmo(HandGunController hand)
	{
		//_sound.PlaySound(_sound.gulpingSound);
		for (var i = 0; i < hand.myAmmo.Count; i++)
		{
			var initialScale = hand.myAmmo[i].transform.localScale;
			
			_gulpingTweens.Add(hand.myAmmo[i].transform.DOScale(initialScale * 1.5f, 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
			{
				_gulpingTweens.Add(hand.myAmmo[i].transform.DOScale(initialScale, 0.05f).SetEase(Ease.OutSine));
			}));
			
			yield return new WaitForSeconds(0.05f);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("GunSpawner")) return;
		//SpawnGuns();
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
		
		if(collectibleTransform.CompareTag("Solids"))
			collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.leftAmmoIndex * offsetOnY);
		else
			collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.rightAmmoIndex * offsetOnY);

		collectibleComponent.SwingMag(handGunController, mag);
	}

	public void OnStartShooting(HandGunController handGunController,GameObject muzzle)
	{
		//remove first ammo item from myammo list and move the bullets up
		//unparent the first ammo from player
		if (handGunController.myAmmo.Count == 0) return;
		
		//StartCoroutine(
		Shoot(handGunController,muzzle,handAnimator);
				//);
	}

	private void Shoot(HandGunController handGunController, GameObject muzzle,Animator anim)
	{
		var bullet = handGunController.myAmmo[0];
		var collectibleComponent = bullet.GetComponent<Collectible>();
		//collectibleComponent.ammoToFollow = bullet.transform;
		collectibleComponent.ammoToFollow = null;

		var laundeKaPosition = myAmmo[0].transform.position;
		handGunController.myAmmo.RemoveAt(0);
		// handGunController.myAmmo[0].GetComponent<Collectible>().ammoToFollow = transform;

		bullet.SetActive(true);
		collectibleComponent.StartMoving(muzzle.transform.position);

		if (bullet.CompareTag("Solids"))
		{
			LoadingAmmoIllusion(loadingBurgerAmmo,_burgerAmmoPos);

			leftMuzzleParticle.Play();
			if (arrangementForObstacleAimModeSwitch)
			{
				if (handGunController.myAmmo.Count > 0)
				{
					handGunController.myAmmo[0].GetComponent<Collectible>().ammoToFollow = leftMagPos.transform;
				}
				ReArrange(leftHandController, laundeKaPosition);
			}
		}
		else
		{
			LoadingAmmoIllusion(loadingCokeAmmo,_cokeAmmoPos);

			rightMuzzleParticle.Play();
			if (arrangementForObstacleAimModeSwitch)
			{
				if(rightHandController.myAmmo.Count > 0)
					rightHandController.myAmmo[0].GetComponent<Collectible>().ammoToFollow = rightMagPos.transform;
				ReArrange(rightHandController, laundeKaPosition);
			}
		}
		
		anim.SetTrigger(ToShoot);
	}

	private void ReArrange(HandGunController hand, Vector3 firstPos)
	{
		var ammoList = hand.myAmmo;
		//caller of this function has removed ammoList 0 and passed on its posn as firstpos
		KillAllMoveTweens();
		_moveTweeners.Add(ammoList[0].transform.DOMove(firstPos, 0.25f));
		
		if (ammoList.Count <= 1) return;

		for (var i = 1; i < ammoList.Count; i++)
			_moveTweeners.Add(ammoList[i].transform.DOMove(ammoList[i - 1].transform.position, 0.25f));
	}

	private void AdiosCollider()
	{
		gameObject.GetComponent<Collider>().enabled = false;
		rightGun.transform.parent = rightHand.transform;
		leftGun.transform.parent = leftHand.transform;
		_burgerAmmoPos = loadingBurgerAmmo.transform.position;
		_cokeAmmoPos = loadingCokeAmmo.transform.position;
	}

	public void SpawnGuns()
	{
		rightGun.SetActive(true);
		leftGun.SetActive(true);

		StartCoroutine(HoldGuns());
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

	private void StartShooting() => canShoot = true;

	private IEnumerator HoldGuns()
	{
		handAnimator.SetTrigger(HoldGunHash);

		yield return new WaitForSeconds(1f);
		var rightGunScale = rightGun.transform.lossyScale;
		var leftGunScale = leftGun.transform.lossyScale;
		rightGun.transform.parent = rightHandController.transform;
		//
		leftGun.transform.parent = leftHandController.transform;
	}

	private void StopShooting()
	{
		canShoot = false;
		_collider.enabled = true;
	}

	private void KillAllMoveTweens()
	{
		foreach (var t in _moveTweeners) t.Kill(true);

		_moveTweeners.Clear();
	}

	private void KillAllGuplingTweens()
	{
		foreach (var t in _gulpingTweens)
			t.Kill(true);
		
		_gulpingTweens.Clear();
	}
}
