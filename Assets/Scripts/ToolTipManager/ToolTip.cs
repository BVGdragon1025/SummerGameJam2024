using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private Building _building;

	public string message;

    private void Awake()
    {
        _building = GetComponent<Building>();
        message = $"Name: {_building.BuildingName}, \n Cost: {_building.BuildingCost}, \n Gives: {_building.Currency}/{_building.SpawnRate}s, Requires element: {_building.BuildingType}";
    }

    private void Start()
    {
        

    }

}