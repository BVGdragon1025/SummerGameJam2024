using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.InitializeEvent(FMODEvents.Instance.menuMusic);
    }

    public void PlayClick()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.clickSFX, transform.position);
    }

    public void PlayHover()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hoverSFX, transform.position);
    }

}
