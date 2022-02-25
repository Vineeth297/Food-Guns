using UnityEngine;
using TMPro;

public class HeadAimTriggerVideoLevel : MonoBehaviour
{
	public GameObject mouthObject;

	private Collider _collider;
	
	private PlayerControl _playerControl;
	private float _temp;
	
	[SerializeField] private Animator animator;
	private static readonly int Eat = Animator.StringToHash("Eat");

	[SerializeField] private TMP_Text text;

	[SerializeField] private HandGunController leftHandController, rightHandController;
	
	private void Start()
	{
		_playerControl = FindObjectOfType<PlayerControl>();

		_collider = GetComponent<Collider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_collider.enabled = false;

			animator.SetTrigger(Eat);

			//leftHandController.canShootAtObstacleHead = true;
			for (int i = 0; i < leftHandController.myAmmo.Count; i++)
			{
				leftHandController.myAmmo[i].GetComponent<Collectible>().mouth = mouthObject;
			}
			//rightHandController.canShootAtObstacleHead = true;
			for (int i = 0; i < rightHandController.myAmmo.Count; i++)
			{
				rightHandController.myAmmo[i].GetComponent<Collectible>().mouth = mouthObject;
			}
		}
	}
}
