using UnityEngine;

namespace Swarm.GO
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private Vector3 _direction = Vector3.forward;
        [SerializeField] private bool _limitToScreen = false;
        [SerializeField] private float _screenWidth = 0f;
        [SerializeField] private float _screenHeight = 0f;

        private void Update()
        {
            var newPosition = transform.position + _direction * _speed * Time.deltaTime;

            if (_limitToScreen)
            {
                newPosition.x = Mathf.Clamp(newPosition.x, -_screenWidth / 2, _screenWidth / 2);
                newPosition.y = Mathf.Clamp(newPosition.y, -_screenHeight / 2, _screenHeight / 2);
            }

            transform.position = newPosition;
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction.normalized;
        }
    }
}
