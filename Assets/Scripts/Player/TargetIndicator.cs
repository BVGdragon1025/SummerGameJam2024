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

            
            //float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
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
