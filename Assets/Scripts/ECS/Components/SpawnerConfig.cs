using Unity.Entities;

namespace Swarm.ECS
{
    public struct SpawnerConfig : IComponentData
    {
        public Entity EnemyPrefab;
        public int Count;
    }
}