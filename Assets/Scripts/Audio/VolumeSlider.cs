using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] private VolumeTypeEnum _volumeType;

    private Slider _volumeSlider;

    private void Awake()
    {
        _volumeSlider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (_volumeType)
        {
            case VolumeTypeEnum.MASTER:
                _volumeSlider.value = AudioManager.Instance.masterVolume;
                break;
            case VolumeTypeEnum.MUSIC:
                _volumeSlider.value = AudioManager.Instance.musicVolume;
                break;
            case VolumeTypeEnum.AMBIENCE:
                _volumeSlider.value = AudioManager.Instance.ambienceVolume;
                break;
            case VolumeTypeEnum.SFX:
                _volumeSlider.value = AudioManager.Instance.sfxVolume;
                break;
            default:
                Debug.LogWarning($"Volume type not supported: {_volumeType}!");
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (_volumeType)
        {
            case VolumeTypeEnum.MASTER:
                AudioManager.Instance.masterVolume = _volumeSlider.value;
                break;
            case VolumeTypeEnum.MUSIC:
                AudioManager.Instance.musicVolume = _volumeSlider.value;
                break;
            case VolumeTypeEnum.AMBIENCE:
                AudioManager.Instance.ambienceVolume = _volumeSlider.value;
                break;
            case VolumeTypeEnum.SFX:
                AudioManager.Instance.sfxVolume = _volumeSlider.value;
                break;
            default:
                Debug.LogWarning($"Volume type not supported: {_volumeType}!");
                break;
        }
    }

}
