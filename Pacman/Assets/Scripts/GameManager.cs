using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    

    private Transform chasePoint;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        chasePoint = player.transform.Find("ChasePoint");
    }

    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }
}
