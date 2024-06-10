using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private ArtifactSO _artifactsData;
    [SerializeField] private ScenesData _scenesData;

    private void Awake()
    {
        ResetLevelData();
        ResetArtifacts();
        ResetScenes();
    }

    private void ResetLevelData()
    {
        Debug.LogWarning("Reseting all Level Data!");

        _levelData.currency = _levelData.DefaultCurrency;
        _levelData.infectionFrequency = _levelData.DefaultInfectionFrequency;
        _levelData.isObeliskUnlocked = false;
        _levelData.isRostrumUnlocked = false;
        _levelData.isStonehengeUnlocked = false;
        _levelData.maxBuildingInRange = _levelData.DefaultMaxBuildingInRange;
        _levelData.lvlPlagueValue = _levelData.DefaultLvlPlagueValue;
        _levelData.plagueIncreaseFrequency = _levelData.DefaultPlagueIncreaseFrequency;
        _levelData.playerPlagueValue = _levelData.DefaultPlayerPlagueValue;
        _levelData.playerSpeed = _levelData.DefaultPlayerSpeed;
    }

    private void ResetArtifacts()
    {
        Debug.LogWarning("Reseting all Artifacts!");

        if (_artifactsData.forestArtifacts.Count > 0)
        {
            _artifactsData.forestArtifacts.Clear();
        }
        _artifactsData.forestArtifacts = new(_artifactsData.DefaultForestArtifacts);

        if(_artifactsData.playerArtifacts.Count > 0)
        {
            _artifactsData.playerArtifacts.Clear();
        }
        _artifactsData.playerArtifacts = new(_artifactsData.DefaultPlayerArtifacts);
    }

    private void ResetScenes()
    {
        Debug.LogWarning("Reseting scenes order!");
        if(_scenesData.scenesList.Count > 0)
        {
            _scenesData.scenesList.Clear();
        }
        _scenesData.scenesList = new(_scenesData.defaultSceneList);

        _scenesData.scenesPlayed = 0;

    }

}
