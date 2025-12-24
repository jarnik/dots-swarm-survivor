using UnityEngine;

namespace Swarm.Runtime
{
    public abstract class StatsProviderBase : MonoBehaviour
    {
        public abstract Mode GetCurrentMode();
        public abstract int GetObjectCount();
        public abstract float GetFPS();        
    }
}