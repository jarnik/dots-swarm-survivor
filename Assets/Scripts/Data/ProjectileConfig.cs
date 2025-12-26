using UnityEngine;

namespace Swarm.Data
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Swarm/ProjectileConfig")]
    public class ProjectileConfig : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _life;

        public float Speed => _speed;
        public float Lifetime => _life;
    }
}