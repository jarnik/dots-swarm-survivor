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
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerConfig>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var config = SystemAPI.GetSingleton<PlayerConfig>();

            new MoveJob
            {
                DeltaTime = deltaTime,
                ScreenWidth = config.ScreenWidth,
                ScreenHeight = config.ScreenHeight
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;
        public float ScreenWidth;
        public float ScreenHeight;

        void Execute(ref LocalTransform transform, in Direction dir, in MovementSpeed speed)
        {
            transform.Position += dir.Value * speed.Value * DeltaTime;
            
            if (dir.isLimitedToScreen)
            {
                transform.Position.x = math.clamp(transform.Position.x, -ScreenWidth/2, ScreenWidth/2);
                transform.Position.y = math.clamp(transform.Position.y, -ScreenHeight/2, ScreenHeight/2);
            }
            
            if (dir.isOriented && math.lengthsq(dir.Value) > 0.001f)
            {
                var angle = math.atan2(dir.Value.y, dir.Value.x);
                transform.Rotation = quaternion.Euler(0, 0, angle - math.radians(90));
            }
            else if (dir.isFlipFace && math.lengthsq(dir.Value) > 0.001f)
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