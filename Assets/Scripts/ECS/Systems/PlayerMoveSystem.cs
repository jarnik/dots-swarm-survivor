using Unity.Entities;
using Unity.Mathematics;
using Swarm.ECS.Components;
using Unity.Burst;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    public partial struct PlayerMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (dir, input) in
                    SystemAPI.Query<RefRW<Direction>, RefRO<PlayerInput>>()
                    .WithAll<PlayerTag>())
            {
                dir.ValueRW.Value = new float3(input.ValueRO.Movement.x, input.ValueRO.Movement.y, 0);
            }
        }
    }
}