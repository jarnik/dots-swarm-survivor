using Unity.Entities;
using UnityEngine;

namespace Swarm.Runtime
{
    public class StatsECS : StatsProviderBase
    {
        private EntityManager _entityManager;
        private float _fps;

        private void Awake()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Debug.Assert(_entityManager != null, "EntityManager is null in StatsECS");
        }

        public override int GetObjectCount()
        {
            return _entityManager.UniversalQuery.CalculateEntityCount();
        }

        private void Update()
        {
            _fps = 1.0f / Time.unscaledDeltaTime;
        }

        public override float GetFPS() => _fps;

        public override Mode GetCurrentMode() => Mode.ECS;
    }
}
