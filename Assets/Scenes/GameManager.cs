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

    private void Awake()
    {
        countdownTimer.onTick.AddListener(OnCountdownTimerTick);
        countdownTimer.onFinished.AddListener(OnCountdownTimerFinished);
    }

    private void Start()
    {
        player = playerSpawner.Spawn();
        player.GetComponent<PlayableBoundary>().boundary = map.localBounds;
        trackableCamera.objectToTrack = player;
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
        if (timeElapsed >= 10 && enemies.Count == 0)
        {
            GameObject obj = enemySpawner.Spawn();
            obj.GetComponent<PlayableBoundary>().boundary = map.localBounds;
            enemies.Add(obj);
        }
    }

    private void OnCountdownTimerFinished()
    {
        Debug.Log("Game over!");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Health>()?.Kill();
        }

        SceneManager.LoadScene("GameOver");
        SceneManager.UnloadSceneAsync("Game");
    }
}
