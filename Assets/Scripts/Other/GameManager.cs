using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField, Tooltip("Gives player additional Nature Points per second. Value can be decimal.")]
    private float _additionalCurrency;

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
    public int buildingsInfected;

    [Header("Sliders")]
    [SerializeField]
    private Slider _forestPlagueSlider;
    [SerializeField]
    private Slider _currencySlider;
    [SerializeField]
    private Slider _playerPlagueSlider;

    public LevelData LevelData { get { return _levelData; } }
    public static GameManager Instance;
    private AudioManager _audioManager;

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

        _forestPlagueSlider.maxValue = _maxLvlPlagueValue;
        _currencySlider.maxValue = _maxCurrency;
        _playerPlagueSlider.maxValue = _maxPlayerPlagueValue;

    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _levelData.lvlPlagueValue = _maxLvlPlagueValue;
        _levelData.playerPlagueValue = 0;
        InvokeRepeating(nameof(IncreasePlayerPlague), _plagueIncreaseDelay, _plagueIncreaseFrequency);
        InvokeRepeating(nameof(SelectBuidingToInfect), _infectTimer, _infectTimer);
        InvokeRepeating(nameof(GiveSomePoints), 5.0f, 1.0f);
    }

    private void Update()
    {
        KillPlayer();

        if (isLevelCompleted && IsInvoking(nameof(IncreasePlayerPlague)) && IsInvoking(nameof(SelectBuidingToInfect)))
        {
            CancelInvoke(nameof(IncreasePlayerPlague));
            CancelInvoke(nameof(SelectBuidingToInfect));
            CancelInvoke(nameof(GiveSomePoints));
        }

        ChangePoisonPlayerAmbient();
        ChangePlagueState();

        if(_levelData.lvlPlagueValue <= 0)
        {
            isLevelCompleted = true;
            _audioManager.SetPublicVariable("Forest_State", 1.0f);
            _audioManager.SetPublicVariable("Danger_Phase", 0.0f);
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
            if (!structures[random].isInfected && !structures[random].hasPlague)
            {
                Debug.Log($"Building infected: {structures[random].name}");
                structures[random].isInfected = true;
                _audioManager.SetPublicVariable("Infection_State", 1.0f);
                buildingsInfected += 1;
            }

        }

    }

    private void ChangePoisonPlayerAmbient()
    {
        float plagueLvl = _levelData.playerPlagueValue / _maxPlayerPlagueValue;
        _audioManager.SetPublicVariable("Player_Infection", plagueLvl);
    }

    private void ChangePlagueState()
    {
        if(buildingsInfected == 0 || buildingsInfected == structures.Count)
        {
            _audioManager.SetPublicVariable("Infection_State", 0.0f);
        }
    }

    public float Timer(float timerValue, float timeLimit)
    {
        if(timerValue <= timeLimit)
        {
            timerValue += Time.deltaTime;

        }

        return timerValue / timeLimit;

    }

    public float ResetTimer()
    {
        float timerValue = 0.0f;
        return timerValue;
    }

    public void GiveSomePoints()
    {
        //Debug.LogFormat($"Some nature points! <color=#00ff00ff>{_additionalCurrency} pts!</color>");
        ChangeCurrencyValue(_additionalCurrency);
    }

    public void UpdateSliders()
    {
        _forestPlagueSlider.value = _levelData.lvlPlagueValue;
        _currencySlider.value = _levelData.currency;
        _playerPlagueSlider.value = _levelData.playerPlagueValue;
    }

}
