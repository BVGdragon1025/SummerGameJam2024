using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGateController : MonoBehaviour
{
    [SerializeField]
    private GameObject _infectedGate;
    [SerializeField]
    private GameObject _healedGate;

    private void OnEnable()
    {
        GameManager.OnLevelFinished += UnlockEndGate;
    }

    private void OnDisable()
    {
        GameManager.OnLevelFinished -= UnlockEndGate;
    }

    private void UnlockEndGate()
    {
        _infectedGate.SetActive(false);
        _healedGate.SetActive(true);
    }

}
