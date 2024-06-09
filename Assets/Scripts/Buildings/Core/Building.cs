using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    protected float buildingCost = 0;
    public float BuildingCost { get { return buildingCost; } }
    [SerializeField, Tooltip("The amount of resource this building gives")]
    protected float resourceAmount;
    public float Currency { get { return resourceAmount; } set { resourceAmount += value; } }
    [SerializeField, Tooltip("Rate at which this buiilding spawns it's resource, in seconds")]
    private float _spawnRate;
    public float SpawnRate { get { return _spawnRate; } set { _spawnRate -= (_spawnRate * value); } }
    public TextMeshPro timerText;
    [SerializeField, Tooltip("Current amount of Plague this structure has, in normal units.")]
    private float _currentPlague;
    public float CurrentPlague { get { return _currentPlague; } set { _currentPlague = value; } }
    [SerializeField, Tooltip("Time before this structure dies from Plague, in seconds")]
    private float _maxPlagueTime;
    public float MaxPlagueTime { get { return _maxPlagueTime; } set { _maxPlagueTime += value; } }
    public bool isInfected;
    public bool hasPlague;
    public bool hasFinished;

    [Header("Other Data")]
    public int elementsInTrigger;
    public GameObject triggerGameObject;
    public GameObject healthyState;
    public GameObject infectedState;
    public GameObject plagueState;

    protected GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        hasFinished = false;
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

    public abstract void GiveResourceToPlayer();


}
