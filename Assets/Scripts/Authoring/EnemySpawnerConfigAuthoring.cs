using UnityEngine;
using Unity.Entities;
using Swarm.ECS.Components;

namespace Swarm.ECS
{
    [DisallowMultipleComponent]
    public class EnemySpawnerConfigAuthoring : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        public int Count = 1000;
        public float Cooldown = 1f;
        public float DistanceMin = 5f;
        public float DistanceMax = 20f;

        public class Baker : Baker<EnemySpawnerConfigAuthoring>
        {
            public override void Bake(EnemySpawnerConfigAuthoring authoring)
            {
                var singletonEntity = GetEntity(TransformUsageFlags.Dynamic);
                var enemyPrefabEntity = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic);

                AddComponent(singletonEntity, new EnemySpawnerConfig
                {
                    EnemyPrefab = enemyPrefabEntity,
                    Count = authoring.Count,
                    Cooldown = authoring.Cooldown,
                    DistanceMin = authoring.DistanceMin,
                    DistanceMax = authoring.DistanceMax
                });
                AddComponent<EnemySpawner>(singletonEntity);
            }
        }
    }
}