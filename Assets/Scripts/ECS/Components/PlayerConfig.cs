using Unity.Entities;

namespace Swarm.ECS.Components
{
    public struct PlayerConfig : IComponentData
    { 
        public float ScreenWidth;
        public float ScreenHeight;
    }
}