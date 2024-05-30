using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingComponent : MonoBehaviour
{
    [Header("Helper values")]
    [SerializeField]
    private float _healingElementDelay;
    [SerializeField]
    private float _healingStructureDelay;
    [SerializeField]
    private float _plagueElementPenalty;
    [SerializeField]
    private float _plagueHealingValue;

    [Header("Helper bools")]
    [SerializeField]
    private bool _isHealingElement;
    [SerializeField]
    private bool _isRestoringStructure;

    [Header("Object References")]
    [SerializeField]
    private GameObject _structure;
    [SerializeField]
    private GameObject _element;
    private PlayerController _playerController;

    private void Awake()
    {
        _isRestoringStructure = false;
        _isHealingElement = false;
        _playerController = GetComponentInParent<PlayerController>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Element"))
        {
            _element = other.gameObject;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Structure"))
        {
            _structure = other.gameObject;
        }
    }

    private void Update()
    {
        if(_element != null)
        {
            ElementsController element = _element.GetComponentInParent<ElementsController>();
            if (element.hasPlague)
            {
                if (Input.GetKeyDown(KeyCode.F) && !_isHealingElement)
                {
                    StartCoroutine(HealElement(element));
                }
            }

            if(_isHealingElement && _playerController.PlayerMovement != 0)
            {
                StopAllCoroutines();
                _isHealingElement = false;
                Debug.LogFormat("<color=red>Healing interupted!</color>");
            }   
        }

        if(_structure != null)
        {
            Building building = _structure.GetComponent<Building>();
            if(building.hasPlague)
            {
                if(building.CurrentPlague > 0)
                {
                    building.CurrentPlague = _plagueHealingValue;
                }

                if(building.CurrentPlague <= 0)
                {
                    building.hasPlague = false;
                }

            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Element"))
        {
            _element = null;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Structure"))
        {
            _structure = null;
        }
    }

    private IEnumerator HealElement(ElementsController element)
    {
        _isHealingElement = true;
        Debug.LogFormat("<color=green>Healing started</color>");
        yield return new WaitForSeconds(_healingElementDelay);
        GameManager.Instance.ChangePlayerPlagueLevel(_plagueElementPenalty);
        element.hasPlague = false;
        Debug.LogFormat("<color=green>Healing completed!</color>");
        _isHealingElement = false;
    }

    private IEnumerator HealStructure(Building building)
    {
        _isRestoringStructure = true;
        Debug.LogFormat("<color=orange>Structure healing start!</color>");
        building.CurrentPlague = _plagueHealingValue;
        yield return new WaitForSeconds(_healingStructureDelay);
        _isRestoringStructure = false;

    }

}
