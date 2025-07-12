
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class GridCellHighlighter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public Color posColor = Color.green;

    public Color negColor = Color.red;
    public Color highlightColor = Color.yellow; // Color to highlight the cell

    public GridCell gridCell; // Reference to the GridCell component

    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridCell = GetComponent<GridCell>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Store the original color of the cell
        }
    }


    void OnMouseEnter()
    {
        Debug.Log("Mouse entered the cell: " + gridCell.gridIndex);
        if (spriteRenderer != null && gameManager != null && !gameManager.playingCard && !gridCell.isOccupied)
        {
            spriteRenderer.color = highlightColor; // Change the color to highlight when the mouse enters
        }
        else if (gridCell.isOccupied || gridCell.gridIndex.y > 0)
        {
            spriteRenderer.color = negColor; // Change to negative color if the cell is occupied or not in the first row
        }
        else
        {
            spriteRenderer.color = posColor; // Change to positive color if the cell is not occupied and in the first row
        }
    }
    void OnMouseExit()
    {
        Debug.Log("Mouse exited the cell: " + gridCell.gridIndex);
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor; // Restore the original color when the mouse exits
        }
    }
}
