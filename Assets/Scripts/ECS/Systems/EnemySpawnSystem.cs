using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace Swarm.ECS
{
    public partial struct EnemySpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnerConfig>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            var config = SystemAPI.GetSingleton<SpawnerConfig>();

            var rng = Random.CreateFromIndex(0xCAFEBABEu);

            for (int i = 0; i < config.Count; i++)
            {
                Entity e = ecb.Instantiate(config.EnemyPrefab);

                var arenaWidth = 3f;
                var arenaHeight = 2f;
                var xy = rng.NextFloat2(
                    new float2(-arenaWidth, -arenaHeight),
                    new float2(arenaWidth, arenaHeight));
                ecb.SetComponent(e, LocalTransform.FromPosition(new float3(xy, 0f)));
            }

            // Disable system so it only runs once
            state.Enabled = false;
        }
    }
}