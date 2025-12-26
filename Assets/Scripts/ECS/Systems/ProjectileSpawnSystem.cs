using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    public partial struct ProjectileSpawnSystem : ISystem
    {
        private void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            foreach (var (spawner, entity) in
                    SystemAPI.Query<RefRW<ProjectileSpawner>>()
                    .WithEntityAccess())
            {
                spawner.ValueRW.Timer -= deltaTime;

                if (spawner.ValueRW.Timer <= 0)
                {
                    var config = SystemAPI.GetSingleton<ProjectileSpawnerConfig>();

                    spawner.ValueRW.Timer = config.Cooldown;

                    if (!SystemAPI.TryGetSingletonEntity<PlayerData>(out Entity playerDataEntity))
                    {
                        return;
                    }
                    float3 playerPosition = SystemAPI.GetComponent<PlayerData>(playerDataEntity).Position;

                    // ? TODO use the HashGrid to find the nearest N enemies, in neighborhood
                    var spawnJob = new SpawnProjectileJob
                    {
                        ECB = ecb,
                        Prefab = config.ProjectilePrefab,
                        Origin = playerPosition,
                        DistanceMax = config.DistanceMax
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
        public float DistanceMax;

        void Execute([ChunkIndexInQuery] int chunkIndex, in LocalTransform enemyTransform, in EnemyTag tag)
        {
            float3 delta = enemyTransform.Position - Origin;
            float distanceSq = math.lengthsq(delta);

            var distanceSqMax = DistanceMax * DistanceMax;
            if (distanceSq > distanceSqMax)
            {
                // Enemy is out of range; do not spawn projectile
                return;
            }

            Entity projectile = ECB.Instantiate(chunkIndex, Prefab);
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
        }
    }
}