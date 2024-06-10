using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private ScenesData _scenesData;
	[SerializeField] private GameObject _resetManager;

	public void Play()
	{
		if(Time.timeScale <= 0)
		{
			Time.timeScale = 1;
		}
		SceneManager.LoadScene(GetRandomScene());
		_scenesData.scenesPlayed++;
	}
	public void GoBackToMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(0);
	}
	
	public void Quit()
	{
		Application.Quit();
		Debug.Log("Player Has Quit The Game");
	}

	private string GetRandomScene()
	{
		int random = Random.Range(0, _scenesData.scenesList.Count);
		string sceneName = _scenesData.scenesList[random];
		_scenesData.scenesList.Remove(sceneName);
		return sceneName;
	}

	public void StartOver()
	{
		_resetManager.SetActive(true);

		SceneManager.LoadScene(GetRandomScene());
    }

}
