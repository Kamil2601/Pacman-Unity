using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dots : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private Tile bigDotTile;

    public static event Action bigDotEaten;
    public static event Action dotEaten;
    public static event Action gameWon;

    [SerializeField] private Transform topLeft;
    [SerializeField] private Transform bottomRight;

    private int dotsCount = 0;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();    
        CountDots();
    }

    private void CountDots()
    {
        Vector3Int topLeftCell = tilemap.WorldToCell(topLeft.position);
        Vector3Int bottomRightCell = tilemap.WorldToCell(bottomRight.position);

        for (int i=topLeftCell.y; i >= bottomRightCell.y; i--)
        {
            for (int j=topLeftCell.x; j<=bottomRightCell.x; j++)
            {
                Vector3Int position = new Vector3Int(i,j,0);

                if (tilemap.GetTile(position))
                    dotsCount++;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3Int cell = tilemap.WorldToCell(other.transform.position);

            if (tilemap.GetTile(cell))
            {
                dotEaten?.Invoke();
                dotsCount--;
                if (dotsCount == 0)
                {
                    gameWon?.Invoke();
                }
            }
                

            if (tilemap.GetTile(cell) == bigDotTile)
                bigDotEaten?.Invoke();

            tilemap.SetTile(cell, null);
        }
    }
}
