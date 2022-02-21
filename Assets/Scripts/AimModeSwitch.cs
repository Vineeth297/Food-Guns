using UnityEngine;

public class AimModeSwitch : MonoBehaviour
{
	public bool canShoot;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			//stop player
			GameEvents.Ge.InvokeOnAimModeSwitch();
		}
	}
}
