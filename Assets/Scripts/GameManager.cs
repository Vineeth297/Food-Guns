using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject crossHair;

	private void OnEnable()
	{
		GameEvents.Ge.aimModeSwitch += OnAimCanvas;
	}

	private void OnDisable()
	{
		GameEvents.Ge.aimModeSwitch -= OnAimCanvas;
	}

	private void OnAimCanvas()
	{
		crossHair.SetActive(true);
	}

	public void ReloadButton()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
