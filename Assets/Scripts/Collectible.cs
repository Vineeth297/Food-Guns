using UnityEngine;
using DG.Tweening;

public class Collectible : MonoBehaviour
{
	public bool ammoFound;
	public bool startMoving;
	public bool doSnake;
	public float damping;
	public Transform ammoToFollow;
	public int leftAmmoIndex, rightAmmoIndex;
	[HideInInspector] public HandGunController handGunController;
	private PlayerControl _playerControl;

	private Rigidbody _rb;
	private Vector3 _lastPosition;
	[SerializeField] private float forwardOffset;

	public bool canFilterTheList;

	public Camera camera;
	private Collider _collider;

	private Vector3 _initScale;

	public bool isPickedUp;
	private static bool _disableCameraFollow;
	
	[SerializeField,Range(0.001f,0.01f)] private float value = 0.001f;

	[SerializeField] private GameObject mouth;
	
	[SerializeField] private float force = 5f;

	private void OnEnable()
	{
		GameEvents.Ge.aimModeSwitch += OnAimModeSwitch;
	}
	
	private void OnDisable()
	{
		GameEvents.Ge.aimModeSwitch -= OnAimModeSwitch;
	}
	void Start()
	{
		_collider = GetComponent<Collider>();
		_playerControl = FindObjectOfType<PlayerControl>();
		handGunController = FindObjectOfType<HandGunController>();
		
		if (CompareTag("Solids"))
			handGunController = handGunController.leftHandController;
		else
			handGunController = handGunController.rightHandController;
		
		_initScale = transform.localScale;
		
		camera = Camera.main;

		_rb = GetComponent<Rigidbody>();
	}
	
	private void Update()
	{
		if(doSnake)
			FollowTheGuy();
		
		if(startMoving)
			Shoot();
	}

	public Vector3 followOffset;
	
	private void OnTriggerEnter(Collider other)
	{	
		if (other.CompareTag("GunSpawner"))
		{
			if (!_disableCameraFollow)
			{
				_disableCameraFollow = true;
				_playerControl.xSpeed = 0f;
				var playerPosition = _playerControl.gameObject.transform.position;
				playerPosition.x = 0f;

				_playerControl.canWalkAround = false;
				_playerControl.gameObject.transform.DOMove(playerPosition, 0.5f).OnComplete(() =>
				{
					gameObject.SetActive(false);

				}); // = playerPosition;
			}
			else
				gameObject.SetActive(false);
		}

		if (!other.CompareTag(tag)) return;

		if (other.TryGetComponent(out Collectible collected) && !collected.isPickedUp)
		{
			Count(other.gameObject);
			collected.isPickedUp = true;
			PickUpScaleAnim(other.gameObject);
		}
		
		handGunController.OnAmmoFound(handGunController,other.gameObject,handGunController.isLeftHand ? handGunController.leftMagPos : handGunController.rightMagPos);
	}

	private void Count(GameObject other)
	{
		if (other.CompareTag("Solids"))
			GameManager.Singleton.myLeftCollectibles++;
		else
			GameManager.Singleton.myRightCollectibles++;
	}

	private void OnAimModeSwitch()
	{
		if (_playerControl.walkState) return;
		if (startMoving) return;
		
		_collider.enabled = false;
		doSnake = false;
		followOffset.z = 0;

	}
	private void Shoot()
	{
		transform.Translate(camera.transform.forward,Space.World);
	}

	public void StartMoving(Vector3 transformPosition)
	{
		transform.position = transformPosition;
		var dir = mouth.transform.position - transformPosition;
		_rb.isKinematic = false;
		_rb.AddForce(dir * (force),ForceMode.Impulse);
		//startMoving = true;
	}

	 public void SwingMag(HandGunController theHandGunController ,GameObject mag)
	 {
	 	if (theHandGunController.myAmmo.Count == 0) return;

		if (CompareTag("Solids"))
		{
			leftAmmoIndex = theHandGunController.myAmmo.Count;

			if (leftAmmoIndex == 1)
				ammoToFollow = mag.transform;
			else
				ammoToFollow = theHandGunController.myAmmo[leftAmmoIndex - 2].transform;
		}
		else
		{
			rightAmmoIndex = theHandGunController.myAmmo.Count;

			if (rightAmmoIndex == 1)
				ammoToFollow = mag.transform;
			else
				ammoToFollow = theHandGunController.myAmmo[rightAmmoIndex - 2].transform;
		}

		canFilterTheList = true;
	 }

	 private void PickUpScaleAnim(GameObject pickedUpObject)
	 {
		 var pickUpInitScale = pickedUpObject.transform.localScale;
		 Sequence mySequence =  DOTween.Sequence();

		 mySequence.Append(pickedUpObject.transform.DOScale(pickUpInitScale + (pickUpInitScale * 0.2f), 0.5f).SetEase(Ease.OutElastic));
		 mySequence.Append(pickedUpObject.transform.DOScale(pickUpInitScale, 0.1f));
		 pickedUpObject.GetComponent<Collectible>().isPickedUp = true;
	 }

	 private void FollowTheGuy()
	 {
		 if (ammoToFollow != null)
		 {
			 Vector3 smoothPos = Vector3.Lerp(transform.position, ammoToFollow.position + followOffset,
				 Time.deltaTime * damping);
			 transform.position = smoothPos;
		 }
	 }
}
