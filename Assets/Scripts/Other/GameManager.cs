using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Level Specific
    [Header("Level specific data")]
    //Private variables
    [SerializeField]
    private float _currentLvlPlagueValue;
    [SerializeField]
    private float _currentCurrency;
    [SerializeField]
    private float _currentPlayerPlagueValue;
    [SerializeField, Tooltip("Amount of currency player starts with. Value can be decimal.")]
    private float _startingCurrency;
    [SerializeField]
    private LevelData _levelData;
    [SerializeField, Tooltip("Gives player additional Nature Points per second. Value can be decimal.")]
    private float _additionalCurrency;
    #endregion

    #region Getters/Setters
    public float MaxLvlPlagueValue { get { return _levelData.lvlPlagueValue; } set { _levelData.lvlPlagueValue += value; } }
    public float MaxPlayerPlagueValue { get { return _levelData.playerPlagueValue; } set { _levelData.playerPlagueValue += value; } }
    public float MaxPlayerCurrency { get { return _levelData.currency; } set { _levelData.currency += value; } }
    public float CurrentLvlPlague { get { return _currentLvlPlagueValue; } }
    public float CurrentCurrency { get { return _currentCurrency; } }
    public float CurrentPlayerPlague { get { return _currentPlayerPlagueValue; } }
    #endregion

    #region Special Structures Buttons
    [Header("Special Structures Buttons")]
    [SerializeField] private GameObject _obeliskButton;
    [SerializeField] private GameObject _stonehengeButton;
    [SerializeField] private GameObject _rostrumButton;
    #endregion

    #region Gameplay Specific
    [Header("Gameplay specific")]
    [SerializeField, FormerlySerializedAs("_plagueIncrease"), Tooltip("Specifies how much Plague player receives on frequent basis.")]
    private float _plagueIncreaseValue;
    public float PlagueIncrease { get { return _levelData.plagueIncreaseFrequency;  } set { _levelData.plagueIncreaseFrequency /= value; } }
    [SerializeField, Tooltip("Specifies delay before first Plague Increase on level start, in seconds. 0 - player immediatly gets first batch of Plague")]
    private float _plagueIncreaseDelay;
    public float InfectionFrequency { get { return _levelData.infectionFrequency;  }  set { _levelData.infectionFrequency -= value; } }
    public float PlayerSpeed { get { return _levelData.playerSpeed; } set { _levelData.playerSpeed += (_levelData.playerSpeed *  value); } }
    public int MaxBuildingLimit { get { return _levelData.maxBuildingInRange; } set { _levelData.maxBuildingInRange += value; } }
    public bool isLevelCompleted;
    public List<Building> structures = new();
    public int buildingsInfected;
    [SerializeField] private GameObject _exitTrigger;
    [SerializeField] private GameObject _infectedGate;
    #endregion

    #region Sliders
    [Header("Sliders")]
    [SerializeField]
    private Slider _forestPlagueSlider;
    [SerializeField]
    private Slider _currencySlider;
    [SerializeField]
    private Slider _playerPlagueSlider;
    #endregion

    #region Game Over Region
    [Header("Game Over Screens")]
    [SerializeField]
    private GameObject _victory;
    [SerializeField]
    private GameObject _death;
    #endregion

    #region Other
    public LevelData LevelData { get { return _levelData; } }
    public static GameManager Instance;
    private AudioManager _audioManager;
    #endregion

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

        SetupGame();
        Time.timeScale = 1.0f;

    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _audioManager.SetPublicVariable("Forest_State", 0.0f);

        InvokeRepeating(nameof(IncreasePlayerPlague), _plagueIncreaseDelay, _levelData.plagueIncreaseFrequency);
        InvokeRepeating(nameof(SelectBuidingToInfect), _levelData.infectionFrequency, _levelData.infectionFrequency);
    }

    private void Update()
    {
        KillPlayer();

        if (isLevelCompleted)
        {
            CancelInvoke();
            YouWin();
        }

        ChangePoisonPlayerAmbient();
        UpdateSliders();

        if(_currentLvlPlagueValue <= 0)
        {
            isLevelCompleted = true;
            _audioManager.SetPublicVariable("Forest_State", 1.0f);
        }

    }

    public void ChangeForestPlagueLevel(float value)
    {
        _currentLvlPlagueValue = Mathf.Clamp(_currentLvlPlagueValue + value, 0, _levelData.lvlPlagueValue);
    }

    public void ChangeCurrencyValue(float value)
    {
        _currentCurrency = Mathf.Clamp(_currentCurrency + value, 0, _levelData.currency);
    }

    public void ChangePlayerPlagueLevel(float value)
    {
        _currentPlayerPlagueValue = Mathf.Clamp(_currentPlayerPlagueValue + value, 0, _levelData.playerPlagueValue);
    }

    private void IncreasePlayerPlague()
    {

        Debug.Log("Increasing player plague!");
        
        ChangePlayerPlagueLevel(_plagueIncreaseValue);
        

    }

    private void KillPlayer()
    {
        if(!isLevelCompleted && (_currentPlayerPlagueValue >= _levelData.playerPlagueValue))
        {
            Debug.Log("Player is dead!");
            GameOver();
        }
    }

    private void SelectBuidingToInfect()
    {
        if(structures.Count > 0)
        {
            int random = Random.Range(0, structures.Count);
            if (structures[random].PlagueState == PlagueState.Healthy)
            {
                Debug.Log($"Building infected: {structures[random].name}");
                structures[random].ChangePlagueState(PlagueState.Infected);
                _audioManager.PlayOneShot(FMODEvents.Instance.structureInfected, structures[random].transform.position);
                buildingsInfected += 1;
            }

        }

    }

    private void ChangePoisonPlayerAmbient()
    {
        float plagueLvl = _currentPlayerPlagueValue / _levelData.lvlPlagueValue;
        _audioManager.SetPublicVariable("Player_Infection", plagueLvl);
    }

    public float Timer(float timerValue, float timeLimit)
    {
        if(timerValue <= timeLimit)
        {
            timerValue += Time.deltaTime;

        }

        return timerValue / timeLimit;

    }

    private void SetupGame()
    {
        isLevelCompleted = false;
        PrepareButtons();

        _forestPlagueSlider.minValue = 0;
        _forestPlagueSlider.maxValue = _levelData.lvlPlagueValue;
        _currencySlider.maxValue = _levelData.currency;
        _playerPlagueSlider.minValue = 0;
        _playerPlagueSlider.maxValue = _levelData.playerPlagueValue;

        _currentLvlPlagueValue = _levelData.lvlPlagueValue;
        _currentPlayerPlagueValue = 0;
        _currentCurrency = _startingCurrency;
    }

    public float ResetTimer()
    {
        float timerValue = 0.0f;
        return timerValue;
    }

    public void UpdateSliders()
    {
        _forestPlagueSlider.value = _currentLvlPlagueValue;
        _currencySlider.value = _currentCurrency;
        _playerPlagueSlider.value = _currentPlayerPlagueValue;
    }

    private void PrepareButtons()
    {
        UnlockSpecialStructure(_obeliskButton, _levelData.isObeliskUnlocked);
        UnlockSpecialStructure(_stonehengeButton, _levelData.isStonehengeUnlocked);
        UnlockSpecialStructure(_rostrumButton, _levelData.isRostrumUnlocked);
    }

    private void UnlockSpecialStructure(GameObject structureButton, bool isUnlocked)
    {
        structureButton.SetActive(isUnlocked);
    }

    private void GameOver()
    {
        Cursor.visible = true;
        _death.SetActive(true);
        Time.timeScale = 0;
    }

    private void YouWin()
    {
        if (isLevelCompleted)
        {
            if (!_exitTrigger.activeInHierarchy)
            {
                _exitTrigger.SetActive(true);
                _infectedGate.SetActive(false);
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
