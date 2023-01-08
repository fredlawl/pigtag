using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour, IPointerClickHandler
{
    private bool gameIsPaused = false;

    [SerializeField]
    private SpawnerManager playerSpawner;

    [SerializeField]
    private SpawnerManager enemySpawner;

    [SerializeField]
    private CountdownTimer countdownTimer;

    private GameObject player;
    private readonly List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    private Tilemap map;
    private Pathing.GameGrid gameGrid;

    public delegate void OnMapClick(PointerEventData eventData);
    public event OnMapClick mapClickEvent; 

    private void Awake()
    {
        countdownTimer.onTick.AddListener(OnCountdownTimerTick);
        countdownTimer.onFinished.AddListener(OnCountdownTimerFinished);

        gameGrid = new Pathing.GameGrid(map);
        gameGrid.HydrateObstacles();
    }

    private void Start()
    {
        player = playerSpawner.SpawnInactive();
        mapClickEvent += player.GetComponent<Player.ClickMovement>().OnPointerClick;
        player.GetComponent<Player.PlayerManager>().onDied.AddListener(OnPlayerDied);
        Pathable pathable = player.AddComponent<Pathable>();
        pathable.pathfinder = gameGrid;
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
        if (timeElapsed >= 10 && enemies.Count == 0)
        {
            /*
             * there's a bug here that if the instantiation fails, then 
             * we'll spawn infinitely.
             */
            GameObject obj = enemySpawner.SpawnInactive();
            enemies.Add(obj);

            Pathable p = obj.AddComponent<Pathable>();
            p.pathfinder = gameGrid;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        mapClickEvent.Invoke(eventData);
    }
}
