using System.Collections;
using UnityEngine;
using Cinemachine;
public class ShreddingMechanic : MonoBehaviour
{
    public Transform m_transform;
    public Rigidbody m_rigidbody;
    public CinemachineFreeLook cmFreeLook;
    public Animator m_animator;
    public AfterImage[] afterImages;


    [Header("ShreddValue")]
    [Tooltip("The factor that the medium scale will be divided or multiply")]
    public float sizeChangeFactor = 3;
    [Tooltip("The factor that the mass will be divided or multiply")]
    public float massChangeFactor = 2;
    [Tooltip("The factor that the animator speed will be divided or multiply")]
    public float animationSpeedFactor = 2;
    public float shreddingDuration;

    private bool isShredding;
    private Size currentSize = Size.medium;
    private float initMass;
    float initAnimatorSpeed;
    // Start is called before the first frame update
    void Start()
    {
        MyEventSystem.instance.OnResize += ChangeSize;
        initMass = m_rigidbody.mass;
        initAnimatorSpeed = m_animator.speed;
    }

    private void ChangeSize(float sizeFactor)
    {


        //shred if is not already shredding;
        if (!isShredding)
        {
            if (Mathf.Sign(sizeFactor) > 0 && (int)currentSize != 2) { currentSize++; }
            else if (Mathf.Sign(sizeFactor) < 0 && (int)currentSize != 0) { currentSize--; }
            else
                return;
            StartCoroutine(ChangingSize(sizeFactor));
        }
    }

    IEnumerator ChangingSize(float sizeFactor)
    {

        isShredding = true;
        //initial calculs to no repeat them during the loop;
        //Scale
        float globalFactor = sizeFactor / (shreddingDuration * 100);
        float _sizeFactor = 1 - (1 / sizeChangeFactor);
        //Mass
        float _massChangeFactor = 1 - (1 / massChangeFactor);
        //Anim

        float _animationSpeedFactor = 1 - (1 / animationSpeedFactor);

        //stop the character if he jumps
        Vector3 _velocity = m_rigidbody.velocity;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.useGravity = false;

        //change mass,anim and scale
        for (int i = 0; i < shreddingDuration * 100; i++)
        {

            m_transform.localScale += Vector3.one * _sizeFactor * globalFactor;
            m_rigidbody.mass += initMass * globalFactor * _massChangeFactor;
            m_animator.speed -= initAnimatorSpeed * globalFactor * _animationSpeedFactor;

            //create an after image every 0.1*shreddingDuration sec
            if (i % 10 == 0)
            {
                afterImages[i / 10].gameObject.SetActive(true);
                afterImages[i / 10].DoAfterImage();
                if (Mathf.Abs(sizeFactor) < 0)
                {
                    afterImages[i / 10].transform.localScale = m_transform.localScale;
                }
                else
                {
                    afterImages[i / 10].transform.localScale = m_transform.localScale += Vector3.one * _sizeFactor * globalFactor * 3;
                }
                afterImages[i / 10].transform.localPosition = m_transform.localPosition;
                afterImages[i / 10].transform.rotation = m_transform.rotation;

            }
            //change the orbits of the free cam so the cam angle stay the same
            for (int x = 0; x < cmFreeLook.m_Orbits.Length; x++)
            {
                cmFreeLook.m_Orbits[x].m_Height += _sizeFactor * globalFactor;
                cmFreeLook.m_Orbits[x].m_Radius += _sizeFactor * globalFactor*4;

            }
            yield return new WaitForSeconds(0.01f);
        }
        //restore velocity
        m_rigidbody.useGravity = true;
        m_rigidbody.velocity = new Vector3(_velocity.x, 0, _velocity.z);
        isShredding = false;
    }
}
public enum Size { small, medium, big }