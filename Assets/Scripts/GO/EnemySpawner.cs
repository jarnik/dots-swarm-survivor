using UnityEngine;

namespace Swarm.GO
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Data.EnemySpawnerConfig _config;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _spawnOrigin;

        public int GetEnemyCount() => transform.childCount;

        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _config.Cooldown)
            {
                _timer = 0f;
                SpawnEnemies();
            }
        }
        
        private void SpawnEnemies()
        {
            for (int i = 0; i < _config.CountGO; i++)
            {
                float distance = Random.Range(_config.DistanceMin, _config.DistanceMax);
                float angle = Random.Range(0f, 360f);
                Vector3 direction = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    0f,
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                );
                Vector3 spawnPosition = _spawnOrigin.position + direction * distance;

                var enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity, transform);
                enemy.Initialize(_playerTransform);
            }
        }
        
    }
}
