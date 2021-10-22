using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
     enum gravityDirection { down=0, left=1, up=2, right=3 }
    private gravityDirection currentDirection;
    public Transform vcam;
    public Transform m_Transform;
    public bool isTurning;
    public Rigidbody rgb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(Turning(1));

        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Turning(-1));

        }
    }


    
   private IEnumerator Turning(float direction)
    {
        if (!isTurning)
        {
            isTurning = true;
            for (int i = 0; i < 100; i++)
            {
                m_Transform.Rotate(Vector3.forward * direction * 90 / 100);
                yield return new WaitForSeconds(0.01f);

            }
            isTurning = false;
        }
       
    }
}
