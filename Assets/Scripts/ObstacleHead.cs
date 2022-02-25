using DG.Tweening;
using TMPro;
using UnityEngine;

public enum FoodType
{
	Solids,
	Liquids
}

public class ObstacleHead : MonoBehaviour
{
	[SerializeField] private Animator animator;
	private static readonly int Eat = Animator.StringToHash("Eat");

	[SerializeField] private FoodType foodType = FoodType.Solids;

	[SerializeField] private TMP_Text text;
	public int maxEat;
	[SerializeField] private ParticleSystem eatingParticleSystem;

	private bool _iAmFull;
	private PlayerControl _playerControl;
	private float _playerInitialSpeed;

	public HandGunController rightHandCon, leftHandCon;
	private void Start()
	{
		text.text = maxEat.ToString();
		_playerControl = FindObjectOfType<PlayerControl>();
		_playerInitialSpeed = _playerControl.movementSpeed;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(foodType.ToString()))
		{
			animator.SetTrigger(Eat);
			eatingParticleSystem.Play();
		}

		if (other.CompareTag("Player"))
			CloseMouth();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag(foodType.ToString()))
		{
			GameEvents.Ge.InvokeObstacleHeadEatingBurger();
			
			maxEat -= 1;
			text.text = maxEat.ToString();
			
			other.GetComponent<Collectible>().handGunController.myAmmo.Remove(other.gameObject);
			other.gameObject.SetActive(false);
			
			if (maxEat == 0)
				GoUnder(other.gameObject);
		}
	}

	public void OpenMouth() => animator.speed = 0;

	private void CloseMouth() => animator.speed = 1;

	private void GoUnder(GameObject foodItem)
	{
		SoundManager.Singleton.PlaySound(SoundManager.Singleton.hmmSound);
		GetComponent<Collider>().enabled = false;
		transform.DOMove(transform.position + Vector3.down * 6.5f, 1f).OnComplete(() =>
		{
			print("HEre");
			_playerControl.walkState = true;
			_playerControl.movementSpeed = _playerInitialSpeed;
			GameEvents.Ge.InvokeOnContinueFollowing();
			GameEvents.Ge.InvokeStopShooting();

			print("Speed -> " + _playerControl.movementSpeed);
		});
		
		if (foodItem.CompareTag("Solids"))
		{
			if (GameManager.Singleton.myLeftCollectibles < 1)
				GameManager.Singleton.lostPanel.SetActive(true);
		}
		else if(foodItem.CompareTag("Liquids"))
		{
			if(GameManager.Singleton.myRightCollectibles < 1)
				GameManager.Singleton.lostPanel.SetActive(true);
		}
	}
}
