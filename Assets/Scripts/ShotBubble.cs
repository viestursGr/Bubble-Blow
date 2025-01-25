using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBubble : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bubbleManager = FindObjectOfType<BubbleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string color; // Assign a color string like "Red", "Blue", etc.
    public BubbleManager bubbleManager;
    private Rigidbody2D rb;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision happened!");

        if (collision.gameObject.CompareTag("BubbleStatic"))
        {
            // Stop the shot bubble's movement
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;

            // Snap the bubble to the grid and add it
            Vector3 snappedPosition = bubbleManager.SnapToGrid(transform.position);
            transform.position = snappedPosition;

            bubbleManager.AddBubbleToGrid(gameObject, snappedPosition);

            // Disable this script after the bubble is part of the grid
            enabled = false;
        }
    }
}
