using UnityEngine;

namespace Project.Scripts.GamePlay.Character
{
    public class PhysicsCheck : MonoBehaviour
    {
        private CapsuleCollider2D coll;
        //private PlayerMoveController playerMoveController;
        private Rigidbody2D rb;
        [Header("检测参数")] public bool manual;
        [SerializeField]private bool isPlayer;//是否是玩家
        [SerializeField]private Vector2 bottomOffset;//脚底位移差值
        [SerializeField]private Vector2 leftOffset;//左方位移差值
        [SerializeField]private Vector2 rightOffset;//右方位移差值
        [SerializeField]private float checkRadius;//定义碰撞范围
        [SerializeField]private LayerMask groundLayer;//定义地面

        [Header("状态")]
        [SerializeField]private bool isGround;//判断是否站在地面
        [SerializeField]private bool touchLeftWall;
        [SerializeField]private bool touchRightWall;
        public bool IsGround => isGround;
        private void Awake()
        {
            coll = GetComponent<CapsuleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            if (!manual)
            {
                //isGround = true;
                rightOffset = new Vector2((coll.bounds.size.x / 2 + coll.offset.x),coll.offset.y );
                leftOffset = new Vector2(-(coll.bounds.size.x / 2 - coll.offset.x), rightOffset.y);
            }
            if (isPlayer)
            {
                //playerMoveController = GetComponent<PlayerMoveController>();
            }
        }

        private void Update()
        {
            Check();
        }

        /// <summary>
        /// 检测地面
        /// </summary>
        private void Check()
        {
            //检测地面
            //OverlapCircle(中心点（强制转换transform为二维变量再添加脚底位移差值）,检测范围 ,确认值)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + 
                                               new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y)
                , checkRadius, groundLayer);

            rightOffset = new Vector2((coll.bounds.size.x / 2 + 
                                       coll.offset.x * transform.localScale.x), coll.offset.y);
            leftOffset = new Vector2(-(coll.bounds.size.x / 2 - 
                                       coll.offset.x * transform.localScale.x), rightOffset.y);

            //墙体判断
            touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + 
                                                    leftOffset, checkRadius, groundLayer);
            touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + 
                                                     rightOffset, checkRadius, groundLayer);
        }

        private void OnDrawGizmosSelected()//当物体被选中时进行绘制
        {
            //虚线绘制检测范围图形DrawWireSphere（中心点，检测范围）
            Gizmos.DrawWireSphere((Vector2)transform.position +
                                  new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), 
                checkRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + 
                                  new Vector2(leftOffset.x, leftOffset.y), checkRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + 
                                  new Vector2(rightOffset.x, rightOffset.y), checkRadius);
        }
    }
}
