using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float minAngle = -60f; // Minimum rotation angle
    public float maxAngle = 60f;  // Maximum rotation angle

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Clamp the angle within the allowed range
        angle = Mathf.Clamp(angle - 90, minAngle, maxAngle);

        // Apply the rotation
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}