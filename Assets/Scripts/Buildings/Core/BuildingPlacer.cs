using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    private GameObject _buildingPrefab;
    private GameObject _toBuild;
    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hit;

    public LayerMask groundLayerMask;
    public static BuildingPlacer Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _mainCamera = Camera.main;
        _buildingPrefab = null;
    }

    private void Update()
    {
        if(_buildingPrefab != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if(_toBuild.activeSelf)
                    _toBuild.SetActive(false);
            }
            else if (!_toBuild.activeSelf)
            {
                _toBuild.SetActive(true);
            }

            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(_ray, out _hit, 1000f, groundLayerMask))
            {
                Debug.Log($"GameObject {_hit.collider.name} with LayerMask {_hit.collider.gameObject.layer}");

                if(_toBuild.activeSelf)
                    _toBuild.SetActive(true);
                _toBuild.transform.position = _hit.point;
            }
            else if (_toBuild.activeSelf)
            {
                _toBuild.SetActive(false);
            }
                
        }
    }

    public void SetBuildinfPrefab(GameObject prefab)
    {
        _buildingPrefab = prefab;
        PrepareBuilding();
    }

    private void PrepareBuilding()
    {
        if(_toBuild)
            Destroy(_toBuild);

        _toBuild = Instantiate(_buildingPrefab);
        _toBuild.SetActive(false);
    }

}
