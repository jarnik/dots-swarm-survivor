using UnityEngine;

namespace Swarm.Data
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Swarm/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private float _speed;

        public float Speed => _speed;
    }
}