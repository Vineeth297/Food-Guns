using System;
using System.Collections;
using UnityEngine;

public class EatingScript : MonoBehaviour
{
	[SerializeField] private ParticleSystem splash;

	private Transform _hungryHuman;
	
	private void Start() => _hungryHuman = transform.root;

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Solids") && !other.CompareTag("Liquids")) return;
		
		splash.Play();

		if (GameManager.Singleton.myLeftCollectibles + GameManager.Singleton.myRightCollectibles != 0) return;

		StartCoroutine(WinScreen());

	}

	private IEnumerator WinScreen()
	{
		_hungryHuman.GetComponent<HungryHuman>().Eat();

		yield return new WaitForSeconds(1.5f);
		
		GameManager.Singleton.winPanel.SetActive(true);
	}
}
