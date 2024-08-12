using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    private Slider _slider;
    [SerializeField]
    private BuildingType _buildingType;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        GameManager.OnValueChange += SetSliderValue;
    }

    private void OnDisable()
    {
        GameManager.OnValueChange -= SetSliderValue;
    }

    private void SetSliderValue(float value, BuildingType buildingType)
    {
        if(_buildingType == buildingType)
        {
            _slider.value = value;
        }
    }

    public void SetText(TextMeshProUGUI text)
    {
        text.SetText($"{_slider.value}");
    }

}
