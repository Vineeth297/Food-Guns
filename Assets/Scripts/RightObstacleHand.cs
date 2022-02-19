using UnityEngine;

public class RightObstacleHand : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject palmObject;
	
	private static readonly int Eat = Animator.StringToHash("Drink"); 
	public HandGunController leftHand, rightHand;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Liquids"))
		{
			if (GameManager.Singleton.myRightCollectibles < 1)
				GameManager.Singleton.lostPanel.SetActive(true);
			else
			{
				EatMethod(other.gameObject);
				GameManager.Singleton.myRightCollectibles =
					other.GetComponent<Collectible>().handGunController.myAmmo.Count;
			}
		}

		if (other.CompareTag("Solids"))
		{
			other.transform.parent = null;
			other.GetComponent<Rigidbody>().useGravity = true;	
			other.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	private void EatMethod(GameObject snack)
	{
		snack.GetComponent<Collectible>().ammoToFollow = snack.transform;
		GetComponent<Collider>().enabled = false;

		GameObject theSnack = snack;
		var index = rightHand.myAmmo.IndexOf(theSnack);
		GameObject prevSnack = rightHand.myAmmo[index - 1];

		theSnack.GetComponent<Collectible>().doSnake = false;
		theSnack.transform.position = palmObject.transform.position;
		theSnack.transform.SetParent(palmObject.transform);

		if (rightHand.myAmmo.Count > index) 
			UnlinkSnackChain(index,theSnack);

		theSnack.GetComponent<Collider>().enabled = false;
		animator.SetTrigger(Eat);
		theSnack.GetComponent<Collectible>().enabled = false;
	}

	private void UnlinkSnackChain(int index, GameObject theSnack)
	{
		var component = theSnack.GetComponent<Collectible>();
		var count = rightHand.myAmmo.Count;
		
		for (int i = index; i < count; i++)
		{
			var ammoPos = rightHand.myAmmo[index].transform.position;
			rightHand.myAmmo[index].GetComponent<Collectible>().enabled = false;
			var obj = rightHand.myAmmo[index];
			rightHand.myAmmo.RemoveAt(index);
		}
	}
}
