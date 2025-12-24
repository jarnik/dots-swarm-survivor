using Swarm.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Swarm.UI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private StatsProviderBase _statsProvider;
        [SerializeField] private StateContainer _stateContainer;
        [SerializeField] private float _updateInterval = 0.5f;
        [SerializeField] private Text _titleText;
        [SerializeField] private Text _objectCountText;
        [SerializeField] private Text _fpsText;
        [SerializeField] private GameObject _activeIndicator;
        [SerializeField] private Color _colorActive = Color.green;
        [SerializeField] private Color _colorInactive = Color.red;

        private float _timeSinceLastUpdate = 0f;

        private void Start()
        {
            Debug.Assert(_stateContainer != null, "StateContainer is not assigned");
            Debug.Assert(_activeIndicator != null, "ActiveIndicator is not assigned");
        }

        private void Update()
        {
            var isActive = _statsProvider != null && _statsProvider.GetCurrentMode() == _stateContainer.data.mode;

            if (_activeIndicator.activeSelf != isActive)
            {
                _activeIndicator.SetActive(isActive);
            }

            if (_titleText != null)
            {
                _titleText.color = isActive ? _colorActive : _colorInactive;
            }

            if (isActive)
            {
                _timeSinceLastUpdate += Time.unscaledDeltaTime;
                if (_timeSinceLastUpdate > _updateInterval)
                {
                    _timeSinceLastUpdate = 0f;
                    UpdateStats();
                }
            }
        }

        private void UpdateStats()
        {
            int objectCount = _statsProvider.GetObjectCount();
            float fps = _statsProvider.GetFPS();

            if (_objectCountText != null)
            {
                _objectCountText.text = objectCount.ToString();
            }

            if (_fpsText != null)
            {
                _fpsText.text = fps.ToString("F1");
            }
        }
    }
}
