using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Entities;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
    public partial struct ApplyDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (life, damageBuffer) in
                    SystemAPI.Query<RefRW<Lifetime>, DynamicBuffer<DamageEvent>>())
            {
                if (damageBuffer.Length > 0)
                {
                    life.ValueRW.Life = 0;
                    damageBuffer.Clear();
                    break;
                }
            }
        }
    }
}