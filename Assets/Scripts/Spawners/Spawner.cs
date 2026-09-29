using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners {
    public class Spawner : MonoBehaviour {
        [SerializeField] private float spawnRate;
        [SerializeField] private List<GameObject> enemyPrefabs = new();

        private IEnumerator SpawnRoutine() {
            yield return new WaitForSeconds(spawnRate);
            SpawnRandomEnemyAtRandomMapLocation();
        }

        private void SpawnRandomEnemyAtRandomMapLocation() {
            if (enemyPrefabs.Count == 0) return;
            
            var randomEnemyIndex = Random.Range(0, enemyPrefabs.Count);
        }
    }
}
