using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPoints : MonoBehaviour
{
    public Image image;
    public Transform target;
    [SerializeField] private Vector3 _displacement;

    private void Update()
    {

        UpdateWaypoint(image, target);

    }

    private void UpdateWaypoint(Image img, Transform target)
    {
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + _displacement);

        if (Vector3.Dot(((target.position + _displacement) - transform.position), transform.forward) < 0)
        {
            // Target is behind the player
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        img.transform.position = pos;
    }

    public void TurnOffWaypoint()
    {
        image.gameObject.SetActive(false);
    }

    public void TurnOnWaypoint()
    {
        image.gameObject.SetActive(true);
    }

}

