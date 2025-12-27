using Swarm.GO;
using UnityEngine;

namespace Swarm.Runtime
{
    public class StatsGO : StatsProviderBase
    {
        [SerializeField] private EnemySpawner _enemySpawner;

        private float _fps;

        private void Awake()
        {
        }

        public override int GetObjectCount() => _enemySpawner.GetEnemyCount();

        private void Update()
        {
            _fps = 1.0f / Time.unscaledDeltaTime;
        }

        public override float GetFPS() => _fps;

        public override Mode GetCurrentMode() => Mode.GameObject;
    }
}
