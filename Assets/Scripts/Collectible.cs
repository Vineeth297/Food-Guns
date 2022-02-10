using System;
using UnityEngine;
using DG.Tweening;
using Random = System.Random;

public class Collectible : MonoBehaviour
{
	public bool ammoFound;
	public bool startMoving;
	public bool doSnake;
	public float damping;
	public Vector3 offsetOnY;
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

	private bool _isPickedUp;

	[SerializeField,Range(0.001f,0.01f)] private float value = 0.001f;
	
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

		/*_playerControl = FindObjectOfType<PlayerControl>();
		if (CompareTag("Solids"))
			_handGunController = _playerControl.leftHandGun;
		else
			_handGunController = _playerControl.rightHandGun;*/

		_initScale = transform.localScale;

	}
	
	private void Update()
	{
		// if(doSnake)
		// 	SnakeMovement();
		
		if(doSnake)
			FollowTheGuy();
		
		if(startMoving)
			Shoot();
	}

	public Vector3 followOffset;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != tag) return;
		if(!other.GetComponent<Collectible>()._isPickedUp)
			PickUpScaleAnim(other.gameObject);
		handGunController.OnAmmoFound(handGunController,other.gameObject,handGunController.isLeftHand ? handGunController.leftMagPos : handGunController.rightMagPos);
	}

	private void OnAimModeSwitch()
	{
		if (_playerControl.walkState) return;
		if (startMoving) return;
		// Vector3 smoothPos = Vector3.Lerp(transform.position, ammoToFollow.position + followOffset , Time.deltaTime * damping);
		// transform.position = smoothPos;
		// transform.eulerAngles = ammoToFollow.eulerAngles;
		//print(transform.position);
		_collider.enabled = false;
		doSnake = false;
	}
	private void Shoot()
	{
		// transform.Translate(Vector3.forward,Space.World);
		transform.Translate(camera.transform.forward,Space.World);
	}

	public void StartMoving(Vector3 transformPosition)
	{
		transform.position = transformPosition;
		startMoving = true;
	}

	 public void SwingMag(HandGunController handGunController ,GameObject mag)
	 {
	 	if (handGunController.myAmmo.Count == 0) return;

		if (CompareTag("Solids"))
		{
			leftAmmoIndex = handGunController.myAmmo.Count;

			if (leftAmmoIndex == 1)
				ammoToFollow = mag.transform;
			else
				ammoToFollow = handGunController.myAmmo[leftAmmoIndex - 2].transform;
		}
		else
		{
			rightAmmoIndex = handGunController.myAmmo.Count;

			if (rightAmmoIndex == 1)
				ammoToFollow = mag.transform;
			else
				ammoToFollow = handGunController.myAmmo[rightAmmoIndex - 2].transform;
		}
		
		canFilterTheList = true;
	 }

	 private void SnakeMovement()
	 {
		 if (!startMoving)
		 {
			 if (CompareTag("Solids"))
				 try
				 {
					 var myPos = _playerControl.leftPositions[(_playerControl.leftIntervalPos * leftAmmoIndex)];
					 myPos.z = _playerControl.leftPositions[0].z + (_playerControl.leftPositions[0].z - myPos.z);
					
					 transform.position = Vector3.Lerp(transform.position, myPos + Vector3.right * value, Time.deltaTime * damping);
					 // transform.position = Vector3.SmoothDamp(transform.position, myPos, ref myPos, 1f, Time.deltaTime * damping);
					 //transform.position = Vector3.MoveTowards(transform.position,myPos,Time.deltaTime * damping);
					 //transform.position = Vector3.Lerp(transform.position, new Vector3(myPos.x * value,myPos.y,myPos.z), Time.deltaTime * damping);
				 }
				 catch (Exception e)
				 {
					// print($"ammoIndex = {ammoIndex}, result = {_playerControl.intervalPos * ammoIndex}, leftPositions.count = {_playerControl.leftPositions.Count}");
				 }
			 else
				 try
				 { 
					 var myPos = _playerControl.rightPositions[(_playerControl.rightIntervalPos * rightAmmoIndex)];
					 myPos.z = _playerControl.rightPositions[0].z + (_playerControl.rightPositions[0].z - myPos.z);

					 transform.position = Vector3.Lerp(transform.position, myPos, Time.deltaTime * damping);
				 }
				 catch (Exception e)
				 {
					// print($"ammoIndex = {ammoIndex}, result = {_playerControl.intervalPos * ammoIndex}, rightPositions.count = {_playerControl.rightPositions.Count}");
				 }
		 }
		 
		 if (canFilterTheList)
		{
			if(CompareTag("Solids"))
				_playerControl.leftPositions.RemoveRange((_playerControl.leftIntervalPos * leftAmmoIndex), _playerControl.leftPositions.Count - (_playerControl.leftIntervalPos * leftAmmoIndex));
			else
				_playerControl.rightPositions.RemoveRange((_playerControl.rightIntervalPos * rightAmmoIndex), _playerControl.rightPositions.Count - (_playerControl.rightIntervalPos * rightAmmoIndex));
		}
		
		canFilterTheList = false;
	 }
	
	 /*public bool ammoFound;
	 public bool startMoving;
	 
	 public float damping;
	 public Vector3 offsetOnY;
	 public Transform ammoToFollow;
	 public int ammoIndex;
	 HandGunController _handGunController;
	 private PlayerControl _playerControl;
 
	 private Rigidbody _rb;
	 private Vector3 _lastPosition;
	 [SerializeField] private float forwardOffset;
 
	 public bool canFilterTheList;
 
	 void Start()
	 {
		 _playerControl = FindObjectOfType<PlayerControl>();
		 if (CompareTag("Solids"))
			 _handGunController = _playerControl.leftHandGun;
		 else
			 _handGunController = _playerControl.rightHandGun;
	 }
	 
	 private void Update()
	 {
		 if(!ammoFound)
			 transform.Rotate(45f * Time.deltaTime, 45f * Time.deltaTime, 45f * Time.deltaTime);
		 
		 if(ammoFound)
			 SnakeMovement();
		 
		 if(startMoving)
			 Shoot();
	 }
 
	 private void Shoot()
	 {
		 transform.Translate(Vector3.forward,Space.World);
	 }
 
	 public void StartMoving(Vector3 transformPosition)
	 {
		 transform.position = transformPosition;
		 startMoving = true;
	 }
 
	  public void SwingMag(GameObject mag)
	  {
		  if (_handGunController.myAmmo.Count == 0) return;
		 
		 ammoIndex = _handGunController.myAmmo.Count;
 
		 if (ammoIndex == 1)
			 ammoToFollow = mag.transform;
		 else
			 ammoToFollow = _handGunController.myAmmo[ammoIndex - 2].transform;
		 
		 canFilterTheList = true;
	  }
 
	  private void SnakeMovement()
	  {
		  if (!startMoving)
		  {
			  if(CompareTag("Solids"))
				 transform.position = Vector3.Lerp(transform.position, _playerControl.leftPositions[(_playerControl.intervalPos * ammoIndex)], Time.deltaTime * damping);
			  else
				  transform.position = Vector3.Lerp(transform.position, _playerControl.rightPositions[(_playerControl.intervalPos * ammoIndex)], Time.deltaTime * damping);
		  }
		  // transform.position = Vector3.Lerp(transform.position,
			 //  _playerControl.Positions[(_playerControl.IntervalPos * ammoIndex) * (int)_playerControl.PositionValiation(_playerControl.IntervalPos)],
			 //  Time.deltaTime * damping);
		 
	 //	 print($"vlaidated fkiat {_playerControl.PositionValiation(_playerControl.IntervalPos)} for {ammoIndex}");
 
		 if (canFilterTheList)
		 {
			 if(CompareTag("Solids"))
				 _playerControl.leftPositions.RemoveRange((_playerControl.intervalPos * ammoIndex), _playerControl.leftPositions.Count - (_playerControl.intervalPos * ammoIndex));
			 else
				 _playerControl.rightPositions.RemoveRange((_playerControl.intervalPos * ammoIndex), _playerControl.rightPositions.Count - (_playerControl.intervalPos * ammoIndex));
		 }
		 
		 canFilterTheList = false;
	  }*/

	 private void PickUpScaleAnim(GameObject pickedUpObject)
	 {
		 var pickUpInitScale = pickedUpObject.transform.localScale;
		 Sequence mySequence = DOTween.Sequence();

		 mySequence.Append(pickedUpObject.transform.DOScale(pickUpInitScale + (pickUpInitScale * 0.2f), 0.5f).SetEase(Ease.OutElastic));
		 mySequence.Append(pickedUpObject.transform.DOScale(pickUpInitScale, 0.1f));
		 pickedUpObject.GetComponent<Collectible>()._isPickedUp= true;
	 }

	 private void FollowTheGuy()
	 {
		 if (ammoToFollow != null)
		 {
			 Vector3 smoothPos = Vector3.Lerp(transform.position, ammoToFollow.position + followOffset,
				 Time.deltaTime * damping);
			 transform.position = smoothPos;
			 transform.eulerAngles = ammoToFollow.eulerAngles;
		 }
	 }
}
