using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EndTransitionController : MonoBehaviour
{
    [SerializeField] private ScenesData _scenesData;
    [SerializeField, Tooltip("Specifies delay between level finish and End Game Screen")]
    private float _endScreenDelay;

    [Header("Mood Section")]
    [SerializeField] private Color32 _defaultColor;
    [SerializeField] private Color32 _healedColor;
    [SerializeField] private float _transitionSpeed = 1;
    [SerializeField] private Light _directionalLight;
    [SerializeField] private float _defaultIntensity;
    [SerializeField] private float _healedIntensity;
    [SerializeField] private GameObject _fog;

    [Header("End Level Section")]
    [SerializeField] private GameObject _artifactScreen;
    [SerializeField] private GameObject _victoryScreen;

    private void OnEnable()
    {
        _fog.SetActive(false);
        Debug.Log("Scenes left: " + _scenesData.scenesList.Count);
        StartCoroutine(ChangeMood());
    }

    private IEnumerator ChangeMood()
    {
        float timer = 0;

        while (_directionalLight.color != _healedColor)
        {
            timer += Time.deltaTime * _transitionSpeed;
            _directionalLight.color = Color32.Lerp(_defaultColor, _healedColor, timer);
            _directionalLight.intensity = Mathf.Lerp(_defaultIntensity, _healedIntensity, timer);
            yield return null;
        }

        if (_directionalLight.color == _healedColor && _scenesData.scenesList.Count <= 0)
        {
            Debug.Log("Launching ending!");
            StartCoroutine(TransitionToEndScreen());
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _scenesData.scenesPlayed < _scenesData.defaultSceneList.Count)
        {
            Cursor.visible = true;
            Time.timeScale = 0;
            _artifactScreen.SetActive(true);
        }
    }

    private IEnumerator TransitionToEndScreen()
    {
        Debug.Log("Ending the game!");
        yield return new WaitForSeconds(_endScreenDelay);
        Cursor.visible = true;
        _victoryScreen.SetActive(true);
        Time.timeScale = 0;
        
    }


}
