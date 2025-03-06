using Project.Scripts.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.GamePlay.Manager
{
    public class GameInputManager : Singleton<GameInputManager>
    {
        private PlayerInputController _playerInputController;
        public Vector2 InputDirection => _playerInputController.Player.Move.ReadValue<Vector2>();
        
        public bool Jump => _playerInputController.Player.Jump.ReadValue<float>() > 0;
        
        protected override void Awake()
        {
            base.Awake();
            _playerInputController ??= new PlayerInputController();
        }

        private void OnEnable()
        {
            _playerInputController.Enable();
        }

        private void OnDisable()
        {
            _playerInputController.Disable();
        }
    }
}
