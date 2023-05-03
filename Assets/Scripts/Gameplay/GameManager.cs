using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager UIManager;
    public LevelManifest LevelManifest;
    public uint FinalWave { get; private set; }
    public uint CurrentWave { get; private set; } = 0;
    public uint KillCount { get; private set; } = 0;
    public static bool GamePaused = false;
    // add enemy stats for spawning and such here later
    
    private void Start()
    {
        CurrentWave = 1;
        FinalWave = LevelManifest.FinalWave;
    }

    public void EnemyKilled()
    {
        // GameManager will load a level manifest for total enemies, enemies per wave, etc.
        KillCount++;
    }

    public void EndGameDefeat()
    {
        UIManager.OpenDeathPanel();
    }
}
