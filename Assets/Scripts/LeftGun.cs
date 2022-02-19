using UnityEngine;
using DG.Tweening;

public class LeftGun : MonoBehaviour
{
	private Tweener _tweener1,_tweener2;
	private Vector3 _initScale;
	
	private void OnEnable()
	{
		_initScale = transform.localScale;
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(transform.DOScale(_initScale + (_initScale * 0.2f), 0.3f).SetEase(Ease.OutElastic));
		mySequence.Append(transform.DOScale(_initScale ,0.1f));
	}

	public void PickUpReaction()
	{
		if (_tweener1.IsActive())
		{
			_tweener1.Kill();
			_tweener2.Kill();
			transform.localScale = _initScale;
		}

		transform.DOScale(_initScale + (_initScale * 0.2f), 0.5f).SetEase(Ease.OutElastic).OnUpdate(() =>
		{
			transform.DOScale(_initScale, 0.5f).SetEase(Ease.OutQuint);
		});
	}	
}
