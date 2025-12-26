using UnityEngine;
using Unity.Entities;

namespace Swarm.ECS
{
    [DisallowMultipleComponent]
    public class EnemySpawnerAuthoring : MonoBehaviour
    {
        public Data.EnemySpawnerConfig config;

        public class Baker : Baker<EnemySpawnerAuthoring>
        {
            public override void Bake(EnemySpawnerAuthoring authoring)
            {
                var config = authoring.config;

                var singletonEntity = GetEntity(TransformUsageFlags.Dynamic);
                var enemyPrefabEntity = GetEntity(config.EnemyPrefab, TransformUsageFlags.Dynamic);

                AddComponent(singletonEntity, new EnemySpawnerConfig
                {
                    EnemyPrefab = enemyPrefabEntity,
                    Count = config.Count,
                    Cooldown = config.Cooldown,
                    DistanceMin = config.DistanceMin,
                    DistanceMax = config.DistanceMax
                });
                AddComponent<Components.EnemySpawner>(singletonEntity);
            }
        }
    }
}