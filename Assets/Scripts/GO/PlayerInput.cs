using UnityEngine;

namespace Swarm.GO
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private UnitMovement _playerUnit;

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 inputDirection = new Vector3(horizontal, vertical, 0);

            _playerUnit.SetDirection(inputDirection);
        }
    }
}
