using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    [UpdateBefore(typeof(DespawnSystem))]
    public partial struct ProjectileCollisionSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            var gridData = SystemAPI.GetSingleton<EnemyHashGrid>();

            var collisionJob = new ProjectileCollisionJob
            {
                grid = gridData.Grid,
                localTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
                enemyTagLookup = SystemAPI.GetComponentLookup<EnemyTag>(true),
                hitRadius = 0.2f,
            ECB = ecb
            }.ScheduleParallel(state.Dependency);

            state.Dependency = collisionJob;
        }
    }

    [BurstCompile]
    public partial struct ProjectileCollisionJob : IJobEntity
    {
        [ReadOnly] public NativeParallelMultiHashMap<uint, Entity> grid;
        [ReadOnly] public ComponentLookup<LocalTransform> localTransformLookup;
        [ReadOnly] public ComponentLookup<EnemyTag> enemyTagLookup;
        [ReadOnly] public float hitRadius;
        public EntityCommandBuffer.ParallelWriter ECB;

        public struct EnemyHitProcessor : HashGridSystem.ICollisionProcessor
        {
            public EntityCommandBuffer.ParallelWriter ECB;
            public int SortKey;

            public void OnHit(Entity enemy)
            {
                ECB.AddBuffer<DamageEvent>(SortKey, enemy);
                ECB.AppendToBuffer(SortKey, enemy, new DamageEvent {});
            }
        }

        void Execute([ChunkIndexInQuery] int chunkIndex, ref Lifetime lifetime, in LocalTransform projectileTransform, in ProjectileTag projectile)
        {
            var hitProcessor = new EnemyHitProcessor
            {
                ECB = ECB,
                SortKey = chunkIndex
            };

            uint collisionCount = 0;

            HashGridSystem.SearchGrid(
                projectileTransform.Position,
                hitRadius,
                grid,
                localTransformLookup,
                enemyTagLookup,
                ref hitProcessor,
                ref collisionCount
            );

            if (collisionCount > 0)
            {
                lifetime.Life = 0f;
            }
        }
    }
}