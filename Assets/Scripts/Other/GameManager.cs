using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level specific data")]
    //Private variables
    [SerializeField]
    private float _maxLvlPlagueValue;
    [SerializeField]
    private float _maxCurrency;
    [SerializeField]
    private float _maxPlayerPlagueValue;
    [SerializeField]
    private LevelData _levelData;

    [Header("Gameplay specific")]
    [SerializeField]
    private float _plagueIncrease;
    [SerializeField, Tooltip("Specifies delay between plague inrcreases, in seconds")]
    private float _plagueIncreaseFrequency;
    [SerializeField, Tooltip("Specifies delay before first Plague Increase on level start, in seconds. 0 - player immediatly gets first batch of Plague")]
    private float _plagueIncreaseDelay;
    public bool isLevelCompleted;


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

        isLevelCompleted = false;

    }

    private void Start()
    {
        _levelData.lvlPlagueValue = _maxLvlPlagueValue;
        _levelData.playerPlagueValue = 0;
        InvokeRepeating(nameof(IncreasePlayerPlague), _plagueIncreaseDelay, _plagueIncreaseFrequency);
    }

    private void Update()
    {
        KillPlayer();

        if (isLevelCompleted && IsInvoking(nameof(IncreasePlayerPlague)))
        {
            CancelInvoke(nameof(IncreasePlayerPlague));
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

    private void IncreasePlayerPlague()
    {
        Debug.Log("Increasing player plague!");
        ChangePlayerPlagueLevel(_plagueIncrease);

    }

    private void KillPlayer()
    {
        if(!isLevelCompleted && (_levelData.playerPlagueValue >= _maxPlayerPlagueValue))
        {
            Debug.Log("Player is dead!");
        }
    }


}
