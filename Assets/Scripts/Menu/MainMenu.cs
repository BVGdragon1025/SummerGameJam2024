using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private ScenesData _scenesData;
	[SerializeField] private GameObject _resetManager;
	AsyncOperation _asyncSceneLoad = null;

	public void Play()
	{
		if(Time.timeScale <= 0)
		{
			Time.timeScale = 1;
		}
		SceneManager.LoadScene(GetRandomScene());
		_scenesData.scenesPlayed++;
	}

	public void PlayAsync(GameObject skipButton, VideoPlayer videoPlayer)
	{
        if (Time.timeScale <= 0)
        {
            Time.timeScale = 1;
        }

		StartCoroutine(ShowSkipButton(skipButton, videoPlayer));

    }

	IEnumerator ShowSkipButton(GameObject skipButtonObject, VideoPlayer videoPlayer)
	{
		yield return null;

		_asyncSceneLoad = SceneManager.LoadSceneAsync(GetRandomScene());
		_asyncSceneLoad.allowSceneActivation = false;


		while (!_asyncSceneLoad.isDone)
		{
            if (_asyncSceneLoad.progress >= 0.9f)
            {
				if(videoPlayer.isPlaying)
					skipButtonObject.SetActive(true);

				if(videoPlayer.time >= videoPlayer.clip.length - 4.0f)
                    skipButtonObject.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Space) && _asyncSceneLoad.allowSceneActivation != true)
                {
                    skipButtonObject.SetActive(false);
                    videoPlayer.Stop();
                    _asyncSceneLoad.allowSceneActivation = true;
                    _scenesData.scenesPlayed++;
                }
            }

			yield return null;

        }

    }

	public void PlayNextSceneAsync()
	{
		_asyncSceneLoad.allowSceneActivation = true;
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
		_scenesData.scenesPlayed++;
    }

	public void StopMusicInMenu()
	{
		AudioManager.Instance.StopEvent(FMODEvents.Instance.menuMusic);
	}

}
