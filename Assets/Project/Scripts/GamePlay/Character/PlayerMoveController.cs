using Project.Scripts.GamePlay.Manager;
using UnityEngine;

namespace Project.Scripts.GamePlay.Character
{
    public class PlayerMoveController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        //private CapsuleCollider2D _col;
        private PhysicsCheck _physicsCheck;
        private Vector2 moveDirection;
        private bool isJumping;//跳跃状态
        [Header("基础参数")]
        [SerializeField] private float moveSpeed;//移动速度
        [SerializeField] private float coyoteTime = 0.3f; // 土狼跳允许的时间窗口
        [SerializeField] private float jumpForce = 10f;//跳跃力
        private float coyoteTimeCounter; // 土狼跳计时器
        private bool _shouldJump;//是否跳跃

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _physicsCheck = GetComponent<PhysicsCheck>();
            //_col = GetComponent<CapsuleCollider2D>();
        }
        
        private void Update()
        {
            if (GameInputManager.Instance.Jump)
            {
               _shouldJump = true;
            }
            moveDirection = GameInputManager.Instance.InputDirection;
            // 更新土狼跳计时器
            if (_physicsCheck.IsGround)
            {
                coyoteTimeCounter = coyoteTime; // 在地面时重置计时器
                isJumping = false;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime; // 不在地面时减少计时器
            }
        }

        private void FixedUpdate()
        {
            if (_shouldJump)
            {
                Jump();
                _shouldJump = false;
            }
            Move();
        }

        private void Move()
        {
            _rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed * Time.deltaTime, _rb.linearVelocityY);
            var faceDir = moveDirection.x switch
            {
                > 0 => 1,
                < 0 => -1,
                _ => (int)transform.localScale.x
            };
            transform.localScale = new Vector3(faceDir ,1 ,1);
        }
        
        private void Jump()
        {
            if ((_physicsCheck.IsGround || coyoteTimeCounter > 0) && !isJumping)
            {
                isJumping = true;
                coyoteTimeCounter = 0; // 跳跃后重置土狼跳计时器
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, 0); // 重置垂直速度
                _rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
