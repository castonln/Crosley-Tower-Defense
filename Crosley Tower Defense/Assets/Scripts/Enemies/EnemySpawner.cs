using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Lanes")]
    [SerializeField] private Lane rightLane;
    [SerializeField] private Lane leftLane;
    [SerializeField] private Lane topRightLane;
    [SerializeField] private Lane topLeftLane;

    [Header("References")]
    [SerializeField] private Transform shootTarget;
    [SerializeField] private GameObject enemyGroup;
    [SerializeField] private WaveData[] waves;

    private WaveData wave;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private bool isWaveActive = false;
    private int waveCountMultiplier = 1;
    private float timeSinceWaveStart = 0f;

    private SubWaveInfo subWave;
    private int currentSubWaveIndex = 0;
    private float timeSinceLastSpawn = 0f;
    private int enemiesToSpawnInSubwave = 0;

    public static event Action OnWaveEnd;

    private int waveTotalUI = 1;

    public static EnemySpawner main;
    private void Awake()
    {
        main = this;
    }

    private bool AreEnemiesAlive()
    {
        Lane[] lanes = { rightLane, leftLane, topRightLane, topLeftLane };
        
        foreach (Lane lane in lanes)
        {
            if (lane.AreEnemiesAlive()) return true;
        }

        return false;
    }

    private void Update()
    {
        if (!isWaveActive) return;

        timeSinceWaveStart += Time.deltaTime;

        if (!isSpawning)
        {
            if (!AreEnemiesAlive()) WaveEnd();
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= subWave.secondsBetweenEnemies)
        {
            if (enemiesToSpawnInSubwave > 0)
            {
                GameObject enemy = SpawnEnemy(subWave.enemyPrefab, GetLaneFromSpawnSide(subWave.spawnSide).SpawnPoint);

                if (enemy && enemy.TryGetComponent<Triceracopter>(out Triceracopter spawnedTriceracopter))
                {
                    spawnedTriceracopter.SetHealth((int)spawnedTriceracopter.GetHealth() * waveCountMultiplier);
                }

                enemiesToSpawnInSubwave--;
                timeSinceLastSpawn = 0f;
            }
            else
            {
                StartNextSubWave();
            }
        }

    }

    public void WaveStart()
    {
        if (currentWaveIndex >= waves.Length)
        {
            ResetWavesAndIncreaseWaveCountMultiplier();
        }

        wave = waves[currentWaveIndex];
        subWave = wave.subWaves[currentSubWaveIndex];
        SetEnemiesToSpawnInSubwave();
        timeSinceLastSpawn = subWave.secondsBetweenEnemies;
        timeSinceWaveStart = 0f;
        isSpawning = true;
        isWaveActive = true;
    }

    private void WaveEnd()
    {
        isWaveActive = false;
        isSpawning = false;
        currentWaveIndex++;
        currentSubWaveIndex = 0;
        timeSinceLastSpawn = 0f;
        timeSinceWaveStart = 0f;
        waveTotalUI++;
        OnWaveEnd?.Invoke();
        print("Wave ended");
    }

    private void ResetWavesAndIncreaseWaveCountMultiplier()
    {
        currentWaveIndex = 0;
        waveCountMultiplier += 1;
    }

    private void StartNextSubWave()
    {
        currentSubWaveIndex++;
        if (currentSubWaveIndex >= wave.subWaves.Length)
        {
            isSpawning = false;
            
        } else
        {
            subWave = wave.subWaves[currentSubWaveIndex];
            SetEnemiesToSpawnInSubwave();
        }
        timeSinceLastSpawn = subWave.secondsBetweenEnemies;
    }
    private Lane GetLaneFromSpawnSide(SpawnSide spawnSide)
    {
        return spawnSide switch
        {
            SpawnSide.left => leftLane,
            SpawnSide.right => rightLane,
            SpawnSide.topRight => topRightLane,
            SpawnSide.topLeft => topLeftLane,
            _ => leftLane
        };
    }

    private GameObject SpawnEnemy(GameObject prefab, Transform spawnPoint)
    {
        if (prefab == null) return null;

        GameObject enemy = Instantiate(
            prefab,
            spawnPoint.position,
            spawnPoint.rotation,
            enemyGroup.transform
           );

        enemy.GetComponent<Triceracopter>().SetTarget(shootTarget);
        enemy.GetComponentInChildren<SpriteRenderer>().sortingOrder = GetLaneFromSpawnSide(subWave.spawnSide).GetNumberOfEnemies();
        enemy.GetComponent<Triceracopter>().SetLane(GetLaneFromSpawnSide(subWave.spawnSide));
        GetLaneFromSpawnSide(subWave.spawnSide).AddEnemy(enemy);

        if (subWave.spawnSide == SpawnSide.left || subWave.spawnSide == SpawnSide.topLeft)
            FlipHorizontally(enemy.transform);
        return enemy;
    }

    // we don't multiply empty events that wait for spawning
    private void SetEnemiesToSpawnInSubwave()
    {
        enemiesToSpawnInSubwave = subWave.enemyPrefab != null ? subWave.count * waveCountMultiplier : subWave.count;
    }

    private void FlipHorizontally(Transform objectTransform)
    {
        Vector3 newScale = objectTransform.localScale;
        newScale.x *= -1;
        objectTransform.localScale = newScale;
    }

    public bool IsWaveActive()
    {
        return isWaveActive;
    }

    public int GetWaveTotalUI()
    {
        return waveTotalUI;
    }

    public float GetTimeSinceWaveStart()
    {
        return timeSinceWaveStart;
    }
}
