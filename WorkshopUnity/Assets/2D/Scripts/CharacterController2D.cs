using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode rightKey;

    [Header("Physic variable")]
    public float speed, gravity = 9.8f;
    [SerializeField] private AnimationCurve horizontalSpeed, jumpForce;
    //jump
    public int maxJumpNumber;
    private int remainingJump;
    public float jumpCoolDown;
    [Range(1, 10)] public float jumpFactorMultiplicator;
    private float verticalTimeStamp;
    private bool _isGrounded;
    private bool isGrounded
    {
        get => _isGrounded;
        set
        {
            _isGrounded = value;
            m_animator?.SetBool("IsGrounded", value);
        }
    }
    private float fallFactor = 1;
    private bool isJumping;


    public Raycaster m_raycsater;
    public Transform graphics;
    public Animator m_animator;
    //jump


    private Vector2 currentMovement;
    private Transform m_transform;

    private float lastDirection;
    private float horizontalTimeStamp;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        remainingJump = maxJumpNumber;
    }

    // Update is called once per frame
    void Update()
    {
        currentMovement = Vector2.zero;
        HorizontalUpdate();
        VerticalUpdate();

    }
    private void HorizontalUpdate()
    {
        if (Input.GetKey(leftKey))
        {
            currentMovement.x--;
            graphics.localScale = new Vector3(-Mathf.Abs(currentMovement.x), 1, 1);

        }
        if (Input.GetKey(rightKey))
        {
            currentMovement.x++;
            graphics.localScale = new Vector3(Mathf.Abs(currentMovement.x), 1, 1);
        }


        currentMovement.Normalize();

        if (lastDirection == currentMovement.x)
        {
            horizontalTimeStamp += Time.deltaTime;
        }
        else
        {
            horizontalTimeStamp = 0;
        }

        m_transform.Translate(currentMovement * Time.deltaTime * speed * horizontalSpeed.Evaluate(horizontalTimeStamp));
        m_animator.SetFloat("Speed",Mathf.Abs( currentMovement.x*horizontalSpeed.Evaluate(horizontalTimeStamp)));
        //update last frame
        lastDirection = currentMovement.x;
    }
    bool doOnce;
    private void VerticalUpdate()
    {

        if (Input.GetKeyDown(jumpKey))
        {
             if(remainingJump>0)
            {
                doOnce = false;
                m_animator.SetInteger("JumpNumber", maxJumpNumber - remainingJump);
                remainingJump--;
                verticalTimeStamp =0;
                isGrounded = false;
                m_animator.SetTrigger("Jump");
                isJumping = true;
            }
        }

        bool moveUp = false;
        if (isJumping)
        {
            verticalTimeStamp += Time.deltaTime *(Input.GetKey(jumpKey)? 1: jumpFactorMultiplicator);
            currentMovement.y = gravity * jumpForce.Evaluate(verticalTimeStamp);
            if (currentMovement.y >= 0) moveUp = true;

            if (jumpForce.keys[jumpForce.length-1].time < verticalTimeStamp)
            {
               isJumping = false;
            }
        }
        else
        {
            fallFactor += Time.deltaTime * 2;
            currentMovement.y = gravity * -1*fallFactor;
        }


        if (m_raycsater.ThrowRays(moveUp? RayDirection.up: RayDirection.down))
        {
            if (!moveUp)
            {
                isGrounded = true;
                isJumping = false;
                remainingJump = maxJumpNumber;
            }
            fallFactor = 1;
            return;
        }
        else
        {
            if (!moveUp)
            {
                isGrounded = false;
                if (!doOnce)
                {
                    doOnce = true;
                    m_animator.SetTrigger("Falling");
                }
            }
            m_transform.Translate(currentMovement * Time.deltaTime*gravity);

        }
        
    }

}
