using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EndTransitionController : MonoBehaviour
{
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

    private void OnEnable()
    {
        _fog.SetActive(false);
        StartCoroutine(ChangeMood());

    }

    private IEnumerator ChangeMood()
    {
        float timer = 0;

        while(_directionalLight.color != _healedColor)
        {
            timer += Time.deltaTime * _transitionSpeed;
            _directionalLight.color = Color32.Lerp(_defaultColor, _directionalLight.color, timer);
            _directionalLight.intensity = Mathf.Lerp(_defaultIntensity, _healedIntensity, timer * 2);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            _artifactScreen.SetActive(true);
        }
    }


}
