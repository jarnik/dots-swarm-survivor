using UnityEngine;
using Unity.Entities;

namespace Swarm.ECS
{
    [DisallowMultipleComponent]
    public class SpawnerConfigAuthoring : MonoBehaviour
    {
        public GameObject EnemyPrefab;
        public int Count = 1000;

        public class Baker : Baker<SpawnerConfigAuthoring>
        {
            public override void Bake(SpawnerConfigAuthoring authoring)
            {
                // Use the authoring GameObject itself as the singleton entity
                var singletonEntity = GetEntity(TransformUsageFlags.None);
                var enemyPrefabEntity = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic);

                AddComponent(singletonEntity, new SpawnerConfig
                {
                    EnemyPrefab = enemyPrefabEntity,
                    Count = authoring.Count
                });
            }
        }
    }
}