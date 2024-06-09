using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsHelper : MonoBehaviour
{
    public List<GameObject> structures;
    public BuildingType buildingType;
    private GameManager _gameManager;

    private void Awake()
    {
        structures = new();
        _gameManager = GameManager.Instance;

    }

    public void AddStructureToList(GameObject gameObject)
    {
        structures.Add(gameObject);
    }

    private void Update()
    {
        if (structures.Count == _gameManager.LevelData.maxBuildingInRange)
        {
            gameObject.SetActive(false);
        }
    }
}
