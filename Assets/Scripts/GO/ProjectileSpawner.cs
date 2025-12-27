using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Swarm.GO
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private Data.PlayerConfig _playerConfig;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _spawnPoint;

        private ObjectPool<Projectile> _projectilePool;

        private float _timer;
        private List<Projectile> _projectiles = new List<Projectile>();

        public int GetProjectileCount() => _projectiles.Count;

        private void Start()
        {
            Debug.Assert(_playerConfig != null, "Player config is not assigned.");
            Debug.Assert(_projectilePrefab != null, "Projectile prefab is not assigned.");

            _projectiles = new List<Projectile>();
            _projectilePool = new ObjectPool<Projectile>(CreateProjectile);
        }

        private Projectile CreateProjectile()
        {
            var projectile = Instantiate(_projectilePrefab, _spawnPoint.position, Quaternion.identity, transform);
            return projectile;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _playerConfig.ProjectileCooldown)
            {
                _timer = 0f;
                SpawnProjectiles();
            }
        }
        
        private void OnProjectileDestroyed(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            _projectiles.Remove(projectile);
            _projectilePool.Release(projectile);
        }
        
        private void SpawnProjectiles()
        {
            var enemies = _enemySpawner.Enemies;
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                var distance = Vector3.Distance(enemy.transform.position, _spawnPoint.position);
                if (distance > _playerConfig.ProjectileMaxDistance)
                {
                    continue;
                }

                Vector3 direction = (enemy.transform.position - _spawnPoint.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(direction);

                var projectile = _projectilePool.Get();
                projectile.transform.position = _spawnPoint.position;
                projectile.transform.rotation = rotation;
                projectile.gameObject.SetActive(true);

                projectile.Reset();
                projectile.onDestroyed = OnProjectileDestroyed;
                projectile.onHitEnemy = _enemySpawner.OnEnemyDestroyed;

                _projectiles.Add(projectile);
            }
        }
    }
}