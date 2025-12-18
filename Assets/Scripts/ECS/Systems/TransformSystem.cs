using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    public partial struct TransformSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            new MoveJob
            {
                DeltaTime = deltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;

        void Execute(ref LocalTransform transform, in Direction dir, in MovementSpeed speed)
        {
            transform.Position += dir.Value * speed.Value * DeltaTime;
            
            if (dir.isOriented && math.lengthsq(dir.Value) > 0.001f)
            {
                var angle = math.atan2(dir.Value.y, dir.Value.x);
                transform.Rotation = quaternion.Euler(0, 0, angle - math.radians(90));
            } else if (dir.isFlipFace && math.lengthsq(dir.Value) > 0.001f)
            {
                if (dir.Value.x < 0)
                {
                    transform.Rotation = quaternion.Euler(0, math.radians(180), 0);
                }
                else
                {
                    transform.Rotation = quaternion.Euler(0, 0, 0);
                }
            }
        }
    }
}