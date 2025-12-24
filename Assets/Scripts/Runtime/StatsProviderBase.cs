using UnityEngine;

namespace Swarm.Runtime
{
    public abstract class StatsProviderBase : MonoBehaviour
    {
        public abstract int GetObjectCount();
        public abstract float GetFPS();
    }
}