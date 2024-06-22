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
    [SerializeField, Tooltip("Current amount of Plague this structure has, in normal units.")]
    private float _currentPlague;
    public float CurrentPlague { get { return _currentPlague; } set { _currentPlague = value; } }
    [SerializeField, Tooltip("Time before this structure dies from Plague, in seconds")]
    private float _maxPlagueTime;
    public float MaxPlagueTime { get { return _maxPlagueTime; } set { _maxPlagueTime += value; } }
    public bool hasFinished;

    [Header("Other Data")]
    public GameObject healthyState;
    public GameObject infectedState;
    [FormerlySerializedAs("plagueState")]
    public GameObject plagueGameObject;

    protected GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        hasFinished = false;
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
        hasFinished = false;
        //Debug.Log($"Production start! Resource type: {buildingType}, time: {time}");
        yield return new WaitForSeconds(time);
        hasFinished = true;
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
                _currentPlague = 0.0f;
                if(!hasFinished)
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
