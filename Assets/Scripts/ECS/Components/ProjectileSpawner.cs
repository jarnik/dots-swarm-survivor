using Unity.Entities;

namespace Swarm.ECS.Components
{
    public struct ProjectileSpawner : IComponentData
    {
        public float Timer;
        public float Cooldown; // e.g., 2.0s
        public Entity ProjectilePrefab;
    }
}