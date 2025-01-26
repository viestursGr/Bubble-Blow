using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBubble : Bubble
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Shot bubble started");
        rb = GetComponent<Rigidbody2D>();
        bubbleManager = FindObjectOfType<BubbleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BubbleManager bubbleManager;
    private Rigidbody2D rb;
    private bool AddedToGrid = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (AddedToGrid == false && collision.gameObject.CompareTag("bubble"))
        {
            // Stop the shot bubble's movement
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;

            // Snap the bubble to the grid and add it
            Vector3 snappedPosition = bubbleManager.SnapToGrid(transform.position);
            transform.position = snappedPosition;

            var collidedBubble = collision.gameObject.GetComponent<Bubble>();
            bubbleManager.AddBubbleToGrid(gameObject, snappedPosition);

            // Disable this script after the bubble is part of the grid
            AddedToGrid = true;

            // Notify the cannon that it can shoot again
            FindObjectOfType<ShootBubble>().OnBubbleResolved();
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
