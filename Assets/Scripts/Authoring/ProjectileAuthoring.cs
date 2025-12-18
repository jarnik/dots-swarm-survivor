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
                AddComponent<Direction>(entity);
                AddComponent<MovementSpeed>(entity);
                SetComponent(entity, new MovementSpeed { Value = 5f });
            }
        }
    }
}