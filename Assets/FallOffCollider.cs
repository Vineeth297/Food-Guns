
using UnityEngine;

public class FallOffCollider : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Solids") && !other.CompareTag("Liquids")) return;
						
		other.GetComponent<Collectible>().handGunController.myAmmo.Remove(other.gameObject);
		if (other.CompareTag("Solids"))
		{
			print(other.GetComponent<Collectible>().handGunController);
			other.GetComponent<Collectible>().handGunController.totalLeftCollectibles =
				other.GetComponent<Collectible>().handGunController.myAmmo.Count;
		}
		else
		{
			print(other.GetComponent<Collectible>().handGunController);
			other.GetComponent<Collectible>().handGunController.totalRightCollectibles =
				other.GetComponent<Collectible>().handGunController.myAmmo.Count;
		}
		other.gameObject.SetActive(false);
	}	
}
