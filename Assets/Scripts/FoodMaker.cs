using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public enum PartsOfBurger
{
	BottomBun,
	Cheese,
	Tomato,
	Lettuce,
	TopBun
}


public class FoodMaker : MonoBehaviour
{	
	[SerializeField] private PartsOfBurger partsOfBurger = PartsOfBurger.Cheese;
	private int _childNum = 0;
	private void Start()
	{
		_childNum = (int) partsOfBurger;
		
		transform.GetChild(_childNum).gameObject.SetActive(true);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids"))
		{
			SnackMaker(other.gameObject);
		}	
			
		//gameObject.GetComponent<Collider>().enabled = false;
	}


	private void SnackMaker(GameObject item)
	{
		var initScale = item.transform.localScale;
		Sequence mySequence = DOTween.Sequence();

		mySequence.Append(item.transform.DOScale(initScale + (initScale * 0.2f), 1f).SetEase(Ease.OutElastic));
		mySequence.Append(item.transform.DOScale(initScale ,0.1f));
		item.transform.GetChild(_childNum).gameObject.SetActive(true);
	}
}
