using Unity.Entities;
using Unity.Mathematics;

namespace Swarm.ECS.Components
{
    public struct PlayerData : IComponentData
    {
        public float3 Position;
    }
}