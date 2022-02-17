using UnityEngine;
using DG.Tweening;
public enum PartsOfDrink
{
	CoolDrink,
	Cap
}

public class DrinkMaker : MonoBehaviour
{
	[SerializeField] private PartsOfDrink partsOfDrink = PartsOfDrink.CoolDrink;
	private int _childNum = 0;

    // Start is called before the first frame update
    void Start()
	{
		_childNum = (int) partsOfDrink;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Liquids")) return;
		
		var initScale = other.transform.localScale;
		Sequence mySequence = DOTween.Sequence();

		mySequence.Append(other.transform.DOScale(initScale + (initScale * 0.2f), 1f).SetEase(Ease.OutElastic));
		mySequence.Append(other.transform.DOScale(initScale ,0.1f));
		other.transform.GetChild(_childNum).gameObject.SetActive(true);
		//gameObject.GetComponent<Collider>().enabled = false;

	}
}
