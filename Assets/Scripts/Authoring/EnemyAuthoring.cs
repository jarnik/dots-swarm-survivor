using Swarm.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Swarm.ECS
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public class Baker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<EnemyTag>(entity);
                AddComponent<Direction>(entity);
                AddComponent<MovementSpeed>(entity);
                AddComponent<Prefab>(entity);
                AddComponent<Health>(entity);

                var buffer = AddBuffer<DamageEvent>(entity);

                SetComponent(entity, new MovementSpeed { Value = 1f });
                SetComponent(entity, new Health { Value = 100f });
            }
        }
    }
}