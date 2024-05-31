using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPoints : MonoBehaviour
{
    public List<Image> images = new List<Image>();
    public List<Transform> targets = new List<Transform>();

    private void Update()
    {
        for (int i = 0; i < images.Count && i < targets.Count; i++)
        {
            UpdateWaypoint(images[i], targets[i]);
        }
    }

    private void UpdateWaypoint(Image img, Transform target)
    {
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(target.position);

        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
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

    public void AddWaypoint(Image img, Transform target)
    {
        images.Add(img);
        targets.Add(target);
    }
}

