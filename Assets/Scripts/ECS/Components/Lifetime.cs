using Unity.Entities;

namespace Swarm.ECS.Components
{
    public struct Lifetime : IComponentData
    {
        public float Life;
        public float DecayRate;
    }
}