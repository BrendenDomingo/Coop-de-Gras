using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager UIManager;
    public int FinalWave = 3;
    public int CurrentWave { get; private set; } = 0;
    public static bool GamePaused = false;
    // add enemy stats for spawning and such here later
    
    private void Start()
    {
        CurrentWave = 1;
    }

    public void EndGame()
    {
        // eventually add game over / death UI
        UIManager.OpenMainMenuScene();
    }
}
