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
			//pickup the triggered snack
			EatMethod(other.gameObject);
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
		
		print(snack.name);
		snack.GetComponent<Collectible>().ammoToFollow = snack.transform;

		GetComponent<Collider>().enabled = false;

		GameObject theSnack = snack;
		var index = rightHand.myAmmo.IndexOf(theSnack);
		GameObject prevSnack = rightHand.myAmmo[index - 1];

		theSnack.GetComponent<Collectible>().doSnake = false;
		theSnack.transform.position = palmObject.transform.position;

		//rightHand.myAmmo.RemoveAt(index);
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
		print("Out Index " + index);
		for (int i = index; i < count; i++)
		{
			var ammoPos = rightHand.myAmmo[index].transform.position;
			//rightHand.myAmmo[index].transform.DOJump(new Vector3(ammoPos.x + Random.Range(0f, 1f), ammoPos.y, ammoPos.x + Random.Range(0f, 1f)), 5f, 1, 1f);
			print("In Index " + index);
			rightHand.myAmmo[index].GetComponent<Collectible>().enabled = false;
			//rightHand.myAmmo[index].GetComponent<Rigidbody>().isKinematic = false;
			// rightHand.myAmmo.RemoveAt(index);
			var obj = rightHand.myAmmo[index];
			rightHand.myAmmo.RemoveAt(index);
			//obj.transform.DOJump(new Vector3(ammoPos.x + Random.Range(0f, 1f), ammoPos.y, ammoPos.x + Random.Range(0f, 1f)), 1f, 1, 1f);
		}
	}
}
