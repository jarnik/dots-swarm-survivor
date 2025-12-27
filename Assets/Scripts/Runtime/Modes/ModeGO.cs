using Swarm.GO;
using UnityEngine;

namespace Swarm.Runtime
{
    public class ModeGO : ModeBase
    {
        [SerializeField] private ProjectileSpawner _projectileSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;

        public override Mode ModeType => Mode.GameObject;

        public override void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public override void Clear()
        {
            _projectileSpawner.Clear();
            _enemySpawner.Clear();
        }
    }
}
