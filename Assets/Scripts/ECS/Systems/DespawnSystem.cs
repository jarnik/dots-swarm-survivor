using Unity.Burst;
using Unity.Entities;
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

            var despawnProjectilesJob = new DespawnProjectilesJob
            {
                DeltaTime = deltaTime,
                Ecb = ecb.AsParallelWriter()
            }.ScheduleParallel(state.Dependency);

            despawnProjectilesJob.Complete();

            var despawnEnemiesJob = new DespawnEnemiesJob
            {
                DeltaTime = deltaTime,
                Ecb = ecb.AsParallelWriter()
            }.ScheduleParallel(state.Dependency);

            state.Dependency = despawnEnemiesJob;
        }

        [BurstCompile]
        public partial struct DespawnProjectilesJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter Ecb;

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

        [BurstCompile]
        public partial struct DespawnEnemiesJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter Ecb;

            void Execute([ChunkIndexInQuery] int sortKey, Entity entity, ref Health health, in EnemyTag tag)
            {
                health.Value -= DeltaTime;
                if (health.Value <= 0)
                {
                    // Record destroy; ECB will perform it safely on main thread
                    Ecb.DestroyEntity(sortKey, entity);
                }
            }
        }
    }
}