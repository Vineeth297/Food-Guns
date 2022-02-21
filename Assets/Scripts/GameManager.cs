using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Singleton;
	[SerializeField] private GameObject crossHair;

	public GameObject winPanel;
	public GameObject lostPanel;
	
	public TMP_Text levelNumber;

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

	private void Start()
	{
		levelNumber.text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1);
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
		if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCount)
		{
			SceneManager.LoadScene(Random.Range(1, SceneManager.sceneCount));
		}
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void WinPanel()
	{
		levelNumber.enabled = false;
		winPanel.SetActive(true);
	}
}
