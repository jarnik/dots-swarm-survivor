using System;
using UnityEngine;

namespace Swarm.Runtime
{
    public class ModeSwitcher : MonoBehaviour
    {
        [SerializeField] private StateContainer _stateContainer;
        [SerializeField] private GameObject[] _gameObjectModeObjects;
        [SerializeField] private GameObject[] _ecsModeObjects;

        private void Awake()
        {
            Debug.Assert(_stateContainer != null, "StateContainer is not assigned");
            SetMode(_stateContainer.data.mode);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var newMode = _stateContainer.data.mode == Mode.ECS ? Mode.GameObject : Mode.ECS;
                SetMode(newMode);
            }
        }

        private void SetMode(Mode newMode)
        {
            if (_stateContainer.data.mode == newMode)
                return;

            _stateContainer.data.mode = newMode;
            Debug.Log($"Switched to {_stateContainer.data.mode} mode.");

            for (int i = 0; i < _gameObjectModeObjects.Length; i++)
            {
                _gameObjectModeObjects[i].SetActive(newMode == Mode.GameObject);
            }

            for (int i = 0; i < _ecsModeObjects.Length; i++)
            {
                _ecsModeObjects[i].SetActive(newMode == Mode.ECS);
            }
        }
    }
}