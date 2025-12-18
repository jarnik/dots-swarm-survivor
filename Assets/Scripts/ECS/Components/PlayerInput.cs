using Unity.Entities;
using Unity.Mathematics;

namespace Swarm.ECS.Components
{
    public struct PlayerInput : IComponentData
    {
        public float2 Movement;
    }
}