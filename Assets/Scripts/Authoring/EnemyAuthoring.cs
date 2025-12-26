using Swarm.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Swarm.ECS
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public Data.EnemyConfig config;

        public class Baker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var config = authoring.config;

                AddComponent<EnemyTag>(entity);
                AddComponent<Direction>(entity);
                AddComponent<MovementSpeed>(entity);
                AddComponent<Prefab>(entity);
                AddComponent(entity, new Lifetime { Life = 1f });

                var buffer = AddBuffer<DamageEvent>(entity);

                SetComponent(entity, new MovementSpeed { Value = config.Speed });
            }
        }
    }
}