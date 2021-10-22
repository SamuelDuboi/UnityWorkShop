using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
public class CharacterController3D : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode jumpKey;
    public float minCamTreshold = 1;

    [Header("Physic variable")]
    public CharacterStats characterStats;
    private int remainingJump;
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
    private bool isJumping;

    //references
    public Raycaster m_raycsater;
    public Transform vcam;
    public Transform mainCam;
    public Transform centerTransform;
    public Animator m_animator;
    public Rigidbody m_rigidbody;


    private Vector3 currentMovement;
    private Vector3 currentCamMouvement;
    private float currentRotation;
    private Transform m_transform;
    private Vector3 lastDirection;
    private float horizontalTimeStamp;
    private float camMaxDistance;
    private float groundPosY;
    private float currentPosY;
    private float m_JumpForce;


    Vector3 m_forward;
    Vector3 m_right;
    Vector3 dir;
    void Start()
    {
        m_transform = GetComponent<Transform>();
        remainingJump = characterStats.maxJumpNumber;
        camMaxDistance = (vcam.position - m_transform.position).magnitude;
        groundPosY =m_transform.position.y;

        m_JumpForce = characterStats.jumpForce * 100;

        MyEventSystem.instance.OnJump += Jump;
    }

    // Update is called once per frame
    void Update()
    {
       
      
        HorizontalUpdate();
        IsFalling();
       // CameraController();

    }
    private void HorizontalUpdate()
    {

        currentMovement = Vector3.zero;
        currentRotation = 0;
        m_forward = mainCam.forward;

        m_forward.y = 0;
        m_forward.Normalize();


        m_right = new Vector3(m_forward.z, 0, -m_forward.x);

        currentMovement = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        dir = m_forward * currentMovement.x + m_right * currentMovement.z;

        if (Vector3.Dot(lastDirection, currentMovement)> 0)
        {
            if (currentMovement != lastDirection)
            {
                horizontalTimeStamp = 0;
            }
            else
            {
                horizontalTimeStamp += Time.deltaTime;
            }

            m_transform.Translate(dir * Time.deltaTime * characterStats.speed * characterStats.horizontalSpeed.Evaluate(horizontalTimeStamp)/m_rigidbody.mass, Space.World);
           
        }
        else
        {
            horizontalTimeStamp = 0;
            m_animator.SetFloat("Speed", currentMovement.magnitude);
        }

       
        //rotate player to the input direction on camera's plan
        if(currentMovement.magnitude>0.1)
        m_transform.forward = Vector3.Lerp(m_transform.forward, dir, Time.deltaTime * characterStats.angularSpeed);
      

        //update animation
        m_animator.SetFloat("Speed",currentMovement.magnitude);
        if(currentMovement.magnitude<0.9)
            m_animator.SetFloat("Turn", currentMovement.magnitude);
        else
        {
            m_animator.SetFloat("Turn", 0);

        }


        //update last frame
        lastDirection = currentMovement;
    }
    bool doOnce;
    private void IsFalling()
    {

        if (!isGrounded)
        {
            if (m_rigidbody.velocity.y < 0 && !doOnce)
            {
                doOnce = true;
                m_animator.SetTrigger("Fall");
            }
        }  
           
    }

    void Jump()
    {
        if (remainingJump > 0)
        {
            doOnce = false;
            m_animator.SetInteger("JumpNumber", characterStats.maxJumpNumber - remainingJump);
            remainingJump--;
            m_rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
            isGrounded = false;
            m_animator.SetTrigger("Jump");
            m_animator.SetBool("IsGrounded", false);
            isJumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.position.y < centerTransform.position.y) 
            BecomeGrounded();
    }
    public void BecomeGrounded()
    {
            doOnce = false;
            isGrounded = true;
            m_animator.SetBool("IsGrounded", true);
            remainingJump = characterStats.maxJumpNumber;
        
    }

    private float camResetTimer;

    //used to be great and then I discovered Cinemachine FreeLook
/*    public void CameraController()
    {

        float distanceCamPlayer = (vcam.position - m_transform.position).magnitude;

        if (camMaxDistance + 1 > distanceCamPlayer && (mainCam.position - m_transform.position).magnitude > minCamTreshold)
            vcam.Translate(dir * Time.deltaTime * characterStats.speed * characterStats.horizontalSpeed.Evaluate(horizontalTimeStamp), Space.World);
        else if (camMaxDistance + 1 < distanceCamPlayer)
            vcam.position = Vector3.MoveTowards(vcam.position, m_transform.position, Time.deltaTime * 20);



        // to adapt the cam pos in y but with a small delay to have the feeling of getting higher
        vcam.position += new Vector3(0,( m_transform.position.y - currentPosY)*0.5f, 0);
        currentPosY = m_transform.position.y;

        //get camm input (twin stick)
        currentCamMouvement =  new Vector3(0, Input.GetAxis("CamHorizontal"),0);

        //move the came
        if(currentCamMouvement.magnitude > 0.2 )
        {
               Debug.Log(currentCamMouvement.magnitude);
                vcam.transform.RotateAround(m_transform.position,  currentCamMouvement, Time.deltaTime * 90);
                camResetTimer = 0;
           
        }
    }*/
    
  
}
