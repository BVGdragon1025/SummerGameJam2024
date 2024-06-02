using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player Footsteps")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }

    [field: Header("Test SFX")]
    [field: SerializeField] public EventReference testSFXEvent { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Effects")]
    [field: SerializeField] public EventReference interactions { get; private set; }
    [field: SerializeField] public EventReference structureInteractions { get; private set; }

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
