using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(HashGridSystem))]
    public partial struct ProjectileSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            JobHandle spawnJob = default;

            foreach (var (spawner, transform, entity) in
                    SystemAPI.Query<RefRW<ProjectileSpawner>, RefRO<LocalTransform>>()
                    .WithEntityAccess())
            {
                spawner.ValueRW.Timer -= deltaTime;
                if (spawner.ValueRW.Timer <= 0)
                {
                    spawner.ValueRW.Timer = spawner.ValueRW.Cooldown;

                    // For a showcase: Fire at every enemy currently in the world
                    // In a real game, you'd use the HashGrid to find the nearest N enemies
                    spawnJob = new SpawnProjectileJob
                    {
                        ECB = ecb,
                        Prefab = spawner.ValueRO.ProjectilePrefab,
                        Origin = transform.ValueRO.Position
                    }.ScheduleParallel(state.Dependency);

                    state.Dependency = spawnJob;
                }
            }

        }
    }

    [BurstCompile]
    public partial struct SpawnProjectileJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public Entity Prefab;
        public float3 Origin;

        // We use [ChunkIndexInQuery] for the ParallelWriter
        void Execute([ChunkIndexInQuery] int chunkIndex, in LocalTransform enemyTransform, in EnemyTag tag)
        {
            Entity projectile = ECB.Instantiate(chunkIndex, Prefab);

            float3 delta = enemyTransform.Position - Origin;
            float distanceSq = math.lengthsq(delta);

            float3 direction;
            // Only normalize if the distance is greater than a very small epsilon
            if (distanceSq > 0.001f)
            {
                direction = delta / math.sqrt(distanceSq);
            }
            else
            {
                direction = float3.zero;
            }

            // Set initial position and rotation
            ECB.SetComponent(chunkIndex, projectile, LocalTransform.FromPosition(Origin));
            ECB.SetComponent(chunkIndex, projectile, new Direction { Value = direction, isOriented = true });
            ECB.SetComponent(chunkIndex, projectile, new Lifetime { Life = 1.0f });
        }
    }
}