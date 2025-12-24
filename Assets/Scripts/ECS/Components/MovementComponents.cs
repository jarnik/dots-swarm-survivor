using Unity.Entities;
using Unity.Mathematics;

namespace Swarm.ECS.Components
{
    public struct Direction : IComponentData
    {
        public float3 Value;
        public bool isOriented;
        public bool isFlipFace;
        public bool isLimitedToScreen;
    }

    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }
}