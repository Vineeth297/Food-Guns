using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private float xForce;
	[SerializeField] private float xSpeed;
	[SerializeField] private float leftBoundary, rightBoundary;

	[SerializeField] private float ammoSize;
	[SerializeField] private GameObject rightHand;
	[SerializeField] private GameObject leftHand;
	[SerializeField] private GameObject shootingPosition;
	private HandGunController _rightHandGun, _leftHandGun;
	
	private Camera _camera;

	[SerializeField] private float offsetOnY;
	[SerializeField] private float extendedMagPositionOnZ;

	private void OnEnable()
	{
		GameEvents.Ge.onAmmoFound += OnAmmoFound;
	}

	private void OnDisable()
	{
		GameEvents.Ge.onAmmoFound -= OnAmmoFound;
	}

	private void Start()
	{
		_rightHandGun = rightHand.GetComponent<HandGunController>();
		_leftHandGun = leftHand.GetComponent<HandGunController>();
	}
	
    void Update()
    {
		transform.Translate(Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed , 0f , 0f) * Time.deltaTime,Space.World);
		
		if(transform.position.x < leftBoundary)
			transform.position = new Vector3(leftBoundary,transform.position.y,transform.position.z);
		if(transform.position.x > rightBoundary)
			transform.position = new Vector3(rightBoundary,transform.position.y,transform.position.z);
		
		OnStartShooting(_leftHandGun);
		
	#if UNITY_EDITOR
		xForce = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * xSpeed : 0;
	#elif UNITY_ANDROID
        if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		  {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			xForce = touchDeltaPosition.x*swipeSpeed*Mathf.Deg2Rad;
          }
	#endif
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids"))
		{
			GameEvents.Ge.onAmmoFound(_leftHandGun,other.gameObject);
		}

		if (other.CompareTag("Liquids"))
		{
			GameEvents.Ge.onAmmoFound(_rightHandGun, other.gameObject);
		}
	}

	private void OnAmmoFound(HandGunController handGunController, GameObject collectible)
	{
		collectible.GetComponent<Collider>().enabled = false;
		var collectibleTransform = collectible.transform;
		var collectibleComponent = collectible.GetComponent<Collectible>();

		handGunController.myAmmo.Add(collectible.gameObject);
		collectibleTransform.localScale = Vector3.one * ammoSize;
		collectibleComponent.ammoFound = true;
		collectibleComponent.transform.rotation = Quaternion.Euler(Vector3.zero);
		collectibleTransform.parent = handGunController.transform;
		
		var yPos = (handGunController.myAmmo.Count - 1) * handGunController.myAmmo[^1].GetComponent<Renderer>().bounds.size.y * offsetOnY ;
		collectibleTransform.localPosition = new Vector3(0f,-yPos , extendedMagPositionOnZ) ;
		
	}

	private void OnStartShooting(HandGunController handGunController)
	{
		//remove first ammo item from myammo list and move the bullets up
		//unparent the first ammo from player
		if (handGunController.myAmmo.Count == 0) return;
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			GameObject bullet = handGunController.myAmmo[0];
			handGunController.myAmmo.RemoveAt(0);
			bullet.transform.localPosition = shootingPosition.transform.localPosition;
			bullet.transform.parent = null;
			bullet.GetComponent<Collectible>().startShooting = true;

			for (int i = 0; i < handGunController.myAmmo.Count; i++)
			{
				var yPos = i * handGunController.myAmmo[^1].GetComponent<Renderer>().bounds.size.y * offsetOnY ;
				handGunController.myAmmo[i].transform.localPosition = new Vector3(0f,-yPos , extendedMagPositionOnZ) ;	
			}
		}
	}
}

