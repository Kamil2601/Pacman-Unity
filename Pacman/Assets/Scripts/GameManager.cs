using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private Transform chasePoint;

    private List<Ghost> ghosts;
    private Player player;

    private float frightenedTime = 8f;

    private Mode currentGhostMode = Mode.Scatter;

    public static GameManager Instance { get => instance; }
    public Mode CurrentGhostMode { get => currentGhostMode; }

    void Awake()
    {
        ghosts = new List<Ghost>();

        if (Instance == null)
            instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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
        player.Stop();

        foreach (var ghost in ghosts)
        {
            ghost.Stop();
        }

        StartCoroutine(WaitAndReset());
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(2);

        foreach (var ghost in ghosts)
        {
            ghost.ResetState();
        }

        player.ResetState();
    }

    public void SetGhostsFrightened()
    {
        foreach (var ghost in ghosts)
        {
            ghost.SetFrightened(frightenedTime);
        }
    }
}
