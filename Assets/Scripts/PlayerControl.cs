using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
	public float movementSpeed = 5f;
	[SerializeField] private float xForce;
	public float xSpeed;
	[SerializeField] private float leftBoundary, rightBoundary;
	
	[SerializeField] private Transform offsetOnRight, offsetOnLeft;

	public bool walkState, canWalkAround = true;

	[SerializeField] private GameObject cameraFinalPosition;
	private Camera _camera;
	
	private float _swipeSpeed = 3f;

	[SerializeField] private HandGunController _handGunController;

	private float _temp;
	public int shootCount;
	private void OnEnable()
	{
		GameEvents.Ge.aimModeSwitch += OnAimModeSwitch;
		GameEvents.Ge.obstacleHeadAimSwitch += OnObstacleAimModeSwitch;
	}

	private void OnDisable()
	{
		GameEvents.Ge.aimModeSwitch -= OnAimModeSwitch;
		GameEvents.Ge.obstacleHeadAimSwitch -= OnObstacleAimModeSwitch;
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
			xForce = touchDeltaPosition.x*_swipeSpeed*Mathf.Deg2Rad;
          }
		if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		  {
			xForce = 0;
          }
	#endif
		
		if (walkState)
		{
			transform.Translate((Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f)) * Time.deltaTime, Space.World);
		}
		/*else
		{ 
			if (canWalkAround)
				if(Input.GetMouseButton(0))
				{
					if(xForce * xSpeed > 0)
						transform.Rotate(Vector3.up,1f);
					if(xForce * xSpeed < 0)
						transform.Rotate(Vector3.up,-1f);
				}
			
		} */
		
		if(transform.position.x < leftBoundary)
			transform.position = new Vector3(leftBoundary,transform.position.y,transform.position.z);
		else if (transform.position.x > rightBoundary)
			transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Divider"))
		{
			_handGunController.leftHandController.transform.parent.DOMoveX(
				other.transform.GetChild(0).transform.position.x, 1f);
			
			_handGunController.rightHandController.transform.parent.DOMoveX(
				other.transform.GetChild(1).transform.position.x,1f);
		}

		if (other.CompareTag("AimModeSwitch"))
		{
			GameEvents.Ge.InvokeObstacleHeadAimSwitch();
			_handGunController.leftHandController.arrangementForObstacleAimModeSwitch = true;
			_handGunController.rightHandController.arrangementForObstacleAimModeSwitch = true;
			AssignMouth(other.gameObject);
			other.gameObject.SetActive(false);
		}
		
		if (other.CompareTag("SolidHeadAimModeSwitch"))
		{
			print("Zero");
			_handGunController.leftHandController.GetComponent<Collider>().enabled = false;
			_handGunController.leftHandController.shootCount = other.gameObject.GetComponent<AimModeSwitch>().count;
			_handGunController.leftHandController.arrangementForObstacleAimModeSwitch = true;
			AssignMouth(other.gameObject);
			GameEvents.Ge.InvokeObstacleHeadAimSwitch();
			other.gameObject.SetActive(false);
		}
		
		if (other.CompareTag("LiquidHeadAimModeSwitch"))
		{
			_handGunController.rightHandController.GetComponent<Collider>().enabled = false;
			_handGunController.rightHandController.shootCount = other.gameObject.GetComponent<AimModeSwitch>().count;
			_handGunController.rightHandController.arrangementForObstacleAimModeSwitch = true;
			AssignMouth(other.gameObject);
			GameEvents.Ge.InvokeObstacleHeadAimSwitch();
			other.gameObject.SetActive(false);
		}

		if (other.CompareTag("FinalAimModeSwitch"))
		{
			var cameraTransform = _camera.transform;
			cameraTransform.DORotate(new Vector3(13.404f,0f,0f), 1f, RotateMode.Fast).OnComplete(() =>	
			{
				GameEvents.Ge.InvokeOnStartFeeding();
			});

			cameraTransform.DOMove(cameraFinalPosition.transform.position, 1f);
			cameraTransform.parent = transform;
			/*transform.DOMove(other.transform.position,0.2f).OnComplete(() =>
			{
				var cameraTransform = _camera.transform;
				cameraTransform.DORotate(Vector3.zero, 1f, RotateMode.Fast).OnComplete(() =>	
				{
					GameEvents.Ge.InvokeOnStartFeeding();
				});

				cameraTransform.DOMove(cameraFinalPosition.transform.position, 1f);
				cameraTransform.parent = transform;
			});*/
			_handGunController.leftHandController.arrangementForObstacleAimModeSwitch = true;
			_handGunController.rightHandController.arrangementForObstacleAimModeSwitch = true;

			AssignMouth(other.gameObject);
			GameEvents.Ge.InvokeOnAimModeSwitch();

		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("GunSpawner"))
		{
			_handGunController.SpawnGuns();
			
			SoundManager.Singleton.PlaySound(SoundManager.Singleton.gunEquipSound);
		}
	}
	
	private void OnAimModeSwitch()
	{
		var transformPosition = transform.position;

		walkState = false;
	}
	
	private void AssignMouth(GameObject aimModeSwitch)
	{
		for (int i = 0; i < _handGunController.leftHandController.myAmmo.Count; i++)
		{
			_handGunController.leftHandController.myAmmo[i].GetComponent<Collectible>().mouth =
				aimModeSwitch.GetComponent<AimModeSwitch>().mouthObject;
		}

		for (int i = 0; i < _handGunController.rightHandController.myAmmo.Count; i++)
		{
			_handGunController.rightHandController.myAmmo[i].GetComponent<Collectible>().mouth =
				aimModeSwitch.GetComponent<AimModeSwitch>().mouthObject;
		}
	}
	
	private void OnObstacleAimModeSwitch()
	{
		walkState = false;
		GameEvents.Ge.InvokeStartShooting();
	}
}


