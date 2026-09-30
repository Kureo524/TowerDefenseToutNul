using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Spawners
{
    // [NEW] Spawns enemies in waves.  Existing fields kept.
    public class Spawner : MonoBehaviour
    {
        // ---- Existing fields ----
        [SerializeField] private float spawnRate;
        [SerializeField] private List<GameObject> enemyPrefabs = new();

        // [NEW] Wave settings.
        [SerializeField] private float waveDelay = 5f;
        [SerializeField] private int enemiesPerWave = 5;
        [SerializeField] private Transform[] spawnPoints;

        // [NEW] Goal: where enemies walk towards.
        [SerializeField] private Transform goal;

        private int _currentWave;
        private bool _isSpawning;
        private Coroutine _waveCoroutine;

        public int CurrentWave => _currentWave;
        public bool IsSpawning => _isSpawning;

        private void Start()
        {
            // [NEW] Tell all enemies where to go.
            Enemy.Goal = goal;
            StartWaves();
        }

        // ---- Existing SpawnRoutine (kept for reference) ----
        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnRandomEnemyAtRandomMapLocation();
        }

        // ---- Wave system ----
        public void StartWaves()
        {
            if (_waveCoroutine != null)
                StopCoroutine(_waveCoroutine);
            _waveCoroutine = StartCoroutine(WaveLoop());
        }

        private IEnumerator WaveLoop()
        {
            _currentWave = 0;
            while (true)
            {
                _currentWave++;
                _isSpawning = true;

                for (int i = 0; i < enemiesPerWave; i++)
                {
                    SpawnRandomEnemyAtRandomMapLocation();
                    yield return new WaitForSeconds(0.5f);
                }

                _isSpawning = false;
                yield return new WaitForSeconds(waveDelay);
            }
        }

        private void SpawnRandomEnemyAtRandomMapLocation()
        {
            if (enemyPrefabs.Count == 0) return;

            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject prefab = enemyPrefabs[randomIndex];

            Vector2 spawnPos = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
            enemy.SetActive(true);
        }

        private Vector2 GetRandomSpawnPosition()
        {
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                int idx = Random.Range(0, spawnPoints.Length);
                return spawnPoints[idx].position;
            }

            // [NEW] Fallback: random position in the scene.
            float x = Random.Range(-8f, 8f);
            float y = Random.Range(-4f, 4f);
            return new Vector2(x, y);
        }
    }
}
