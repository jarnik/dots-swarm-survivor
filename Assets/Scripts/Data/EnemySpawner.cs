using UnityEngine;

namespace Swarm.Data
{
    [CreateAssetMenu(fileName = "EnemySpawnerConfig", menuName = "Swarm/EnemySpawnerConfig")]
    public class EnemySpawnerConfig : ScriptableObject
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private int _countECS;
        [SerializeField] private int _countGO;
        [SerializeField] private float _distanceMin;
        [SerializeField] private float _distanceMax;
        [SerializeField] private GameObject _enemyPrefab;

        public float Cooldown => _cooldown;
        public int CountECS => _countECS;
        public int CountGO => _countGO;
        public float DistanceMin => _distanceMin;
        public float DistanceMax => _distanceMax;
        public GameObject EnemyPrefab => _enemyPrefab;
    }
}