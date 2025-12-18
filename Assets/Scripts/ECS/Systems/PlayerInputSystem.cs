using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Swarm.ECS.Components;

namespace Swarm.ECS.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class PlayerInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            float2 moveInput = new float2(moveX, moveY);

            if (math.lengthsq(moveInput) > 1f)
            {
                moveInput = math.normalize(moveInput);
            }

            if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
            {
                return;
            }
            var playerInput = SystemAPI.GetComponent<PlayerInput>(playerEntity);
            playerInput.Movement = moveInput;

            SystemAPI.SetComponent(playerEntity, playerInput);
        }
    }
}