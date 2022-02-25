using DG.Tweening;
using UnityEngine;

public class RightGunSucker : MonoBehaviour
{
	public GameObject suckerDestination;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Liquids"))
		{
			other.transform.DOMove(suckerDestination.transform.position, 0.25f);
		}
	}
}
