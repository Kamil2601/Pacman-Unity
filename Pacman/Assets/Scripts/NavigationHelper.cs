using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavigationHelper : MonoBehaviour
{
    private static NavigationHelper instance;

    [SerializeField] private Tilemap board;
    [SerializeField] private Transform player;
    [SerializeField] private Transform redGhost;
    [SerializeField] private Transform pinkTarget;
    [SerializeField] private Transform boxCenter;
    [SerializeField] private Transform boxGate;

    public static NavigationHelper Instance { get => instance;}

    public Vector3Int BoxCenter { get; private set; }
    public Vector3Int BoxGate { get; private set; }

    void Awake()
    {
        if (Instance == null)
            instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        BoxCenter = GetCellOnBoard(boxCenter.position);
        BoxGate = GetCellOnBoard(boxGate.position);   
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
        return GetCellOnBoard(player.position);
    }

    public Vector3 GetPlayerPosition()
    {
        return player.position;
    }

    public Vector3 GetRedGhostPosition()
    {
        return redGhost.position;
    }

    public Vector3 GetRedGhostCell()
    {
        return GetCellOnBoard(redGhost.position);
    }

    public Vector3Int GetPinkTargetCell()
    {
        return GetCellOnBoard(pinkTarget.position);
    }

    public Vector3 GetPinkTargetPosition()
    {
        return pinkTarget.position;
    }

    public bool IsEmpty(Vector3Int cell)
    {
        return board.GetTile(cell) == null;
    }

    public bool IsEmpty(Vector3 position)
    {
        return IsEmpty(GetCellOnBoard(position));
    }
}
