using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingScript : MonoBehaviour
{
	[SerializeField] private ParticleSystem splash;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids") || other.CompareTag("Liquids"))
		{
			splash.Play();
		}
	}
}
