using UnityEngine;
using Unity.Entities;

namespace Swarm.ECS
{
    [DisallowMultipleComponent]
    public class PlayerConfigAuthoring : MonoBehaviour
    {
        public float ScreenWidth;
        public float ScreenHeight;

        public class Baker : Baker<PlayerConfigAuthoring>
        {
            public override void Bake(PlayerConfigAuthoring authoring)
            {
                var singletonEntity = GetEntity(TransformUsageFlags.None);
                AddComponent(singletonEntity, new Components.PlayerConfig
                {
                    ScreenWidth = authoring.ScreenWidth,
                    ScreenHeight = authoring.ScreenHeight
                });
            }
        }
    }
}