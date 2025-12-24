using Swarm.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Swarm.UI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private Text _objectCountText;
        [SerializeField] private Text _fpsText;
        [SerializeField] private StatsProviderBase _statsProvider;
        [SerializeField] private float _updateInterval = 0.5f;

        private float _timeSinceLastUpdate = 0f;

        private void Update()
        {
            _timeSinceLastUpdate += Time.unscaledDeltaTime;
            if (_timeSinceLastUpdate > _updateInterval)
            {
                _timeSinceLastUpdate = 0f;
                if (_statsProvider != null)
                {
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
