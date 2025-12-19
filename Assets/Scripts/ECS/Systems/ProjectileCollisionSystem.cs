using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Swarm.ECS.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(HashGridSystem))]
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
                damage = 40f,
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
        [ReadOnly] public float damage;
        public EntityCommandBuffer.ParallelWriter ECB;

        public struct EnemyHitProcessor : HashGridSystem.ICollisionProcessor
        {
            public EntityCommandBuffer.ParallelWriter ECB;
            public int SortKey;
            public float Damage;

            public void OnHit(Entity enemy)
            {
                ECB.AddBuffer<DamageEvent>(SortKey, enemy);
                ECB.AppendToBuffer(SortKey, enemy, new DamageEvent { Value = Damage });
            }
        }

        void Execute([ChunkIndexInQuery] int chunkIndex, in LocalTransform projectileTransform, in ProjectileTag tag)
        {
            var hitProcessor = new EnemyHitProcessor
            {
                ECB = ECB,
                SortKey = chunkIndex,
                Damage = damage
            };

            HashGridSystem.SearchGrid(
                projectileTransform.Position,
                hitRadius,
                grid,
                localTransformLookup,
                enemyTagLookup,
                ref hitProcessor
            );
        }
    }
}