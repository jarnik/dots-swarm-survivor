using Swarm.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Swarm.ECS
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTag>(entity);
                AddComponent<PlayerInput>(entity);
                AddComponent<Direction>(entity);
                AddComponent<MovementSpeed>(entity);

                SetComponent(entity, new MovementSpeed { Value = 5f });
            }
        }
    }
}