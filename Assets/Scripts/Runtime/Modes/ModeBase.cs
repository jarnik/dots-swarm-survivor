using UnityEngine;

namespace Swarm.Runtime
{
    public abstract class ModeBase : MonoBehaviour
    {
        public abstract Mode ModeType { get; }
        public abstract void SetActive(bool isActive);
        public abstract void Clear();
    }
}
