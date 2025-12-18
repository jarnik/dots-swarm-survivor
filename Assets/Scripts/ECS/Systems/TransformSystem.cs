using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Entities;
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
            // Apply movement: Position + (Direction * Speed * Time)
            
            transform.Position += dir.Value * speed.Value * DeltaTime;

            // // Optional: Make enemies face the player
            // transform.Rotation = quaternion.LookRotationSafe(dir.Value, math.up());
        }
    }
}