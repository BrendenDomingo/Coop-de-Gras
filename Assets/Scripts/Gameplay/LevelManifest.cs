using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelManifest", menuName = "ScriptableObjects/Level Manifest", order = 1)]

public class LevelManifest : ScriptableObject
{
    public List<WaveManifest> Waves;
    public int LevelNumber = 1;
    public int SceneIndex { get; private set; }
    public string SceneName = "";
    public int FinalWave { get; private set; }

    void Start()
    {
        SceneIndex = SceneManager.GetSceneByName(SceneName).buildIndex;
        FinalWave = Waves.Count;
    }

    public WaveManifest GetWaveManifest(int waveIndex)
    {
        if (waveIndex < 1 || waveIndex > FinalWave)
        {
            throw new System.ArgumentOutOfRangeException("Invalid wave index");
        }

        return Waves[waveIndex];
    }
}
