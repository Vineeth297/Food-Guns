using UnityEngine;
using DG.Tweening;
using UnityEditor.Rendering;

public class LeftGun : MonoBehaviour
{
	private Tweener _tweener1,_tweener2;
	private Vector3 _initScale;
	private Sequence _pickupSequence;

	private void OnEnable()
	{
		_initScale = transform.localScale;
		
		var mySequence = DOTween.Sequence();
		mySequence.Append(transform.DOScale(_initScale + (_initScale * 0.2f), 0.3f).SetEase(Ease.OutElastic));
		mySequence.Append(transform.DOScale(_initScale ,0.15f));
	}

	private void Start()
	{
		_initScale = transform.localScale / 2;
	}

	public void PickUpReaction()
	{
		if (_pickupSequence.IsActive())
		{
			_pickupSequence.Kill();
			//Debug.Break();
		}
		
		_pickupSequence = DOTween.Sequence();

		_pickupSequence.Append(transform.DOScale(_initScale * 1.25f, 0.15f).SetEase(Ease.OutElastic));
		_pickupSequence.Append(transform.DOScale(_initScale ,0.15f));
	}
}
