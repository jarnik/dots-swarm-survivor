using Swarm.Data;
using Swarm.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Swarm.ECS
{
    public class ProjectileAuthoring : MonoBehaviour
    {
        public ProjectileConfig config;

        public class Baker : Baker<ProjectileAuthoring>
        {
            public override void Bake(ProjectileAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var config = authoring.config;

                AddComponent<ProjectileTag>(entity);
                AddComponent<Direction>(entity);
                AddComponent<Prefab>(entity);
                AddComponent(entity, new Lifetime { Life = config.Lifetime, DecayRate = 1f });
                AddComponent(entity, new MovementSpeed { Value = config.Speed });
            }
        }
    }
}