using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Private variables
    [SerializeField]
    private float _maxLvlPlagueValue;
    [SerializeField]
    private float _maxCurrency;
    [SerializeField]
    private float _maxPlayerPlagueValue;
    [SerializeField]
    private LevelData _levelData;

    //Public variables
    public LevelData LevelData { get { return _levelData; } }
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ChangeForestPlagueLevel(float value)
    {
        _levelData.lvlPlagueValue = Mathf.Clamp(_levelData.lvlPlagueValue + value, 0, _maxLvlPlagueValue);
    }

    public void ChangeCurrencyValue(float value)
    {
        _levelData.currency = Mathf.Clamp(_levelData.currency + value, 0, _maxCurrency);
    }

    public void ChangePlayerPlagueLevel(float value)
    {
        _levelData.playerPlagueValue = Mathf.Clamp(_levelData.playerPlagueValue + value, 0, _maxPlayerPlagueValue);
    }



}
