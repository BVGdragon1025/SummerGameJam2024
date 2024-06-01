using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpecialHelper : MonoBehaviour
{
    private Building _building;

    private void OnEnable()
    {
        _building = GetComponentInParent<Building>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Building"))
        {
            Debug.Log("Structure in trigger!");
            _building.elementsInTrigger++;
            Debug.Log($"Giving {_building.BuildingType} to {other.gameObject.name}");
            Building building = other.GetComponent<Building>();
            IncreaseStatistics(building);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Building"))
        {
            Debug.Log("Element exited trigger!");
            _building.elementsInTrigger--;
            Debug.Log($"Taking away {gameObject.tag} from {other.gameObject.name}");
            Building building = other.GetComponent<Building>();
            DecreaseStatistics(building);
        }
    }

    void IncreaseStatistics(Building other)
    {
        switch(_building.BuildingType)
        {
            case BuildingType.HealthUpgrade:
                other.MaxPlagueTime = _building.Currency;
                break;
            case BuildingType.SpeedUpgrade:
                other.SpawnRate = _building.Currency;
                break;
            case BuildingType.ResourceUpgrade:
                other.Currency = _building.Currency;
                break;

        }
    }

    void DecreaseStatistics(Building other)
    {
        switch (_building.BuildingType)
        {
            case BuildingType.HealthUpgrade:
                other.MaxPlagueTime = -_building.Currency;
                break;
            case BuildingType.SpeedUpgrade:
                other.SpawnRate = -_building.Currency;
                break;
            case BuildingType.ResourceUpgrade:
                other.Currency = -_building.Currency;
                break;

        }
    }

}
