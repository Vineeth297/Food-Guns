using TMPro;
using UnityEngine;

public class AimModeSwitch : MonoBehaviour
{
	public bool canShoot;

	public GameObject mouthObject;
	public int count = 0;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			//stop player
			other.GetComponent<PlayerControl>().movementSpeed = 0f;
			print(other.GetComponent<PlayerControl>().movementSpeed);
			GetComponent<Collider>().enabled = false;
		}
	}
}
