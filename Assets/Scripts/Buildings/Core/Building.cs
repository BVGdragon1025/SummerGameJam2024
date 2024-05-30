using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingManager))]
public abstract class Building : MonoBehaviour
{
    [Header("Building Data"), Tooltip("Resource values and other things")]
    [SerializeField]
    protected float buildingCost = 0;
    public float BuildingCost { get { return buildingCost; } }
    [SerializeField]
    protected float currencyAmount;
    public float Currency { get { return currencyAmount; } }
    [SerializeField]
    protected float lvlPlagueLoss;
    public float LvlPlagueLoss { get { return lvlPlagueLoss; } }
    [SerializeField, Tooltip("Rate at which this buiilding spawns it's resources, in seconds")]
    private float _spawnRate;

    protected GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;   
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
        Debug.Log("Production start!");
        yield return new WaitForSeconds(_spawnRate);
        Debug.Log("Production stop!");
    }

}
