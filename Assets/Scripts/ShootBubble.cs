using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBubble : MonoBehaviour
{
    public GameObject bubblePrefab; // Assign your bubble prefab here
    public float shootForce = 10f;
    public Sprite[] bubbleSprites; // Array of sprites for bubble colors
    public BubbleManager bubbleManager;
    public Transform cannonTip; // Assign the top of the cannon in the Inspector
    private bool canShoot = true; // Ensure only one bubble can be active at a time

    void Start()
    {
        bubbleManager = FindObjectOfType<BubbleManager>();
    }

    void Update()
    {
        bool canShoot = Input.GetMouseButtonDown(0);
        if (canShoot) // Left mouse click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (!canShoot) return; // Prevent shooting if a bubble is already active

      // Instantiate the bubble at the cannon's tip
        GameObject bubble = Instantiate(bubblePrefab, cannonTip.position, transform.rotation);



        // gets color of next ball, that's loaded, colors are selected before shooting so player can prepare
        string color = bubbleManager.GetCurrentColor();

        ChangeColor(color, bubble);
        Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * shootForce, ForceMode2D.Impulse);

        AssignRandomColor(bubble);

        // Mark that shooting is disabled until the bubble is resolved
        canShoot = false;
    }

    void ChangeColor(string color, GameObject bubble) 
    {
        var bb = bubble.GetComponent<Bubble>();
        SpriteRenderer spriteRenderer = bubble.GetComponent<SpriteRenderer>();
        foreach(var sprite in bubbleSprites)
        {
            if (sprite.name.Contains(color.ToLower())) {
                bb.BubbleColor = color;
                spriteRenderer.sprite = sprite;
            }
        }
    }

    void AssignRandomColor(GameObject bubble)
    {
        var bb = bubble.GetComponent<Bubble>();

        if (bubbleSprites != null && bubbleSprites.Length > 0)
        {
            // Choose a random sprite (if using sprites)
            int randomIndex = Random.Range(0, bubbleSprites.Length);
            SpriteRenderer spriteRenderer = bubble.GetComponent<SpriteRenderer>();
            
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = bubbleSprites[randomIndex];

                string spriteName = spriteRenderer.sprite.name;
                Debug.Log("Sprite name: " + spriteName);
                switch (spriteName)
                {
                    case "gold_bubble":
                        bb.BubbleColor = "GOLD";
                        break;
                    case "blue_bubble":
                        bb.BubbleColor = "BLUE";
                        break;
                    case "pink_bubble":
                        bb.BubbleColor = "PINK";
                        break;
                    case "green_bubble":
                        bb.BubbleColor = "GREEN";
                        break;
                    case "purple_bubble":
                    default:
                        bb.BubbleColor = "PURPLE";
                        break;
                }

                Debug.Log("Color: " + bb.BubbleColor);
            }
        }
    }

    // Reset the shooting state when the bubble is resolved
    public void OnBubbleResolved()
    {
        canShoot = true;
    }
}