using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private float xForce;
	[SerializeField] private float xSpeed;
	[SerializeField] private float leftBoundary, rightBoundary;

	public HandGunController rightHandGun, leftHandGun;
	[SerializeField] private float ammoSize;
	[SerializeField] private GameObject rightHand;
	[SerializeField] private GameObject leftHand;
	[SerializeField] private GameObject shootingPosition;
	[SerializeField] private Transform offset;

	[SerializeField] private float offsetOnY;
	public GameObject leftMagPos, rightMagPos;

	public List<Vector3> positions;
	public int intervalPos = 10;

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
		rightHandGun = rightHand.GetComponent<HandGunController>();
		leftHandGun = leftHand.GetComponent<HandGunController>();
	}

	public Vector3 oldPos;
	[SerializeField] private AnimationCurve animCurve;

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
		
		transform.Translate((Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f)) * Time.deltaTime,Space.World);

		if(transform.position.x < leftBoundary)
			transform.position = new Vector3(leftBoundary,transform.position.y,transform.position.z);
		else if(transform.position.x > rightBoundary)
			transform.position = new Vector3(rightBoundary,transform.position.y,transform.position.z);
		
		
		positions.Insert(0, offset.position);
			
		//OnStartShooting(leftHandGun);
		if(Input.GetKeyDown(KeyCode.Space))
			OnStartShooting(leftHandGun);

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids"))
		{
			GameEvents.Ge.onAmmoFound(leftHandGun,other.gameObject);
		}
	
		if (other.CompareTag("Liquids"))
		{
			GameEvents.Ge.onAmmoFound(rightHandGun, other.gameObject);
		}
	}

	private void OnAmmoFound(HandGunController handGunController, GameObject collectible)
	{
		collectible.GetComponent<Collider>().enabled = false;
		
		var collectibleTransform = collectible.transform;
		var collectibleComponent = collectible.GetComponent<Collectible>();
		
		var lastAmmoYPos = 0f;
		if(handGunController.myAmmo.Count > 0)
			lastAmmoYPos = handGunController.myAmmo[^1].transform.localPosition.z;
		// lastAmmoYPos = handGunController.myAmmo[^1].transform.position.y;
		
		handGunController.myAmmo.Add(collectible.gameObject);
		collectibleComponent.ammoFound = true;
		collectibleComponent.transform.rotation = Quaternion.Euler(Vector3.zero);

		collectibleTransform.position = leftMagPos.transform.position - new Vector3(0f,0f,collectibleComponent.ammoIndex * offsetOnY);
		
		//if(handGunController.myAmmo.Count == 1)
		//collectibleTransform.parent = leftMagPos.transform;
		
		collectibleTransform.localScale = Vector3.one * ammoSize;
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
		}*/
		collectibleComponent.SwingMag();

	}

	private void OnStartShooting(HandGunController handGunController)
	{
		//remove first ammo item from myammo list and move the bullets up
		//unparent the first ammo from player
		if (handGunController.myAmmo.Count == 0) return;
		
		StartCoroutine(Shoot(handGunController));
		
	}

	IEnumerator Shoot(HandGunController handGunController)
	{
		while (handGunController.myAmmo.Count != 0)
		{
			GameObject bullet = handGunController.myAmmo[0];
			handGunController.myAmmo.RemoveAt(0);
			
			bullet.GetComponent<Collectible>().StartMoving(shootingPosition.transform.position);

			for (int i = 1; i < handGunController.myAmmo.Count; i++)
			{
				handGunController.myAmmo[i].GetComponent<Collectible>().ammoIndex -= 1;
			}
			yield return new WaitForSeconds(0.15f);
		}
	}
}

