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

    [SerializeField] private List<float> ghostModeTimes;
    // private int index = 0;

    void Awake()
    {
        ghosts = new List<Ghost>();

        if (Instance == null)
            instance = this;

        else if (Instance != this)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Dots.bigDotEaten += SetGhostsFrightened;
        Player.playerDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        Dots.bigDotEaten -= SetGhostsFrightened;
        Player.playerDeath -= PlayerDeath;
    }

    private void Start()
    {
        foreach (var ghost in FindObjectsOfType<Ghost>())
        {
            ghosts.Add(ghost);
        }

        this.player = FindObjectOfType<Player>();

        StartCoroutine(GhostModeChangeInTime());
    }
    
    public void PlayerDeath(int livesLeft)
    {
        player.Stop();

        foreach (var ghost in ghosts)
        {
            ghost.Stop();
        }

        if (livesLeft > 0)
            StartCoroutine(WaitAndReset());
        else
            Debug.Log("Game Over!");
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(2);

        foreach (var ghost in ghosts)
        {
            ghost.ResetState();
        }

        player.ResetState();

        StopAllCoroutines();
    }

    public void SetGhostsFrightened()
    {
        foreach (var ghost in ghosts)
        {
            ghost.SetFrightened(frightenedTime);
        }
    }

    private IEnumerator GhostModeChangeInTime()
    {
        foreach (var time in ghostModeTimes)
        {
            yield return new WaitForSeconds(time);

            ChangeCurrentGhostMode();
        }
    }

    private void ChangeCurrentGhostMode()
    {
        if (currentGhostMode == Mode.Scatter)
            currentGhostMode = Mode.Chase;
        else
            currentGhostMode = Mode.Scatter;

        foreach (var ghost in ghosts)
        {
            ghost.SetMode();
        }
    }
}
