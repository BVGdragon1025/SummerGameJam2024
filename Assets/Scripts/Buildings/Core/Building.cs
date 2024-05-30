using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingManager))]
public abstract class Building : MonoBehaviour
{
    [Header("Building Data"), Tooltip("Resource values and other things")]
    [SerializeField]
    private BuildingType _buildingType;
    public BuildingType BuildingType { get { return _buildingType; } }
    [SerializeField]
    protected float buildingCost = 0;
    public float BuildingCost { get { return buildingCost; } }
    [SerializeField, Tooltip("The amount of resource this building gives")]
    protected float resourceAmount;
    public float Currency { get { return resourceAmount; } set { resourceAmount += value; } }
    [SerializeField, Tooltip("Rate at which this buiilding spawns it's resource, in seconds")]
    private float _spawnRate;
    public float SpawnRate { get { return _spawnRate; } set { _spawnRate += (_spawnRate * value); } }
    [SerializeField, Tooltip("Current amount of Plague this structure has, in normal units.")]
    private float _currentPlague;
    public float CurrentPlague { get { return _currentPlague; } set { _currentPlague += value; } }
    [SerializeField, Tooltip("Time before this structure dies from Plague, in seconds")]
    private float _maxPlagueTime;
    public float MaxPlagueTime { get { return _maxPlagueTime; } set { _maxPlagueTime += value; } }
    public bool hasPlague;
    public bool hasFinished;

    [Header("Other Data")]
    public int elementsInTrigger;
    public GameObject triggerGameObject;

    protected GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        hasFinished = false;
    }

    public bool HasCurrency()
    {
        if(gameManager.LevelData.currency < buildingCost)
        {
            return false;
        }
        return true;
    }


    public IEnumerator StartProduction()
    {
        hasFinished = false;
        Debug.Log("Production start!");
        yield return new WaitForSeconds(_spawnRate);
        hasFinished = true;
        Debug.Log("Production stop!");
    }

    public abstract void GiveResourceToPlayer();

}
