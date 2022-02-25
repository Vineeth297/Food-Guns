using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboText : MonoBehaviour
{
	private int _sellingCount;
	private Tweener _tweener;
	private void Start()
	{
		_sellingCount = 0;
	}
	
	public void ComboSelling(TMP_Text comboText,float initFontSize)
	{
		comboText.enabled = true;
		_sellingCount += 1;
		comboText.text = "+" + _sellingCount;

		if (_tweener.IsActive())
		{
			_tweener.Kill();
			comboText.fontSize = initFontSize;
		}

		_tweener = comboText.DOFontSize(comboText.fontSize + 5, 0.3f).OnComplete(() =>
		{
			comboText.fontSize = initFontSize;
			comboText.enabled = false;
		});
	}
}
