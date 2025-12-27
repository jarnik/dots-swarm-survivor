using UnityEngine;

namespace Swarm.Runtime
{
    public class ModeSwitcher : MonoBehaviour
    {
        [SerializeField] private StateContainer _stateContainer;
        [SerializeField] private ModeBase[] _modes;

        private void Awake()
        {
            Debug.Assert(_stateContainer != null, "StateContainer is not assigned");
            SetMode(_stateContainer.data.mode, force: true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var newMode = _stateContainer.data.mode == Mode.ECS ? Mode.GameObject : Mode.ECS;
                SetMode(newMode);
            }
        }

        private void SetMode(Mode newMode, bool force = false)
        {
            if (_stateContainer.data.mode == newMode && !force)
                return;

            _stateContainer.data.mode = newMode;
            Debug.Log($"Switched to {_stateContainer.data.mode} mode.");

            for (int i = 0; i < _modes.Length; i++)
            {
                var mode = _modes[i];
                mode.SetActive(mode.ModeType == newMode);

                if (mode.ModeType != newMode)
                {
                    mode.Clear();
                }
            }
        }
    }
}