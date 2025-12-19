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
        public const float CellSize = 0.3f;

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
                CellSize = CellSize
            });
        }

        public static int2 GetGridPosition(float3 position)
        {
            return (int2)math.floor(position.xy / CellSize);
        }

        public static uint GetGridHash(float3 position)
        {
            return math.hash(GetGridPosition(position));
        }

        public static uint GetGridHash(int2 gridPosition)
        {
            return math.hash(gridPosition);
        }

        public interface ICollisionProcessor
        {
            void OnHit(Entity entity);
        }

        [BurstCompile]
        public static void SearchGrid<TProcessor, TTag>(
            float3 position, 
            float radius, 
            [ReadOnly] NativeParallelMultiHashMap<uint, Entity> grid,
            ComponentLookup<LocalTransform> entityTransforms,
            [ReadOnly] ComponentLookup<TTag> tags,
            ref TProcessor processor
        )
            where TProcessor : struct, ICollisionProcessor
            where TTag : unmanaged, IComponentData
        {
            int2 center = GetGridPosition(position);
            float radiusSq = radius * radius;

            // search neighboring cells
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    uint hash = GetGridHash(center + new int2(x, y));
                    if (grid.TryGetFirstValue(hash, out Entity other, out var it))
                    {
                        do
                        {
                            // Filter by tag first to avoid unnecessary lookups
                            if (!tags.HasComponent(other))
                                continue;
                                
                            var otherEntityTransform = entityTransforms[other];
                            float distSq = math.lengthsq(otherEntityTransform.Position - position);

                            if (distSq > radiusSq)
                                continue;

                            processor.OnHit(other);
                        } while (grid.TryGetNextValue(out other, ref it));
                    }
                }
            }
        }
    }

    [BurstCompile]
    public partial struct PopulateGridJob : IJobEntity
    {
        public NativeParallelMultiHashMap<uint, Entity>.ParallelWriter Grid;
        public float CellSize;

        void Execute(Entity entity, in LocalTransform transform, in EnemyTag tag)
        {
            int2 gridPos = HashGridSystem.GetGridPosition(transform.Position);
            uint hash = math.hash(gridPos);
            Grid.Add(hash, entity);
        }
    }
}