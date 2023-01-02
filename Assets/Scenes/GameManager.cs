using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private bool gameIsPaused = false;

    [SerializeField]
    private SpawnerManager playerSpawner;

    [SerializeField]
    private SpawnerManager enemySpawner;

    [SerializeField]
    private GameCamera.TrackObject trackableCamera;

    [SerializeField]
    private CountdownTimer countdownTimer;

    private GameObject player;
    private readonly List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    private Tilemap map;
    private Pathing.GameGrid gameGrid;
    private Pathing.Pathfinder pathfinder;

    private void Awake()
    {
        countdownTimer.onTick.AddListener(OnCountdownTimerTick);
        countdownTimer.onFinished.AddListener(OnCountdownTimerFinished);

        gameGrid = new Pathing.GameGrid(map);
        gameGrid.MarkObstructables();
        pathfinder = new Pathing.Pathfinder(gameGrid);
    }

    private void Start()
    {
        player = playerSpawner.SpawnInactive();
        PlayableBoundary playerPlayableBoundary = player.AddComponent<PlayableBoundary>();
        playerPlayableBoundary.boundary = map.localBounds;
        player.GetComponent<Player.PlayerManager>().onDied.AddListener(OnPlayerDied);
        trackableCamera.objectToTrack = player;
        player.name = "Player";
        player.SetActive(true);
    }

    private void Update()
    {
        // TODO: Bring up esc-menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePlay();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    private void Play()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    private void TogglePlay()
    {
        gameIsPaused = !gameIsPaused;
        if (gameIsPaused)
        {
            Pause();
        }
        else
        {
            Play();
        }
    }


    private void OnCountdownTimerTick(float secondsRemaining)
    {
        // On or after 10 seconds spawn an enemy
        double timeElapsed = countdownTimer.StartTime.TotalSeconds - secondsRemaining;
        // Fix this logic to actually spawn every second instead of every tick which
        // can be between seconds.
        if (timeElapsed >= 1 && enemies.Count == 0)
        {
            /*
             * there's a bug here that if the instantiation fails, then 
             * we'll spawn infinitely.
             */
            GameObject obj = enemySpawner.SpawnInactive();
            enemies.Add(obj);

            Pathing.Pather p = obj.AddComponent<Pathing.Pather>();
            p.pathfinder = pathfinder;

            obj.SetActive(true);
        }
    }

    private void OnCountdownTimerFinished()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Health>()?.Kill();
        }

        SceneManager.LoadScene("GameOverWin");
    }

    private void OnPlayerDied()
    {
        SceneManager.LoadScene("GameOverLoose");
    }
}
