using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dots : MonoBehaviour
{
    [SerializeField] private GameObject playerMouth;

    private Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerMouth)
        {
            Vector3Int cell = tilemap.WorldToCell(other.transform.position);
            tilemap.SetTile(cell, null);
        }
    }
}
