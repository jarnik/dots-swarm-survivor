using Swarm.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Swarm.ECS
{
    public class PlayerSpawnerAuthoring : MonoBehaviour
    {
        public Data.PlayerConfig _config;
        public GameObject _projectilePrefab;

        public class Baker : Baker<PlayerSpawnerAuthoring>
        {
            public override void Bake(PlayerSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<ProjectileSpawner>(entity);

                var projectilePrefabEntity = GetEntity(authoring._projectilePrefab, TransformUsageFlags.Dynamic);

                AddComponent(entity, new ProjectileSpawnerConfig
                {
                    ProjectilePrefab = projectilePrefabEntity,
                    Cooldown = authoring._config.ProjectileCooldown,
                    DistanceMax = authoring._config.ProjectileMaxDistance
                });
            }
        }
    }
}