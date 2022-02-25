using System;
using System.Collections;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class EatingScript : MonoBehaviour
{
	[SerializeField] private ParticleSystem splash;

	private SoundManager _sound;

	private Transform _hungryHuman;
	
	private void Start()
	{
		_hungryHuman = transform.root;
		_sound = SoundManager.Singleton;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			StartCoroutine(WinScreen());

		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Solids") && !other.CompareTag("Liquids")) return;
		
		//print("Triggered " + other.gameObject);
		splash.Play();
		_sound.PlaySound(_sound.chewingSound);

		if (GameManager.Singleton.myLeftCollectibles + GameManager.Singleton.myRightCollectibles != 0) return;
		
		StartCoroutine(WinScreen());
	}

	private IEnumerator WinScreen()
	{
		//print("Here");
		_sound.PlaySound(_sound.hmmSound);
		_sound.PlaySound(_sound.finalMunchingSound);

		_hungryHuman.GetComponent<HungryHuman>().Eat();

		yield return new WaitForSeconds(1.5f);
		
		GameManager.Singleton.WinPanel();
	}
}
