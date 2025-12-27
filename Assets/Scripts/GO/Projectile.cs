using System;
using UnityEngine;

namespace Swarm.GO
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Data.ProjectileConfig _config;
        [SerializeField] private string _enemyTag;
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _enemyLayer;

        public Action<Projectile> onDestroyed;
        public Action<Enemy> onHitEnemy;

        private static Collider[] _hitResults = new Collider[5];

        private float _life;

        private void Start()
        {
            Debug.Assert(_config != null, "Projectile config is not assigned.");

            Reset();
        }

        public void Reset()
        {
            _life = _config.Lifetime;
        }

        private void Update()
        {
            UpdateLife();
            if (_life > 0f)
            {
                UpdateMovement();
                UpdateHits();
            }
        }

        private void UpdateLife()
        {
            _life -= Time.deltaTime;
            if (_life <= 0f)
            {
                onDestroyed?.Invoke(this);
            }
        }

        private void UpdateMovement()
        {
            transform.position += transform.forward * _config.Speed * Time.deltaTime;
        }

        private void UpdateHits()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position, _radius, _hitResults, _enemyLayer,
                QueryTriggerInteraction.Collide
            );

            for (int i = 0; i < hitCount; i++)
            {
                var enemy = _hitResults[i].GetComponent<Enemy>();
                if (enemy != null)
                {
                    onHitEnemy?.Invoke(enemy);
                    onDestroyed?.Invoke(this);
                    break;
                }
            }
        }
    }
}
