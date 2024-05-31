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
    [SerializeField, Tooltip("Specifies delay between Basic Structure infections, in seconds.")]
    private float _infectTimer;
    public bool isLevelCompleted;
    public List<Building> structures = new();


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
        InvokeRepeating(nameof(SelectBuidingToInfect), _infectTimer, _infectTimer);
    }

    private void Update()
    {
        KillPlayer();

        if (isLevelCompleted && IsInvoking(nameof(IncreasePlayerPlague)) && IsInvoking(nameof(SelectBuidingToInfect)))
        {
            CancelInvoke(nameof(IncreasePlayerPlague));
            CancelInvoke(nameof(SelectBuidingToInfect));
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

    private void SelectBuidingToInfect()
    {
        if(structures.Count > 0)
        {
            int random = Random.Range(0, structures.Count);
            if (!structures[random].isInfected)
            {
                structures[random].isInfected = true;
            }
        }
        else
        {
            Debug.Log("No buildings to infect. Trying once again!");
        }
    }


}
