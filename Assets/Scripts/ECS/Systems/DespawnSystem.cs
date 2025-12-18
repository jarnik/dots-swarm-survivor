using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Swarm.ECS.Components;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    public partial struct DespawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            // Get ECB from EndSimulation for safe playback after jobs finish
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            var handle = new DespawnJob
            {
                DeltaTime = deltaTime,
                Ecb = ecb.AsParallelWriter()
            }.ScheduleParallel(state.Dependency);

            // Update the system dependency
            state.Dependency = handle;
        }

        [BurstCompile]
        public partial struct DespawnJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter Ecb;

            // we exclude Prefab tags
            void Execute([ChunkIndexInQuery] int sortKey, Entity entity, ref Lifetime lifetime, in ProjectileTag tag)
            {
                lifetime.Life -= DeltaTime;
                if (lifetime.Life <= 0)
                {
                    // Record destroy; ECB will perform it safely on main thread
                    Ecb.DestroyEntity(sortKey, entity);
                }
            }
        }
    }
}