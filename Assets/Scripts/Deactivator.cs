using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
	private void OnEnable()
	{
		GameEvents.Ge.aimModeSwitch += Adios;
	}


	private void OnDisable()
	{
		GameEvents.Ge.aimModeSwitch -= Adios;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Solids") || other.CompareTag("Liquids"))
			other.gameObject.SetActive(false);
	}

	private void Adios()
	{
		gameObject.SetActive(false);
	}
}
