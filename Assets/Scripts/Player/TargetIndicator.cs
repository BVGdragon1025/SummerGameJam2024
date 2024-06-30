using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public Transform targetTransform;
    public float hideDistance;

    private void Update()
    {
        Vector3 direction = targetTransform.position - transform.position;

        if(direction.magnitude < hideDistance)
        {
            SetIndicatorActive(false);
        }
        else
        {
            SetIndicatorActive(true);
            transform.LookAt(targetTransform, Vector3.up);

        }

    }

    private void SetIndicatorActive (bool isActive)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }

}
