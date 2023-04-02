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

    private IBuildState buildState;

    private void Awake()
    {
        countdownTimer.onTick.AddListener(OnCountdownTimerTick);
        countdownTimer.onFinished.AddListener(OnCountdownTimerFinished);

        gameGrid = new Pathing.GameGrid(map);

        Tilemap difficultTerrain = GameObject.Find("DifficultTerrain").GetComponent<Tilemap>();
        // Necessary for loading in difficult terrain based on tile map data
        for (int y = difficultTerrain.cellBounds.y; y < difficultTerrain.cellBounds.yMax; y++)
        {
            for (int x = difficultTerrain.cellBounds.x; x < difficultTerrain.cellBounds.xMax; x++)
            {
                var pos = new Vector3Int(x, y, 0);
                if (difficultTerrain.HasTile(pos))
                {
                    gameGrid.AddObstacle(gameGrid.GetNodeFromWorldPosition(pos).gridPosition);
                }
            }
        }


        gameGrid.HydrateObstacles();
    }

    private void Start()
    {
        player = playerSpawner.SpawnInactive();
        var manager = player.GetComponent<Player.PlayerManager>();
        var clickMovement = player.GetComponent<Player.ClickMovement>();
        mapClickEvent += clickMovement.OnPointerClick;
        manager.onDied.AddListener(OnPlayerDied);
        Pathable pathable = player.AddComponent<Pathable>();
        pathable.pathfinder = gameGrid;
        player.name = "Player";

        buildState = new Entry(new BuildStateConfig
        {
            owner = player,
            buildables = manager.buildableBuildings,
            gameGrid = gameGrid
        });

        player.SetActive(true);
    }

    private void Update()
    {
        // TODO: Bring up esc-menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePlay();
        }

        buildState = buildState.doAction();
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

    public class BuildStateConfig
    {
        public List<Building.Building> buildables;
        public GameObject owner;
        public Pathing.GameGrid gameGrid;
    }

    private interface IBuildState
    {
        public IBuildState doAction();
    }

    private class Entry : IBuildState
    {
        private BuildStateConfig config;

        public Entry(BuildStateConfig cfg)
        {
            config = cfg;
        }

        public IBuildState doAction()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                return new BuilderState(config);
            }

            return this;
        }
    }

    private class BuilderState : IBuildState
    {
        private BuildStateConfig config;

        public BuilderState(BuildStateConfig cfg)
        {
            config = cfg;
        }

        public IBuildState doAction()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return new Entry(config);
            }

            foreach (Building.Building building in config.buildables) {
                if (Input.GetKeyDown(building.shortcut))
                {
                    return new BuildState(config, building.Spawn(config.owner));
                }
            }

            return this;
        }
    }

    private class BuildState : IBuildState
    {
        private BuildStateConfig config;
        private Building.Building building;

        public BuildState(BuildStateConfig cfg, Building.Building building)
        {
            config = cfg;
            this.building = building;
        }

        public IBuildState doAction()
        {
            var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos = config.gameGrid.GetNodeFromWorldPosition(mouse).worldPosition;
            pos.z = Camera.main.nearClipPlane;
            building.transform.position = pos;

            if (Input.GetKeyDown(KeyCode.Escape) || this.building.state == Building.Building.BuildingState.Building)
            {
                Debug.Log("Exiting Placing state");
                return new BuilderState(config);
            }

            if (Input.GetMouseButtonDown(0))
            {                
                // todo: move player to location and any player movement cancels the build command
                // but if player reaches location an and then construct is called, go ahead
                // and allow player to move again
                // The obsticle will have to be also removed if not constructed
                building.Construct(pos);
                building.onBuilt += AddObstacle;
            }

            return this;
        }

        public void AddObstacle(GameObject obj)
        {
            config.gameGrid.AddObstacle(obj);
        }
    }
}
