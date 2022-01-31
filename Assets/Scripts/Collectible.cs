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

	void Start()
	{
		_handGunController = FindObjectOfType<HandGunController>();
		_playerControl = FindObjectOfType<PlayerControl>();
	}
	
	private void Update()
	{
		if(!ammoFound)
			transform.Rotate(45f * Time.deltaTime, 45f * Time.deltaTime, 45f * Time.deltaTime);

		if(startMoving)
			Shoot();
	}

	private void LateUpdate()
	{	
		if (ammoFound)
		{
			Vector3 smoothPos = Vector3.Lerp(transform.position, new Vector3(ammoToFollow.position.x,transform.position.y,ammoToFollow.position.z) ,Time.deltaTime * damping);//b: new Vector3(ammoToFollow.position.x,transform.position.y,ammoToFollow.position.z), 
			transform.position = smoothPos;
			transform.eulerAngles = ammoToFollow.eulerAngles;
		}
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
		
		
		if (_handGunController.myAmmo.Count == 1)
		{
			ammoIndex = 1;
			ammoToFollow = _playerControl._leftHandGun.myAmmo[^1].transform;
		}
		else if (_handGunController.myAmmo.Count == 2)
		{
			ammoIndex = 2;
			ammoToFollow = _playerControl._leftHandGun.myAmmo[0].transform;
		}
		else 
		{
			ammoIndex = _handGunController.myAmmo.Count;
			ammoToFollow = _playerControl._leftHandGun.myAmmo[ammoIndex - 2].transform;
		}
	}
}
