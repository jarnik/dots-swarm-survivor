using UnityEngine;
using Unity.Entities;

namespace Swarm.ECS
{
    [DisallowMultipleComponent]
    public class ProjectileSpawnerConfigAuthoring : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public float Cooldown;
        public float DistanceMax;

        public class Baker : Baker<ProjectileSpawnerConfigAuthoring>
        {
            public override void Bake(ProjectileSpawnerConfigAuthoring authoring)
            {
                var singletonEntity = GetEntity(TransformUsageFlags.None);
                var projectilePrefabEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic);

                AddComponent(singletonEntity, new ProjectileSpawnerConfig
                {
                    ProjectilePrefab = projectilePrefabEntity,
                    Cooldown = authoring.Cooldown,
                    DistanceMax = authoring.DistanceMax
                });
            }
        }
    }
}