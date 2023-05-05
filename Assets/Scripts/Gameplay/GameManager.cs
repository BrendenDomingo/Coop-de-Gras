using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager UIManager;
    public LevelManifest LevelManifest;
    public PlayerController PlayerPrefab;
    public float PlayerSpawnX = 0f;
    public float PlayerSpawnY = 0f;
    public int FinalWave { get; private set; }
    public int CurrentWave { get; private set; } = 0;
    public int KillCount { get; private set; } = 0;
    public static bool GamePaused = false;
    private WaveManifest _currentWave;
    private int _enemiesRemaining = 0;
    private static bool _gameInProgress = false;
    private GameObject _playerGameObject;
    private Transform _playerTransform;
    private static bool _victory = false;
    public static bool Victory
    {
        get 
        {
            return _victory;
        }
    }

    private void Start()
    {
        LoadPlayer();
        StartCoroutine(NextWaveDelay());
    }

    private void Update()
    {
        if (FinalWave == 0)
        {
            FinalWave = LevelManifest.FinalWave;
        }

        if (_enemiesRemaining <= 0 && _gameInProgress)
        {
            if (CurrentWave == FinalWave)
            {
                EndGameVictory(); 
            }
            else
            {
                WaveComplete();
            }
        }
    }

    private void FixedUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        _enemiesRemaining = enemies.Length;
    }

    public void EnemyKilled()
    {
        KillCount++;
    }
    
    public void EndGameDefeat()
    {
        _gameInProgress = false;
        UIManager.OpenDeathPanel();
    }

    public void WaveComplete()
    {
        _gameInProgress = false;
        // TODO add handling for shop here which will call NextWave after done shopping 
        //------------TEMP CODE
        StartCoroutine(NextWaveDelay());
        //------------TEMP CODE
        //NextWave();
    }

    IEnumerator NextWaveDelay()
    {
        if ((CurrentWave + 1) == FinalWave)
        {
            UIManager.SetGameInstruction("FINAL WAVE", "STARTING SOON", 5);
        }
        else
        {
            UIManager.SetGameInstruction("WAVE " + (CurrentWave + 1), "STARTING SOON", 5);
        }
        
        yield return new WaitForSeconds(5f);
        NextWave();
    }

    public void NextWave()
    {
        CurrentWave++;
        LoadWave();
        _gameInProgress = true;
    }

    public void EndGameVictory()
    {
        _gameInProgress = false;
        _victory = true;
        string message = ("Press \"Cancel\" to return to the main menu.\n" +
        "Returning automatically in | seconds...");
        // UI Manager will automatically handle main menu switch after timer
        UIManager.SetGameInstruction("MISSION ACCOMPLISHED", message, 20, true);
    }

    private void LoadPlayer()
    {
        Vector3 position = new Vector3(PlayerSpawnX, PlayerSpawnY, 0f);
        _playerGameObject= (GameObject)Instantiate(PlayerPrefab.gameObject, position, Quaternion.identity);
        _playerGameObject.GetComponent<PlayerController>().GameManager = this;
        _playerTransform = _playerGameObject.transform;     

        // Assign player controller and transform references to scene objects
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().target = _playerTransform;
        GameObject.FindGameObjectWithTag("UI_Manager").GetComponent<UIManager>().PlayerController = _playerGameObject.GetComponent<PlayerController>();
    }

    private void LoadWave()
    {
        _currentWave = LevelManifest.GetWaveManifest(CurrentWave);
        _enemiesRemaining = _currentWave.EnemySpawns.Count;
        foreach(EnemySpawn enemySpawn in _currentWave.EnemySpawns)
        {
            Vector3 position = new Vector3(enemySpawn.x, enemySpawn.y, 0f);
            GameObject enemy = (GameObject)Instantiate(enemySpawn.enemyPrefab.gameObject, position, Quaternion.identity);
            Enemy2D _enemyData = enemy.GetComponent<Enemy2D>();
            _enemyData.player = _playerGameObject;
            _enemyData.playerTransform = _playerTransform;
        }
    }
}
