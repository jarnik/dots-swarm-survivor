using Swarm.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Swarm.ECS
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public GameObject ProjectilePrefab;

        public class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTag>(entity);
                AddComponent<PlayerInput>(entity);
                AddComponent<Direction>(entity);

                AddComponent<MovementSpeed>(entity);
                SetComponent(entity, new MovementSpeed { Value = 5f });

                AddComponent<ProjectileSpawner>(entity);
                var projectileEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic);
                SetComponent(entity, new ProjectileSpawner { Cooldown = 0.2f, Timer = 0f, ProjectilePrefab = projectileEntity });
            }
        }
    }
}