using Swarm.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Swarm.ECS
{
    public class ProjectileAuthoring : MonoBehaviour
    {
        public class Baker : Baker<ProjectileAuthoring>
        {
            public override void Bake(ProjectileAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<ProjectileTag>(entity);
                SetComponent(entity, new ProjectileTag { Damage = 120f });

                AddComponent<Direction>(entity);
                AddComponent<Lifetime>(entity);
                SetComponent(entity, new Lifetime { Life = 1f });

                AddComponent<MovementSpeed>(entity);
                SetComponent(entity, new MovementSpeed { Value = 2f });

                AddComponent<Prefab>(entity);
            }
        }
    }
}