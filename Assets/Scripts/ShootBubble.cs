using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBubble : MonoBehaviour
{
    public GameObject bubblePrefab; // Assign your bubble prefab here
    public float shootForce = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bubble = Instantiate(bubblePrefab, transform.position, transform.rotation);
        Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * shootForce, ForceMode2D.Impulse);
    }
}