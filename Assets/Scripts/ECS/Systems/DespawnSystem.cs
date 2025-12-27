using Unity.Burst;
using Unity.Entities;
using Swarm.ECS.Components;
using Unity.Jobs;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public partial struct DespawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            var despawnDeadJob = new DespawnDead
            {
                DeltaTime = deltaTime,
                Ecb = ecb.AsParallelWriter()
            }.ScheduleParallel(state.Dependency);
            despawnDeadJob.Complete();

            var despawnByLifeJob = new DespawnByLifeJob
            {
                DeltaTime = deltaTime,
                Ecb = ecb.AsParallelWriter()
            }.ScheduleParallel(state.Dependency);

            // var combinedDeps = JobHandle.CombineDependencies(despawnDeadJob, despawnByLifeJob);

            state.Dependency = despawnByLifeJob;
        }

        [BurstCompile]
        public partial struct DespawnDead : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter Ecb;

            void Execute([ChunkIndexInQuery] int sortKey, Entity entity, in DeadTag deadTag)
            {
                Ecb.DestroyEntity(sortKey, entity);
            }
        }

        [BurstCompile]
        public partial struct DespawnByLifeJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter Ecb;

            void Execute([ChunkIndexInQuery] int sortKey, Entity entity, ref Lifetime lifetime)
            {
                lifetime.Life -= DeltaTime * lifetime.DecayRate;
                if (lifetime.Life <= 0)
                {
                    // Record destroy; ECB will perform it safely on main thread
                    Ecb.DestroyEntity(sortKey, entity);
                }
            }
        }
    }
}