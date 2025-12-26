using Unity.Collections;
using Unity.Entities;

namespace Swarm.ECS.Components
{
    public struct EnemyHashGrid : IComponentData
    {
        public NativeParallelMultiHashMap<uint, Entity> Grid;
    }
}