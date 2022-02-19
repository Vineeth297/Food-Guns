using UnityEngine;
public class LeftObstacleHand : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject palmObject;
	
	private static readonly int Eat = Animator.StringToHash("Drink"); 
	public HandGunController leftHand, rightHand;

	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids"))
		{
			if (GameManager.Singleton.myLeftCollectibles < 1)
				GameManager.Singleton.lostPanel.SetActive(true);
			else
			{
				EatMethod(other.gameObject);
				GameManager.Singleton.myLeftCollectibles =
					other.GetComponent<Collectible>().handGunController.myAmmo.Count;
			}
		}

		if (other.CompareTag("Liquids"))
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
		var index = leftHand.myAmmo.IndexOf(theSnack);
		GameObject prevSnack = leftHand.myAmmo[index - 1];

		theSnack.GetComponent<Collectible>().doSnake = false;
		theSnack.transform.position = palmObject.transform.position;
		//rightHand.myAmmo.RemoveAt(index);
		theSnack.transform.SetParent(palmObject.transform);

		if (leftHand.myAmmo.Count > index) 
			UnlinkSnackChain(index,theSnack);

		theSnack.GetComponent<Collider>().enabled = false;
		animator.SetTrigger(Eat);
		theSnack.GetComponent<Collectible>().enabled = false;
		
		//theSnack.SetActive(false);
	}

	private void UnlinkSnackChain(int index, GameObject theSnack)
	{
		var component = theSnack.GetComponent<Collectible>();
		var count = leftHand.myAmmo.Count;

		for (int i = index; i < count; i++)
		{
			var ammoPos = leftHand.myAmmo[index].transform.position;
			//leftHand.myAmmo[index].transform.DOJump(new Vector3(ammoPos.x + Random.Range(0f, 1f), ammoPos.y, ammoPos.x + Random.Range(0f, 1f)), 5f, 1, 1f);

			leftHand.myAmmo[index].GetComponent<Collectible>().enabled = false;
			//leftHand.myAmmo[index].GetComponent<Rigidbody>().isKinematic = false;
			var obj = leftHand.myAmmo[index];
			leftHand.myAmmo.RemoveAt(index);
			//obj.transform.DOJump(new Vector3(ammoPos.x + Random.Range(0f, 1f), ammoPos.y, ammoPos.x + Random.Range(0f, 1f)), 1f, 1, 1f);
		}
	}
}
