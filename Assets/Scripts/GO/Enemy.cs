using UnityEngine;

namespace Swarm.GO
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Data.EnemyConfig _config;

        private Transform _playerTransform;

        public void Initialize(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        void Update()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            var playerDirection = (_playerTransform.position - transform.position).normalized;
            transform.position += playerDirection * _config.Speed * Time.deltaTime;

            var rotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = rotation;
        }
    }
}