using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Whatever : MonoBehaviour
{
	private Vector3 _initialScale;

	public float scaleFactor = 0.5f;
	[SerializeField] private float maxFrequency;

	public float randomness = 90f;
	public float strength = 1f;
	public int vibrato = 10;
	
	private void Start()
	{
		_initialScale = transform.localScale;
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			/*var vec = new Vector3(_initialScale .x + Mathf.Sin(Time.time * maxFrequency) , _initialScale .y + Mathf.Sin(Time.time * maxFrequency), _initialScale .z + Mathf.Sin(Time.time * maxFrequency));
 
			transform.localScale = vec;*/

			//transform.DOShakeScale(1f, strength, vibrato, randomness);

			transform.DOScale(_initialScale + _initialScale * scaleFactor, 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
			{
				transform.DOScale(_initialScale, 0.1f).SetEase(Ease.OutSine);
			});


		}
	}
}
