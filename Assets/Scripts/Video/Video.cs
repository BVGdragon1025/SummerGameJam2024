using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private GameObject _skipPrompt;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();

        _videoPlayer.Play();
        _mainMenu.PlayAsync(_skipPrompt, _videoPlayer);

        _videoPlayer.loopPointReached += ChangeScene;
    }

    private void ChangeScene(VideoPlayer vp)
    {
        _mainMenu.PlayNextSceneAsync();
    }
}
