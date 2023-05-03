using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelManifest", menuName = "ScriptableObjects/Level Manifest", order = 1)]

public class LevelManifest : ScriptableObject
{
    public uint LevelNumber = 1;
    public int SceneIndex { get; private set; }
    public string SceneName = "";
    public uint FinalWave = 2;

    void Start()
    {
        SceneIndex = SceneManager.GetSceneByName(SceneName).buildIndex;
    }

    // eventually add wave specific details here to be loaded in the scene
}
