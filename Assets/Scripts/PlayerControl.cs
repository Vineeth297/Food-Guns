using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private float xForce;
	[SerializeField] private float xSpeed;
	[SerializeField] private float leftBoundary, rightBoundary;

	
	[SerializeField] private GameObject rightHand;
	[SerializeField] private GameObject leftHand;

	private HandGunController _rightHandGun, _leftHandGun;

	private Camera _camera;

	[SerializeField] private float offsetOnY;

	private void Start()
	{
		_rightHandGun = rightHand.GetComponent<HandGunController>();
		_leftHandGun = leftHand.GetComponent<HandGunController>();
	}

    // Update is called once per frame
    void Update()
    {
		transform.Translate(Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed , 0f , 0f) * Time.deltaTime,Space.World);
		
		if(transform.position.x < leftBoundary)
			transform.position = new Vector3(leftBoundary,transform.position.y,transform.position.z);
		if(transform.position.x > rightBoundary)
			transform.position = new Vector3(rightBoundary,transform.position.y,transform.position.z);
		
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
		//if (other.CompareTag("Solids") || other.CompareTag("Liquids")) return;
		
		if (other.CompareTag("Solids"))
		{
			// _leftHandGun.myAmmo.Add(other.gameObject);
			// var ammoPos = other.transform.position;
			// other.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
			// other.GetComponent<Collectible>().isCollected = true;
			// other.GetComponent<Collectible>().transform.rotation = Quaternion.Euler(Vector3.zero);
			// other.transform.parent = leftHand.transform;
			// other.transform.localPosition = Vector3.zero;
			
			OnAmmoFound(_leftHandGun,other.gameObject);
		}

		if (other.CompareTag("Liquids"))
		{
			// _rightHandGun.myAmmo.Add(other.gameObject);
			// other.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
			// other.GetComponent<Collectible>().isCollected = true;
			// other.GetComponent<Collectible>().transform.rotation = Quaternion.Euler(Vector3.zero);
			// other.transform.parent = rightHand.transform;
			// other.transform.localPosition = Vector3.zero;
			
			OnAmmoFound(_rightHandGun,other.gameObject);
		}
	}

	private void OnAmmoFound([NotNull] HandGunController handGunFunction, GameObject collectible)
	{
		var collectibleTransform = collectible.transform;
		var collectibleComponent = collectible.GetComponent<Collectible>();

		handGunFunction.myAmmo.Add(collectible.gameObject);
		var ammoPos = collectible.transform.position;
		collectibleTransform.localScale = new Vector3(0.2f,0.2f,0.2f);
		collectibleComponent.isCollected = true;
		collectibleComponent.transform.rotation = Quaternion.Euler(Vector3.zero);
		if (handGunFunction.myAmmo.Count == 1)
		{
			collectibleTransform.parent = handGunFunction.transform;
			collectibleTransform.localPosition = Vector3.zero;
		}
		else
		{
			var lastElement = handGunFunction.myAmmo[^1].transform.localPosition;
			collectibleTransform.localPosition = lastElement;
			print(lastElement);
		}
		collectibleTransform.parent = handGunFunction.transform;

		//collectibleTransform.localPosition = new Vector3(0f,-yPos,0f);
	}
}
