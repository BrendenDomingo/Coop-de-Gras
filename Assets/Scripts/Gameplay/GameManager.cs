using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager UIManager;
    public LevelManifest LevelManifest;
    public int FinalWave { get; private set; }
    public int CurrentWave { get; private set; } = 1;
    public int KillCount { get; private set; } = 0;
    public static bool GamePaused = false;
    private WaveManifest _currentWave;
    private int _enemiesRemaining = 0;
    private bool _gameInProgress = true;
    
    private void Start()
    {
        FinalWave = LevelManifest.FinalWave;
        LoadWave();
    }

    private void Update()
    {
        if (_enemiesRemaining <= 0 && _gameInProgress)
        {
            if (CurrentWave < FinalWave)
            {
               WaveComplete(); 
            }
            else
            {
               EndGameVictory(); 
            }
        }
    }

    public void EnemyKilled()
    {
        // GameManager will load a level manifest for total enemies, enemies per wave, etc.
        KillCount++;
        _enemiesRemaining--;
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
    }

    private void LoadWave()
    {
        _currentWave = LevelManifest.GetWaveManifest(CurrentWave);
        _enemiesRemaining = _currentWave.EnemySpawns.Count;
        foreach(EnemySpawn enemySpawn in _currentWave.EnemySpawns)
        {
            Vector3 position = new Vector3(enemySpawn.x, enemySpawn.y, 0f);
            GameObject enemy = (GameObject)Instantiate(enemySpawn.enemyPrefab.gameObject, position, Quaternion.identity);
        }
    }
}
