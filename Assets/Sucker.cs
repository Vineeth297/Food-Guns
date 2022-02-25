using DG.Tweening;
using UnityEngine;

public class Sucker : MonoBehaviour
{
	public GameObject suckerDestination;
	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out Collectible collectible))
			if(collectible.isPickedUp) return;
		
		if (other.CompareTag("Solids"))
		{
			other.transform.DOMove(suckerDestination.transform.position, 0.25f);
		}
	}
}
