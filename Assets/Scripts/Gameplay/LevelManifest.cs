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

    public WaveManifest GetWaveManifest(int waveIndex)
    {
        SceneIndex = SceneManager.GetSceneByName(SceneName).buildIndex;
        FinalWave = Waves.Count;

        if (waveIndex < 1 || waveIndex > Waves.Count)
        {
            throw new System.ArgumentOutOfRangeException("Invalid wave index");
        }

        return Waves[waveIndex - 1];
    }
}
