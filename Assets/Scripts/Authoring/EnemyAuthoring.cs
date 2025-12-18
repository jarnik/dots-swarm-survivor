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

                SetComponent(entity, new MovementSpeed { Value = 0.3f });
            }
        }
    }
}