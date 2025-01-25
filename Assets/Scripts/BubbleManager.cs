using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public GameObject bubblePrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float bubbleSpacing = 1.0f; // Distance between bubbles in the grid
    public float moveDownInterval = 5f; // Time between moving rows down
    public float moveDownAmount = 0.5f; // Distance to move down each step
    public GameObject[,] bubbleGrid; // 2D array to hold the grid of bubbles
    public int matchThreshold = 3; // Minimum number of connected bubbles to pop
    public Sprite[] bubbleSprites; // Array of sprites for bubble colors

    void Start()
    {
        // Initialize the grid
        bubbleGrid = new GameObject[gridWidth, gridHeight];

        // Generate initial rows of bubbles
        GenerateInitialBubbles();

        StartCoroutine(MoveBubblesDown());
    }

    void GenerateInitialBubbles()
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                Vector3 position = new Vector3(
                    j * bubbleSpacing - gridWidth / 2f * bubbleSpacing + bubbleSpacing / 2f,
                    i * bubbleSpacing,
                    0
                );

                GameObject bubble = Instantiate(bubblePrefab, position, Quaternion.identity, transform);

                // Disable physics for grid bubbles
                Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
                if (rb != null) Destroy(rb); // Remove Rigidbody2D

                AssignRandomColor(bubble);     
                bubbleGrid[j, i] = bubble;
            }
        }
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Round((position.x + gridWidth / 2f * bubbleSpacing) / bubbleSpacing) * bubbleSpacing - gridWidth / 2f * bubbleSpacing;
        float snappedY = Mathf.Round(position.y / bubbleSpacing) * bubbleSpacing;
        return new Vector3(snappedX, snappedY, 0);
    }

    IEnumerator MoveBubblesDown()
    {
        float cumulativeMoveDown = 0f; // Track the total amount the bubbles have moved down

        while (true)
        {
            yield return new WaitForSeconds(moveDownInterval);

            // Move all bubbles down
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    if (bubbleGrid[i, j] != null)
                    {
                        bubbleGrid[i, j].transform.position += Vector3.down * moveDownAmount;
                    }
                }
            }

            // Update cumulative movement
            cumulativeMoveDown += moveDownAmount;

            // Check if a new row needs to be added
            if (cumulativeMoveDown >= bubbleSpacing)
            {
                AddNewRow(); // Add a new row of bubbles
                cumulativeMoveDown = 0f; // Reset the cumulative movement tracker
            }

            // Check if bubbles hit the bottom (game over condition)
            if (CheckGameOver())
            {
                Debug.Log("Game Over! Bubbles reached the bottom.");
                StopAllCoroutines();
            }
        }
    }

    bool CheckGameOver()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (bubbleGrid[i, j] != null && bubbleGrid[i, j].transform.position.y <= -Camera.main.orthographicSize)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AddNewRow()
    {
        // Shift bubbles up to make space for a new row
        for (int j = gridHeight - 1; j > 0; j--)
        {
            for (int i = 0; i < gridWidth; i++)
            {
                bubbleGrid[i, j] = bubbleGrid[i, j - 1];
            }
        }

        // Generate a new row of bubbles
        for (int i = 0; i < gridWidth; i++)
        {
            Vector3 position = new Vector3(
                i * bubbleSpacing - gridWidth / 2f * bubbleSpacing + bubbleSpacing / 2f,
                (gridHeight - 1) * bubbleSpacing,
                0
            );

            GameObject bubble = Instantiate(bubblePrefab, position, Quaternion.identity, transform);
            // Disable physics for grid bubbles
            Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
            if (rb != null) Destroy(rb); // Remove Rigidbody2D
            
            AssignRandomColor(bubble);            
            bubbleGrid[i, 0] = bubble;
        }
    }

    void CheckForMatches(int startX, int startY)
    {
        List<GameObject> matchingBubbles = new List<GameObject>();
        Color targetColor = bubbleGrid[startX, startY].GetComponent<SpriteRenderer>().color;

        // Flood-fill or DFS to find all connected bubbles of the same color
        FindMatchingBubbles(startX, startY, targetColor, matchingBubbles);

        // Remove bubbles if 3 or more are matched
        if (matchingBubbles.Count >= 3)
        {
            foreach (GameObject bubble in matchingBubbles)
            {
                int x = Mathf.RoundToInt((bubble.transform.position.x + gridWidth / 2f * bubbleSpacing) / bubbleSpacing);
                int y = Mathf.RoundToInt(bubble.transform.position.y / bubbleSpacing);
                bubbleGrid[x, y] = null; // Remove from grid
                Destroy(bubble); // Destroy the bubble
            }
        }
    }

    // Recursive method to find all connected bubbles of the same color
    void FindMatchingBubbles(int x, int y, Color targetColor, List<GameObject> matchingBubbles)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight || bubbleGrid[x, y] == null)
            return;

        SpriteRenderer sr = bubbleGrid[x, y].GetComponent<SpriteRenderer>();
        if (sr != null && sr.color == targetColor && !matchingBubbles.Contains(bubbleGrid[x, y]))
        {
            matchingBubbles.Add(bubbleGrid[x, y]);

            // Check adjacent cells
            FindMatchingBubbles(x + 1, y, targetColor, matchingBubbles);
            FindMatchingBubbles(x - 1, y, targetColor, matchingBubbles);
            FindMatchingBubbles(x, y + 1, targetColor, matchingBubbles);
            FindMatchingBubbles(x, y - 1, targetColor, matchingBubbles);
        }
    }

    // Helper: Check if a grid position is within bounds
    private bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    public void AddBubbleToGrid(GameObject bubble, Vector3 position)
    {
        int x = Mathf.RoundToInt((position.x + gridWidth / 2f * bubbleSpacing) / bubbleSpacing);
        int y = Mathf.RoundToInt(position.y / bubbleSpacing);

        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            bubbleGrid[x, y] = bubble; // Add the bubble to the grid
        }

        CheckForMatches(x, y); // Check for matches starting from this bubble
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
