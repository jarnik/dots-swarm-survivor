using UnityEngine;

namespace Swarm.Runtime
{
    [CreateAssetMenu(fileName = "StateContainer", menuName = "Swarm/StateContainer")]
    public class StateContainer : ScriptableObject
    {
        public StateData data { get; private set; } = new StateData();

        public void Initialize()
        {
            data = new StateData();
        }
    }
}