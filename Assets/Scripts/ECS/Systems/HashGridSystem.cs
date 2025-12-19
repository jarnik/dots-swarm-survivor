using Swarm.ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Swarm.ECS.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct HashGridSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var query = SystemAPI.QueryBuilder().WithAll<EnemyTag, LocalTransform>().Build();

            // Use WorldUpdateAllocator so it disposes automatically at frame end
            var grid = new NativeParallelMultiHashMap<uint, Entity>(
                query.CalculateEntityCount(),
                state.WorldUpdateAllocator);

            state.Dependency = new PopulateGridJob
            {
                Grid = grid.AsParallelWriter(),
                CellSize = 2.0f
            }.ScheduleParallel(query, state.Dependency);

            if (!SystemAPI.HasSingleton<EnemyHashGrid>())
            {
                state.EntityManager.CreateEntity(typeof(EnemyHashGrid));
            }

            SystemAPI.SetSingleton(new EnemyHashGrid
            {
                Grid = grid,
                CellSize = 0.3f
            });
        }
    }
    
    [BurstCompile]
    public partial struct PopulateGridJob : IJobEntity
    {
        public NativeParallelMultiHashMap<uint, Entity>.ParallelWriter Grid;
        public float CellSize;

        void Execute(Entity entity, in LocalTransform transform, in EnemyTag tag)
        {
            int2 gridPos = (int2)math.floor(transform.Position.xz / CellSize);
            uint hash = math.hash(gridPos);
            Grid.Add(hash, entity);
        }
    }
}