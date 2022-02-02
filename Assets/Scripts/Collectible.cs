using TreeEditor;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	public bool ammoFound;
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
		_handGunController = FindObjectOfType<HandGunController>();
		_playerControl = FindObjectOfType<PlayerControl>();
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
		transform.Translate(transform.forward,Space.Self);
	}

	public void StartMoving(Vector3 transformPosition)
	{
		transform.position = transformPosition;
		startMoving = true;
	}

	 public void SwingMag()
	 {
	 	if (_handGunController.myAmmo.Count == 0) return;
		
		ammoIndex = _handGunController.myAmmo.Count;

		if (ammoIndex == 1)
			ammoToFollow = _playerControl.leftMagPos.transform;
		else
			ammoToFollow = _handGunController.myAmmo[ammoIndex - 2].transform;

		
		canFilterTheList = true;
	 }

	 private void SnakeMovement()
	 {
		 if(!startMoving)
			transform.position = Vector3.Lerp(transform.position, _playerControl.positions[(_playerControl.intervalPos * ammoIndex)], Time.deltaTime * damping);
		 // transform.position = Vector3.Lerp(transform.position,
			//  _playerControl.Positions[(_playerControl.IntervalPos * ammoIndex) * (int)_playerControl.PositionValiation(_playerControl.IntervalPos)],
			//  Time.deltaTime * damping);
		
	//	 print($"vlaidated fkiat {_playerControl.PositionValiation(_playerControl.IntervalPos)} for {ammoIndex}");
		 
		 if (canFilterTheList)
			 _playerControl.positions.RemoveRange((_playerControl.intervalPos * ammoIndex), _playerControl.positions.Count - (_playerControl.intervalPos * ammoIndex));
		 
		 canFilterTheList = false;
	 }

}
