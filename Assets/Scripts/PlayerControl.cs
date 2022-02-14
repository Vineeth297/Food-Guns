using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
	public float movementSpeed = 5f;
	[SerializeField] private float xForce;
	public float xSpeed;
	[SerializeField] private float leftBoundary, rightBoundary;
	
	[SerializeField] private Transform offsetOnRight, offsetOnLeft;

	public List<Vector3> leftPositions;
	public List<Vector3> rightPositions;
	public int leftIntervalPos = 10;
	public int rightIntervalPos = 10;

	public bool walkState, canWalkAroundVineetMakeThisVariableHiddenAndProtectItWithAMethod = true;

	[SerializeField] private GameObject cameraFinalPosition;
	private Camera _camera;
	
	private float _swipeSpeed = 15f;
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
			leftPositions.Insert(0, offsetOnLeft.position);
			rightPositions.Insert(0, offsetOnRight.position);

		}
		else
		{
			if (canWalkAroundVineetMakeThisVariableHiddenAndProtectItWithAMethod)
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
		
	}
	
	private void OnAimModeSwitch()
	{
		//transform.DOLocalMove(new Vector3(0f, transform.position.y, transform.position.z), 0.5f);
		walkState = false;
		transform.position = new Vector3(0f,transform.position.y,transform.position.z);
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
}


