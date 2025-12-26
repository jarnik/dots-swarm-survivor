using Swarm.ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace Swarm.Runtime
{
    public class PlayerBridge : MonoBehaviour
    {
        [SerializeField] private Transform _playerUnit;

        private Entity _playerEntity;
        private EntityManager _entityManager;

        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _playerEntity = _entityManager.CreateEntity(typeof(PlayerData));
        }

        private void Update()
        {
            _entityManager.SetComponentData(_playerEntity, new PlayerData
            {
                Position = _playerUnit.position
            });
        }
    }
}