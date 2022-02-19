using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
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

	[SerializeField] private TMP_Text comboText;
	[SerializeField] private ComboText comboComponent;

	private float _initFontSize;
	
	private void Start()
	{
		_childNum = (int) partsOfBurger;
		
		transform.GetChild(_childNum).gameObject.SetActive(true);

		//_textScale = text.transform.localScale;
		
		comboText.enabled = false;
		_initFontSize = comboText.fontSize;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids"))
		{
			SnackMaker(other.gameObject);
			comboComponent.ComboSelling(comboText,_initFontSize);
		}	
			
		//gameObject.GetComponent<Collider>().enabled = false;
	}
	
	private void SnackMaker(GameObject item)
	{
		var initScale = item.transform.localScale;
		Sequence mySequence = DOTween.Sequence();

		mySequence.Append(item.transform.DOScale(initScale + (initScale * 0.2f), 1f).SetEase(Ease.OutElastic));
		mySequence.Append(item.transform.DOScale(initScale ,0.1f));
		//item.transform.GetChild(_childNum).gameObject.SetActive(true);
	//Enum.GetNames(typeof(PartsOfBurger)).Length
		for (int i = 1; i <= _childNum; i++)
		{
			item.transform.GetChild(i).gameObject.SetActive(true);
		}
	}
}
