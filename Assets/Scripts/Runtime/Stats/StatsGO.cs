using UnityEngine;

namespace Swarm.Runtime
{
    public class StatsGO : StatsProviderBase
    {
        private float _fps;

        private void Awake()
        {
        }

        public override int GetObjectCount() => 666;

        private void Update()
        {
            _fps = 1.0f / Time.unscaledDeltaTime;
        }

        public override float GetFPS() => _fps;

        public override Mode GetCurrentMode() => Mode.GameObject;
    }
}
