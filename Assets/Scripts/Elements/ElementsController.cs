using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsController : MonoBehaviour
{
    private PlagueState _plagueState;
    public PlagueState PlagueState { get { return _plagueState; } }
    [SerializeField]
    private GameObject _objectWithPlague;
    [SerializeField]
    private GameObject _healthyObject;

    void OnEnable()
    {
        ChangePlagueState(PlagueState.Infected);
    }

    public void ChangePlagueState(PlagueState plagueState)
    {
        switch(plagueState)
        {
            case PlagueState.Infected:
                _healthyObject.SetActive(false);
                _objectWithPlague.SetActive(true);
                break;
            case PlagueState.Healthy:
                _objectWithPlague.SetActive(false);
                _healthyObject.SetActive(true);
                break;
            default:
                _objectWithPlague.SetActive(false);
                _healthyObject.SetActive(true);
                break;
        }

        _plagueState = plagueState;
    }

}
