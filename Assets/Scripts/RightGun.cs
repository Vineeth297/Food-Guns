using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class RightGun : MonoBehaviour
{
	private Tweener _tweener1;
	private Vector3 _initScale;

	private void OnEnable()
	{
		_initScale = transform.localScale;

		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(transform.DOScale(_initScale + (_initScale * 0.2f), 0.3f).SetEase(Ease.OutElastic));
		mySequence.Append(transform.DOScale(_initScale ,0.15f));
	}

	private void Start()
	{
		_initScale = transform.localScale / 2;
	}
	
	public void PickUpReaction()
	{
		Sequence mySequence = DOTween.Sequence();

		mySequence.Append(transform.DOScale(_initScale + (_initScale * 0.25f), 0.1f).SetEase(Ease.OutElastic));
		mySequence.Append(transform.DOScale(_initScale ,0.1f));
	}
}
