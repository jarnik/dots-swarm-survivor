using UnityEngine;

namespace Swarm.Data
{
    [CreateAssetMenu(fileName = "Config", menuName = "Swarm/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _screenWidth;
        [SerializeField] private float _screenHeight;
        [SerializeField] private float _projectileCooldown;
        [SerializeField] private float _projectileMaxDistance;

        public float Speed => _speed;
        public float ScreenWidth => _screenWidth;
        public float ScreenHeight => _screenHeight;
        public float ProjectileCooldown => _projectileCooldown;
        public float ProjectileMaxDistance => _projectileMaxDistance;
    }
}