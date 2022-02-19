using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Singleton;
	[SerializeField] private GameObject crossHair;

	public GameObject winPanel;
	public GameObject lostPanel;

	public int myLeftCollectibles, myRightCollectibles;
	private void OnEnable()
	{
		GameEvents.Ge.aimModeSwitch += OnAimCanvas;
	}

	private void OnDisable()
	{
		GameEvents.Ge.aimModeSwitch -= OnAimCanvas;
	}

	private void Awake()
	{
		if (!Singleton)
			Singleton = this;
		else 
			Destroy(gameObject);
	}

	private void OnAimCanvas()
	{
		crossHair.SetActive(true);
	}

	public void ReloadButton()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
