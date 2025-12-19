
using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

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
            foreach (var (health, damageBuffer) in
                    SystemAPI.Query<RefRW<Health>, DynamicBuffer<DamageEvent>>())
            {
                float total = 0f;
                for (int i = 0; i < damageBuffer.Length; i++)
                    total += damageBuffer[i].Value;

                if (total > 0f)
                {
                    health.ValueRW.Value = math.max(0f, health.ValueRO.Value - total);
                    damageBuffer.Clear();
                }
            }
        }
    }
}