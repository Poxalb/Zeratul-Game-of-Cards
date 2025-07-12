using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GridManager : MonoBehaviour
{

    public int width = 10;
    public int height = 5;

    public GameObject cardPrefab; //asgina el prefab en el inspector

    public GameObject cellPrefab;

    public List<GameObject> occupiedCells = new List<GameObject>();

    public GameObject[,] gridCells;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        gridCells = new GameObject[width, height];

        Vector2 centerOffSet = new Vector2(width / 2f - 0.5f, height / 2f - 0.5f);


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                Vector2 spawnPosition = position - centerOffSet;

                GameObject cell = Instantiate(cellPrefab, spawnPosition, Quaternion.identity);
                cell.transform.SetParent(transform, false);

                cell.GetComponent<GridCell>().gridIndex = position;

                // Ensure grid cells render on top of background elements
                SpriteRenderer cellRenderer = cell.GetComponent<SpriteRenderer>();
                if (cellRenderer != null)
                {
                    cellRenderer.sortingOrder = 100; // Very high sorting order to render above backgrounds
                }
                
                // Also set sorting order for any child SpriteRenderers
                SpriteRenderer[] childRenderers = cell.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer renderer in childRenderers)
                {
                    renderer.sortingOrder = 100;
                }

                gridCells[x, y] = cell;
            }
        }
    }

    public bool AddOccupantToGrid(Card occupant, Vector2 gridIndex)
    {
        if (gridIndex.x >= 0 && gridIndex.x < width && gridIndex.y >= 0 && gridIndex.y < height)
        {
            GridCell cell = gridCells[(int)gridIndex.x, (int)gridIndex.y].GetComponent<GridCell>();

            if (cell.isOccupied)
            {
                Debug.LogError("Cell is already occupied");
                return false;
            }
            else
            {
                GameObject newObj = Instantiate(cardPrefab); // Don't set world position yet

                // Set parent first
                newObj.transform.SetParent(cell.transform);

                // Snap to center of the cell
                newObj.transform.localPosition = Vector3.zero;

                // Scale it to fit in the cell
                newObj.transform.localScale = new Vector3(0.04f, 0.03f, 1f);

                // Assign card data
                newObj.GetComponent<CardDisplay>().cardData = occupant;

                // Ensure cards also render on top
                SpriteRenderer cardRenderer = newObj.GetComponent<SpriteRenderer>();
                if (cardRenderer != null)
                {
                    cardRenderer.sortingOrder = 200; // Even higher sorting order for cards
                }
                
                // Also set sorting order for any child SpriteRenderers on cards
                SpriteRenderer[] cardChildRenderers = newObj.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer renderer in cardChildRenderers)
                {
                    renderer.sortingOrder = 200;
                }

                // Track occupancy
                occupiedCells.Add(newObj);
                cell.occupant = newObj;
                cell.isOccupied = true;

                CardMovement movement = newObj.GetComponent<CardMovement>();
                movement.InitializeCardState(4, true);


                return true;
            }

        }
        else
        {
            Debug.LogError("Grid index out of bounds: " + gridIndex);
            return false;
        }
    }

    public bool IsCellOccupied(Vector2 gridIndex)
    {
        if (gridIndex.x >= 0 && gridIndex.x < width && gridIndex.y >= 0 && gridIndex.y < height)
        {
            GridCell cell = gridCells[(int)gridIndex.x, (int)gridIndex.y].GetComponent<GridCell>();
            return cell.isOccupied;
        }
        else
        {
            Debug.LogError("Grid index out of bounds: " + gridIndex);
            return false;
        }
    }


    public Card GetCardAtGridIndex(Vector2 gridIndex)
    {
        if (gridIndex.x >= 0 && gridIndex.x < width && gridIndex.y >= 0 && gridIndex.y < height)
        {
            GridCell cell = gridCells[(int)gridIndex.x, (int)gridIndex.y].GetComponent<GridCell>();
            if (cell.isOccupied && cell.occupant != null)
            {
                CardDisplay cardDisplay = cell.occupant.GetComponent<CardDisplay>();
                if (cardDisplay != null)
                {
                    return cardDisplay.cardData;
                }
            }
        }
        return null;
    }



}
