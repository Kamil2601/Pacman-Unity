using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameOver gameOverScreen;

    void Awake()
    {
        if (Instance == null)
            instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        Dots.bigDotEaten += SetGhostsFrightened;
        Player.playerDeath += PlayerDeath;
        Dots.gameWon += GameWon;
    }

    private void GameWon()
    {
        StopAll();
        gameOverScreen.gameObject.SetActive(true);
        gameOverScreen.GameWon();
    }

    private void OnDisable()
    {
        Dots.bigDotEaten -= SetGhostsFrightened;
        Player.playerDeath -= PlayerDeath;
    }

    private void Start()
    {
        ghosts = FindObjectsOfType<Ghost>().ToList();

        this.player = FindObjectOfType<Player>();

        StartCoroutine(GhostModeChangeInTime());
    }
    
    private void PlayerDeath(int livesLeft)
    {
        StopAll();

        if (livesLeft > 0)
            StartCoroutine(WaitAndReset());
        else
        {
            StartCoroutine(EndGame());
        }
            
    }

    private void StopAll()
    {
        player.Stop();

        ghosts.ForEach(ghost => ghost.Stop());
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3);

        gameOverScreen.gameObject.SetActive(true);
        gameOverScreen.GameLost();
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(3);

        ghosts.ForEach(ghost => ghost.ResetState());

        player.ResetState();

        StopAllCoroutines();
    }

    public void SetGhostsFrightened()
    {
        ghosts.ForEach(ghost => ghost.SetFrightened(frightenedTime));
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

        ghosts.ForEach(ghost => ghost.SetMode());
    }
}
