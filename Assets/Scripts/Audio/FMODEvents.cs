using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Test SFX")]
    [field: SerializeField] public EventReference testSFXEvent;

    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null)
        {
            Debug.LogError($"More than one instance of {Instance.name} detected! GameObject: {name}. Instance has been deleted.");
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
}
