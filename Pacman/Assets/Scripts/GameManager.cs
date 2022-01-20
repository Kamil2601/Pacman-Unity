using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private Transform chasePoint;

    private List<Ghost> ghosts;
    private Player player;

    private float frightenedTime = 8f;

    public static GameManager Instance { get => instance; }

    void Awake()
    {
        ghosts = new List<Ghost>();

        if (Instance == null)
            instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        
    }

    public void AddGhost(Ghost ghost)
    {
        if (!ghosts.Contains(ghost))
            ghosts.Add(ghost);
    }

    public void AddPlayer(Player player)
    {
        this.player = player;
    }

    public void StopAll()
    {
        Debug.Log("Game Over!");

        player.Stop();

        foreach (var ghost in ghosts)
        {
            ghost.Stop();
        }
    }

    public void SetGhostsFrightened()
    {
        foreach (var ghost in ghosts)
        {
            ghost.SetFrightened(frightenedTime);
        }
    }
}
