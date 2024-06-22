using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpecialHelper : MonoBehaviour
{
    private BuildingSpecial _building;

    private void OnEnable()
    {
        _building = GetComponentInParent<BuildingSpecial>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Building"))
        {
            Building building = other.GetComponent<Building>();

            Debug.Log("Structure in trigger!");
            _building.elementsInTrigger++;
            Debug.Log($"Giving {_building.BuildingType} to {other.gameObject.name}");
            
            IncreaseStatistics(building);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Building"))
        {
            Building building = other.GetComponent<Building>();

            Debug.Log("Structure exited trigger!");
            _building.elementsInTrigger--;
            Debug.Log($"Taking away {gameObject.tag} from {other.gameObject.name}");
            
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
                if (!other.GetComponent<Building>().hasFinished)
                {
                    other.ResetProduction();
                }
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
                if (!other.GetComponent<Building>().hasFinished)
                {
                    other.ResetProduction();
                }
                break;
            case BuildingType.ResourceUpgrade:
                other.Currency = -_building.Currency;
                break;

        }
    }

}
