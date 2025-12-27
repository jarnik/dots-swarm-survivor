using Swarm.GO;
using UnityEngine;

namespace Swarm.Runtime
{
    public class StatsGO : StatsProviderBase
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private ProjectileSpawner _projectileSpawner;

        private float _fps;

        public override int GetObjectCount() => _enemySpawner.GetEnemyCount() + _projectileSpawner.GetProjectileCount();

        private void Update()
        {
            _fps = 1.0f / Time.unscaledDeltaTime;
        }

        public override float GetFPS() => _fps;

        public override Mode GetCurrentMode() => Mode.GameObject;
    }
}
