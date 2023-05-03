using System.Collections.Generic;

[System.Serializable]
public class WaveManifest
{
    public List<EnemySpawn> EnemySpawns;
}

[System.Serializable]
public struct EnemySpawn
{
    public float x;
    public float y;
    public Enemy2D enemyPrefab;
}
