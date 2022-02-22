using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;

    public float speed, jumpForce;
    private float horizontalMove;
    public Transform groundCheck;
    public LayerMask ground;
    [Header("CD的UI组件")]
    public Image cdImage;
    [Header("Dash参数")]
    public float dashTime;//dash时长
    private float dashTimeLeft;//冲锋剩余时间
    private float lastDash=-10f;//上一次dash时间点
    public float dashCD;
    public float dashSpeed;

    public bool isGround, isJump, isDashing;

    bool jumpPressed;
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time >= (lastDash + dashCD))
            {
                ReadyToDash();
            }
        }
        cdImage.fillAmount -= 1.0f / dashCD * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        Dash();
        if (isDashing)
        {
            return;
        }
        GroundMovement();

        Jump();


        SwitchAnim();
    }

    void GroundMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");//只返回-1，0，1
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }

    }

    void Jump()//跳跃
    {
        if (isGround)
        {
            jumpCount = 2;//可跳跃数量
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    void SwitchAnim()//动画切换
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
    }

    void ReadyToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        cdImage.fillAmount = 1;
    }

    void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                if (rb.velocity.y >0&& !isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * gameObject.transform.localScale.x, jumpForce);
                }
                rb.velocity = new Vector2(dashSpeed * transform.localScale.x, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;
                ShadowPool.instance.GetFormPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                if (!isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * transform.localScale.x, jumpForce);
                }
            }
        }
    }
}
