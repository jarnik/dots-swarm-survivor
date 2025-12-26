using Swarm.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Swarm.ECS
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public GameObject ProjectilePrefab;

        public class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<ProjectileSpawner>(entity);
            }
        }
    }
}