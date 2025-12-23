using Unity.Entities;

namespace Swarm.ECS
{
    public struct ProjectileSpawnerConfig : IComponentData
    {
        public Entity ProjectilePrefab;
        public float Cooldown;
        public float DistanceMax;
    }
}