﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBubble : MonoBehaviour
{
    public GameObject bubblePrefab; // Assign your bubble prefab here
    public float shootForce = 10f;
    public Sprite[] bubbleSprites; // Array of sprites for bubble colors

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bubble = Instantiate(bubblePrefab, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);        
        AssignRandomColor(bubble);
        Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * shootForce, ForceMode2D.Impulse);
    }

    void AssignRandomColor(GameObject bubble)
    {
        if (bubbleSprites != null && bubbleSprites.Length > 0)
        {
            // Choose a random sprite (if using sprites)
            int randomIndex = Random.Range(0, bubbleSprites.Length);
            SpriteRenderer spriteRenderer = bubble.GetComponent<SpriteRenderer>();
            
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = bubbleSprites[randomIndex];
            }
        }
    }
}