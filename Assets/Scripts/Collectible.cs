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

	private void FixedUpdate()
	{
		//_rb.MovePosition(Vector3.Lerp(transform.position, ammoToFollow.position, Time.fixedDeltaTime * damping));
	}

	private void Shoot()
	{
		transform.Translate(transform.forward,Space.Self);
	}

	public void StartMoving(Vector3 transformPosition)
	{
		startMoving = true;
		transform.position = transformPosition;
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
		 transform.position = Vector3.Lerp(transform.position, _playerControl.Positions[(_playerControl.IntervalPos * ammoIndex)], Time.deltaTime * damping);

		 if (canFilterTheList)
		 {
			 _playerControl.Positions.RemoveRange((_playerControl.IntervalPos * ammoIndex ),_playerControl.Positions.Count - (_playerControl.IntervalPos * ammoIndex ) );

		 }
		 canFilterTheList = false;

		 // _playerControl.Positions.RemoveRange((_playerControl.IntervalPos * ammoIndex ),_playerControl.Positions.Count - (_playerControl.IntervalPos * ammoIndex ) );
	 }

}
