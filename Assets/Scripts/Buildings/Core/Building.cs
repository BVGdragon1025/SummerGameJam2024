using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BuildingManager))]
public abstract class Building : MonoBehaviour
{
    [Header("Building Data"), Tooltip("Resource values and other things")]
    [SerializeField]
    protected string buildingName;
    public string BuildingName {  get { return buildingName; } }
    [SerializeField]
    protected BuildingType buildingType;
    public BuildingType BuildingType { get { return buildingType; } }
    [SerializeField]
    protected PlagueState plagueState;
    public PlagueState PlagueState { get { return plagueState; } }
    [SerializeField]
    protected float buildingCost = 0;
    public float BuildingCost { get { return buildingCost; } }
    [SerializeField, Tooltip("The amount of resource this building gives")]
    protected float resourceAmount;
    public float Currency { get { return resourceAmount; } set { resourceAmount += value; } }
    [SerializeField, Tooltip("Rate at which this buiilding spawns it's resource, in seconds"), FormerlySerializedAs("_spawnRate")]
    protected float spawnRate;
    public float SpawnRate { get { return spawnRate; } set { spawnRate -= (spawnRate * value); } }
    public TextMeshPro timerText;
    [SerializeField, Tooltip("Time before this structure dies from Plague, in seconds")]
    private float _maxPlagueTime;
    public float MaxPlagueTime { get { return _maxPlagueTime; } set { _maxPlagueTime += value; } }
    [SerializeField, Tooltip("Time before this structure is healed from infection")]
    private float _maxHealingTime;
    public float MaxHealingTime { get { return _maxHealingTime; } set { _maxHealingTime += value; } }
    private bool _hasFinished;
    public bool HasFinished { get { return _hasFinished; } }

    [Header("Other Data")]
    public Material healthyMaterial;
    public Material infectedMaterial;
    public Material plagueMaterial;
    public GameObject healthyState;
    public GameObject infectedState;
    [FormerlySerializedAs("plagueState")]
    public GameObject plagueGameObject;
    private Renderer _renderer;

    protected GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        _hasFinished = false;
        _renderer = GetComponent<Renderer>();
        if(CompareTag("Building"))
            ChangePlagueState(PlagueState.Healthy);
    }

    private void Start()
    {
        //Special check for static structures (those already placed on level)
        if(gameManager == null)
        {
            gameManager = GameManager.Instance;
        }
    }

    public bool HasCurrency()
    {
        if(gameManager.CurrentCurrency < buildingCost)
        {
            return false;
        }
        return true;
    }


    public IEnumerator StartProduction(float time)
    {
        _hasFinished = false;
        //Debug.Log($"Production start! Resource type: {buildingType}, time: {time}");
        yield return new WaitForSeconds(time);
        _hasFinished = true;
        //Debug.Log("Production stop!");
    }

    public void ChangePlagueState(PlagueState state)
    {
        switch (state)
        {
            case PlagueState.Healthy:
                healthyState.SetActive(true);
                infectedState.SetActive(false);
                plagueGameObject.SetActive(false);
                timerText.gameObject.SetActive(true);
                if(!_hasFinished)
                    ResetProduction();
                break;
            case PlagueState.Infected:
                infectedState.SetActive(true);
                healthyState.SetActive(false);
                plagueGameObject.SetActive(false);
                timerText.gameObject.SetActive(false);
                StopAllCoroutines();
                break;
            case PlagueState.Healing:

                break;
            default:
                Debug.LogWarning($"{name} encountered unknown state! State name: {state}");
                break;
        }
        plagueState = state;

    }

    public abstract void GiveResource();

    public abstract void ResetProduction();


}
