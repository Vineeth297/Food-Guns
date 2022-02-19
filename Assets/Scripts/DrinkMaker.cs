using UnityEngine;
using DG.Tweening;
using TMPro;

public enum PartsOfDrink
{
	CoolDrink,
	Cap
}

public class DrinkMaker : MonoBehaviour
{
	[SerializeField] private PartsOfDrink partsOfDrink = PartsOfDrink.CoolDrink;
	private int _childNum = 0;

	[SerializeField] private TMP_Text comboText;
	[SerializeField] private ComboText comboComponent;
	
	private float _initFontSize;
	void Start()
	{
		_childNum = (int) partsOfDrink;
		
		comboText.enabled = false;
		_initFontSize = comboText.fontSize;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Liquids")) return;
		
		comboComponent.ComboSelling(comboText,_initFontSize);

		var initScale = other.transform.localScale;
		Sequence mySequence = DOTween.Sequence();

		mySequence.Append(other.transform.DOScale(initScale + (initScale * 0.2f), 1f).SetEase(Ease.OutElastic));
		mySequence.Append(other.transform.DOScale(initScale ,0.1f));
		other.transform.GetChild(_childNum).gameObject.SetActive(true);
		//gameObject.GetComponent<Collider>().enabled = false;

	}
}
