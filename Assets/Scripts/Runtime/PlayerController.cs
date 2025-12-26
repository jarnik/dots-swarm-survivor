using UnityEngine;

namespace Swarm.Runtime
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _playerUnit;
        [SerializeField] private float _playerSpeed = 5f;
        [SerializeField] private float _screenWidth = 10f;
        [SerializeField] private float _screenHeight = 5f;

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 inputDirection = new Vector3(horizontal, 0, vertical);

            var position = _playerUnit.position + inputDirection.normalized * _playerSpeed * Time.deltaTime;

            var screenOrigin = Vector3.zero;
            position = screenOrigin + new Vector3(
                Mathf.Clamp(position.x, -_screenWidth / 2f, _screenWidth / 2f),
                0,
                Mathf.Clamp(position.z, -_screenHeight / 2f, _screenHeight / 2f)
            );
            _playerUnit.position = position;

            var lookDirection = new Vector3(inputDirection.x, 0, inputDirection.z);
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                _playerUnit.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }
}
