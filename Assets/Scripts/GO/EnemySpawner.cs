using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Swarm.GO
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Data.EnemySpawnerConfig _config;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _spawnOrigin;

        public int GetEnemyCount() => transform.childCount;
        public IReadOnlyList<Enemy> Enemies => _enemies;

        private ObjectPool<Enemy> _enemyPool;

        private float _timer;
        private List<Enemy> _enemies = new List<Enemy>();

        private void Start()
        {
            Debug.Assert(_config != null, "Enemy spawner config is not assigned.");
            Debug.Assert(_enemyPrefab != null, "Enemy prefab is not assigned.");

            _enemyPool = new ObjectPool<Enemy>(CreateEnemy);
        }

        private Enemy CreateEnemy()
        {
            var enemy = Instantiate(_enemyPrefab, Vector3.zero, Quaternion.identity, transform);
            enemy.Initialize(_playerTransform);
            return enemy;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _config.Cooldown)
            {
                _timer = 0f;
                SpawnEnemies();
            }
        }

        public void OnEnemyDestroyed(Enemy enemy)
        {
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
                _enemyPool.Release(enemy);
                enemy.gameObject.SetActive(false);
            }
        }
        
        private void SpawnEnemies()
        {
            for (int i = 0; i < _config.CountGO; i++)
            {
                if (_enemies.Count >= _config.CountGOMax)
                    break;

                float distance = Random.Range(_config.DistanceMin, _config.DistanceMax);
                float angle = Random.Range(0f, 360f);
                Vector3 direction = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    0f,
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                );
                Vector3 spawnPosition = _spawnOrigin.position + direction * distance;

                var enemy = _enemyPool.Get();
                enemy.transform.position = spawnPosition;
                enemy.transform.rotation = Quaternion.identity;
                enemy.gameObject.SetActive(true);

                enemy.Initialize(_playerTransform);

                _enemies.Add(enemy);
            }
        }
        
    }
}
