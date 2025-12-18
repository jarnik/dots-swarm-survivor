using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Swarm.ECS.Components;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    public partial struct EnemyAISystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
            {
                return;
            }

            var playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
            float3 playerPos = playerTransform.Position;

            new CalculateDirectionJob
            {
                PlayerPos = playerPos
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct CalculateDirectionJob : IJobEntity
    {
        public float3 PlayerPos;

        void Execute(ref Direction dir, ref MovementSpeed speed, in LocalTransform transform, in EnemyTag tag)
        {
            float3 delta = PlayerPos - transform.Position;
            float distanceSq = math.lengthsq(delta);

            // Only normalize if the distance is greater than a very small epsilon
            if (distanceSq > 0.001f)
            {
                dir.Value = delta / math.sqrt(distanceSq);
            }
            else
            {
                dir.Value = float3.zero;
            }

            speed.Value = 0.5f;
        }
    }
}