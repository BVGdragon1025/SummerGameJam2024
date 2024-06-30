using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsHelper : MonoBehaviour
{
    [SerializeField]
    private int _amountOfStructures = 0;
    public int AmountOfStructures { get { return _amountOfStructures; }  set { _amountOfStructures = value; } }
    public BuildingType buildingType;
    private GameManager _gameManager;
    private Collider _collider;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _collider = GetComponent<Collider>();

    }

    private void Update()
    {
        if (_amountOfStructures == _gameManager.LevelData.maxBuildingInRange)
        {
            if(_collider.enabled)
                _collider.enabled = false;
        }

        if(_amountOfStructures < _gameManager.LevelData.maxBuildingInRange)
        {
            if(!_collider.enabled)
                _collider.enabled = true;
        }
    }
}
