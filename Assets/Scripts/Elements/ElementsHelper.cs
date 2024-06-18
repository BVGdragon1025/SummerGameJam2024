using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsHelper : MonoBehaviour
{
    public List<GameObject> structures;
    public BuildingType buildingType;
    private GameManager _gameManager;
    private Collider _collider;

    private void Awake()
    {
        structures = new();
        _gameManager = GameManager.Instance;
        _collider = GetComponent<Collider>();

    }

    public void AddStructureToList(GameObject gameObject)
    {
        structures.Add(gameObject);
    }

    public void RemoveStructureFromList(GameObject gameObject)
    {
        structures.Remove(gameObject);
    }

    private void Update()
    {
        if (structures.Count == _gameManager.LevelData.maxBuildingInRange)
        {
            if(_collider.enabled)
                _collider.enabled = false;
        }

        if(structures.Count < _gameManager.LevelData.maxBuildingInRange)
        {
            if(!_collider.enabled)
                _collider.enabled = true;
        }
    }
}
