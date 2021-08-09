using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavigationHelper : MonoBehaviour
{
    public static NavigationHelper instance;

    [SerializeField] private Tilemap board;
    [SerializeField] private GameObject player;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public Vector3Int GetCellOnBoard(Vector3 position)
    {
        Vector3Int cell = board.WorldToCell(position);

        return cell;
    }

    public Vector3Int GetCellOnBoard(Transform transform)
    {
        return GetCellOnBoard(transform.position);
    }

    public Vector3Int GetPlayerCell()
    {
        return GetCellOnBoard(player.transform.position);
    }
}
