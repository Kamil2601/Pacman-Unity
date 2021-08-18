using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dots : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private Tile bigDotTile;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }


    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3Int cell = tilemap.WorldToCell(other.transform.position);
            if (tilemap.GetTile(cell) == bigDotTile)
            {
                Debug.Log("Big dot eaten!");
            }
            tilemap.SetTile(cell, null);
        }
    }
}
