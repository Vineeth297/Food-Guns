using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private float xForce;
	[SerializeField] private float xSpeed;
	[SerializeField] private float leftBoundary, rightBoundary;
	
	[SerializeField] private Transform offsetOnRight, offsetOnLeft;

	public List<Vector3> leftPositions;
	public List<Vector3> rightPositions;
	public int leftIntervalPos = 10;
	public int rightIntervalPos = 10;

	public bool walkState;

	[SerializeField] private GameObject cameraFinalPosition;
	private Camera _camera;
	private void OnEnable()
	{
		GameEvents.Ge.aimModeSwitch += OnAimModeSwitch;
	}

	private void OnDisable()
	{
		GameEvents.Ge.aimModeSwitch -= OnAimModeSwitch;
	}

	private void Start()
	{
		walkState = true;
		_camera = Camera.main;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		
	#if UNITY_EDITOR
		xForce = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * xSpeed : 0;
	#elif UNITY_ANDROID
        if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		  {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			xForce = touchDeltaPosition.x*swipeSpeed*Mathf.Deg2Rad;
          }
	#endif

		if (walkState)
		{
			transform.Translate((Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f)) * Time.deltaTime, Space.World);
			leftPositions.Insert(0, offsetOnLeft.position);
			rightPositions.Insert(0, offsetOnRight.position);
		}
		else
		{
			if(Input.GetMouseButton(0))
			{
				//transform.Translate(new Vector3(xForce * xSpeed, 0f, 0f) * Time.deltaTime);
				if(xForce * xSpeed > 0)
					transform.Rotate(Vector3.up,1f);
				if(xForce * xSpeed < 0)
					transform.Rotate(Vector3.up,-1f);
			}
		} 
		if(transform.position.x < leftBoundary)
			transform.position = new Vector3(leftBoundary,transform.position.y,transform.position.z);
		else if(transform.position.x > rightBoundary)
			transform.position = new Vector3(rightBoundary,transform.position.y,transform.position.z);
		
		/*if (Input.GetKeyDown(KeyCode.Space))
		{
			OnStartShooting(leftHandGun,leftMuzzle);
			OnStartShooting(rightHandGun,rightMuzzle);
		}*/
	}
	
	private void OnAimModeSwitch()
	{
		walkState = false;
		//Camera.main.GetComponent<CameraFollow>().enabled = false;
		var cameraTransform = _camera.transform;
		//_camera.transform.rotation = Quaternion.Euler(Vector3.zero);
		cameraTransform.DORotate(Vector3.zero, 1f, RotateMode.Fast).OnComplete(() =>
		{
			GameEvents.Ge.InvokeOnStartFeeding();
		});
		cameraTransform.DOMove(cameraFinalPosition.transform.position, 1f);
		cameraTransform.parent = transform;
	}
	
	/*
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private float xForce;
	[SerializeField] private float xSpeed;
	[SerializeField] private float leftBoundary, rightBoundary;

	public HandGunController rightHandGun, leftHandGun;
	[SerializeField] private float ammoSize;
	[SerializeField] private GameObject rightHand;
	[SerializeField] private GameObject leftHand;
	[SerializeField] private GameObject leftMuzzle, rightMuzzle;
	[SerializeField] private Transform offsetOnRight, offsetOnLeft;

	[SerializeField] private float offsetOnY;
	public GameObject leftMagPos, rightMagPos;

	[HideInInspector] public List<Vector3> leftPositions;
	[HideInInspector] public List<Vector3> rightPositions;
	public int intervalPos = 10;
	
	
	 private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids"))
		{
			GameEvents.Ge.InvokeOnAmmoFound(leftHandGun, other.gameObject, leftMagPos);
		}
	
		if (other.CompareTag("Liquids"))
		{
			GameEvents.Ge.InvokeOnAmmoFound(rightHandGun, other.gameObject, rightMagPos);
		}
	}

	private void OnAmmoFound(HandGunController handGunController, GameObject collectible, GameObject mag)
	{
		collectible.GetComponent<Collider>().enabled = false;
		
		var collectibleTransform = collectible.transform;
		var collectibleComponent = collectible.GetComponent<Collectible>();
		
		var lastAmmoYPos = 0f;
		if(handGunController.myAmmo.Count > 0)
			lastAmmoYPos = handGunController.myAmmo[^1].transform.localPosition.z;
		// lastAmmoYPos = handGunController.myAmmo[^1].transform.position.y;
		
		handGunController.myAmmo.Add(collectible);
		collectibleComponent.ammoFound = true;
		collectibleComponent.transform.rotation = Quaternion.Euler(Vector3.zero);

		collectibleTransform.position = mag.transform.position - new Vector3(0f,0f,collectibleComponent.ammoIndex * offsetOnY);
		
		//if(handGunController.myAmmo.Count == 1)
		//collectibleTransform.parent = leftMagPos.transform;
		if(collectible.CompareTag("Solids"))
			collectibleTransform.localScale = Vector3.one * ammoSize;

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
		}#1#
		collectibleComponent.SwingMag(mag);

	}

	private void OnStartShooting(HandGunController handGunController,GameObject muzzle)
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
				handGunController.myAmmo[i].GetComponent<Collectible>().ammoIndex -= 1;
			}
			yield return new WaitForSeconds(0.15f);
		}
	}*/
}

