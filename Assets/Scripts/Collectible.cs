using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	public bool ammoFound;
	public bool startMoving;
	
	public float damping;
	public Vector3 offsetOnY;
	public Transform ammoToFollow;
	public int leftAmmoIndex, rightAmmoIndex;
	HandGunController _handGunController;
	private PlayerControl _playerControl;

	private Rigidbody _rb;
	private Vector3 _lastPosition;
	[SerializeField] private float forwardOffset;

	public bool canFilterTheList;

	void Start()
	{
		_playerControl = FindObjectOfType<PlayerControl>();
		_handGunController = FindObjectOfType<HandGunController>();
		
		if (CompareTag("Solids"))
			_handGunController = _handGunController.leftHandGun;
		else
			_handGunController = _handGunController.rightHandGun;
		
		/*_playerControl = FindObjectOfType<PlayerControl>();
		if (CompareTag("Solids"))
			_handGunController = _playerControl.leftHandGun;
		else
			_handGunController = _playerControl.rightHandGun;*/
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

	 public void SwingMag(HandGunController handGunController ,GameObject mag)
	 {
	 	if (handGunController.myAmmo.Count == 0) return;

		if (CompareTag("Solids"))
		{
			leftAmmoIndex = handGunController.myAmmo.Count;
		
			print("left " + leftAmmoIndex);	

			if (leftAmmoIndex == 1)
				ammoToFollow = mag.transform;
			else
				ammoToFollow = handGunController.myAmmo[leftAmmoIndex - 2].transform;
		}
		else
		{
			rightAmmoIndex = handGunController.myAmmo.Count;
		
			print("Right"+ rightAmmoIndex);	

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
					 transform.position = Vector3.Lerp(transform.position,
						 _playerControl.leftPositions[(_playerControl.intervalPos * leftAmmoIndex)],
						 Time.deltaTime * damping);
					 }
					 catch (Exception e)
					 {
						// print($"ammoIndex = {ammoIndex}, result = {_playerControl.intervalPos * ammoIndex}, leftPositions.count = {_playerControl.leftPositions.Count}");
					 }
				 else
					 try
					 {
						 transform.position = Vector3.Lerp(transform.position,
							 _playerControl.rightPositions[(_playerControl.intervalPos * rightAmmoIndex)],
							 Time.deltaTime * damping);
					 }
					 catch (Exception e)
					 {
						// print($"ammoIndex = {ammoIndex}, result = {_playerControl.intervalPos * ammoIndex}, rightPositions.count = {_playerControl.rightPositions.Count}");
					 }
					 
			 
		 }
		 // transform.position = Vector3.Lerp(transform.position,
			//  _playerControl.Positions[(_playerControl.IntervalPos * ammoIndex) * (int)_playerControl.PositionValiation(_playerControl.IntervalPos)],
			//  Time.deltaTime * damping);
		
	//	 print($"vlaidated fkiat {_playerControl.PositionValiation(_playerControl.IntervalPos)} for {ammoIndex}");

		if (canFilterTheList)
		{
			if(CompareTag("Solids"))
				_playerControl.leftPositions.RemoveRange((_playerControl.intervalPos * leftAmmoIndex), _playerControl.leftPositions.Count - (_playerControl.intervalPos * leftAmmoIndex));
			else
				_playerControl.rightPositions.RemoveRange((_playerControl.intervalPos * rightAmmoIndex), _playerControl.rightPositions.Count - (_playerControl.intervalPos * rightAmmoIndex));
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
}
