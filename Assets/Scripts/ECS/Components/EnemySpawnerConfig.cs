using Unity.Entities;

namespace Swarm.ECS
{
    public struct EnemySpawnerConfig : IComponentData
    {
        public Entity EnemyPrefab;
        public int Count;
        public float Cooldown;
        public float DistanceMin;
        public float DistanceMax;
    }
}