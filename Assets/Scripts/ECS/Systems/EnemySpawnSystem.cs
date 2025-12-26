using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Jobs;

namespace Swarm.ECS
{
    public partial struct EnemySpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemySpawner>();
        }

        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (spawner, transform, config, entity) in
                    SystemAPI.Query<RefRW<EnemySpawner>, RefRO<LocalTransform>, RefRO<EnemySpawnerConfig>>()
                    .WithEntityAccess())
            {
                spawner.ValueRW.Timer -= deltaTime;

                if (spawner.ValueRW.Timer <= 0)
                {
                    spawner.ValueRW.Timer = config.ValueRO.Cooldown;

                    var count = config.ValueRO.Count;
                    var frameSeed = (uint)(SystemAPI.Time.ElapsedTime * 1000) + (uint)entity.Index;

                    var spawnJob = new SpawnEnemyJob
                    {
                        ECB = ecb.AsParallelWriter(),
                        Prefab = config.ValueRO.EnemyPrefab,
                        Origin = transform.ValueRO.Position,
                        DistanceMin = config.ValueRO.DistanceMin,
                        DistanceMax = config.ValueRO.DistanceMax,
                        Seed = frameSeed
                    }.Schedule(count, 64, state.Dependency);

                    state.Dependency = spawnJob;
                }
            }
        }
    }

    [BurstCompile]
    public struct SpawnEnemyJob : IJobParallelFor
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public Entity Prefab;

        public float3 Origin;
        public float DistanceMin;
        public float DistanceMax;

        public uint Seed;

        public void Execute(int index)
        {
            var seed = Seed + (uint)index * 9973;
            var rng = new Random(seed);

            float radius = rng.NextFloat(DistanceMin, DistanceMax);
            float randomAngle = rng.NextFloat(0f, math.PI * 2f);
            float2 dir2D = new float2(math.cos(randomAngle), math.sin(randomAngle));
            float2 targetPos = dir2D * radius;

            Entity e = ECB.Instantiate(index, Prefab);

            ECB.SetComponent(index, e, LocalTransform.FromPosition(new float3(targetPos.x, 0f, targetPos.y)));
        }
    }
}