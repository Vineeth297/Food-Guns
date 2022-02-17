using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;
public class GunSpawnerGate : MonoBehaviour
{
	[SerializeField] private GameObject leftGun, rightGun;
	[SerializeField] private ParticleSystem leftPickupParticle, rightPickupParticle;
	
	private Tweener _tweener1;
	
	private Vector3 _initScaleGun1;
	private Vector3 _initScaleGun2;

	private void Start()
	{
		_initScaleGun1 = leftGun.transform.localScale;
		_initScaleGun2 = rightGun.transform.localScale;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids"))
		{
			PickUpReaction(leftGun, _initScaleGun1,leftPickupParticle);
		}
		if (other.CompareTag("Liquids"))
		{
			PickUpReaction(rightGun,_initScaleGun2,rightPickupParticle);
		}	
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			leftGun.SetActive(false);
			rightGun.SetActive(false);
		}
	}
	
	public void PickUpReaction(GameObject gun,Vector3 initScale,ParticleSystem pickupParticleEffect)
	{
		Sequence mySequence = DOTween.Sequence();
		pickupParticleEffect.Play();
		mySequence.Append(gun.transform.DOScale(initScale + (initScale * 0.2f), 0.05f).SetEase(Ease.OutElastic));
		mySequence.Append(gun.transform.DOScale(initScale ,0.01f));
	}
}
