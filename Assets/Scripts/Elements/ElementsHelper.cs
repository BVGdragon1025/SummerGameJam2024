using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsHelper : MonoBehaviour
{
    public List<GameObject> structures;
    [SerializeField] private int _maxStructures;
    public BuildingType buildingType;

    private void Awake()
    {
        structures = new();
    }

    public void AddStructureToList(GameObject gameObject)
    {
        structures.Add(gameObject);
    }

    private void Update()
    {
        if (structures.Count == _maxStructures)
        {
            gameObject.SetActive(false);
        }
    }
}
