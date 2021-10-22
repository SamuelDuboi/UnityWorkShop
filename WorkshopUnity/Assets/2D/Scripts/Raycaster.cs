using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RayDirection
{
    right,left,up,down
}
public class Raycaster : MonoBehaviour
{
    public float rayDistance = 0.1f;
    [Range(1, 10)]
    public int accuracy = 3;
    public float skinWidth = 0.02f;

    public int layerMask = ~1<<8;

    public Transform m_transform;
    public BoxCollider2D m_box;
    private Vector2 rayDirection;
   public bool ThrowRays(RayDirection direction)
    {

        List<Vector2> rays = new List<Vector2>();

        float offsetX=m_box.size.x * 0.5f * m_transform.lossyScale.x ; 
        float offsetY = m_box.size.y * 0.5f * m_transform.lossyScale.y;
        Vector2 startCorner = new Vector2(m_transform.position.x + m_box.offset.x, m_transform.position.y + m_box.offset.y );
        Vector2 endCorner = new Vector2(m_transform.position.x + m_box.offset.x, m_transform.position.y + m_box.offset.y); 


        //set direction and box corner aoccording to direction
        switch (direction)
        {
            case RayDirection.right:
                rayDirection = Vector2.right ;
                startCorner.x += offsetX;
                startCorner.y -= offsetY;
                endCorner.x += offsetX;
                endCorner.y += offsetY;

                startCorner.x -= skinWidth;
                endCorner.x -= skinWidth;
                break;
            case RayDirection.left:
                rayDirection = Vector2.right * -1;
                startCorner.x -= offsetX;
                startCorner.y -= offsetY;
                endCorner.x -= offsetX;
                endCorner.y += offsetY;
                startCorner.x -= skinWidth;
                endCorner.x -= skinWidth;
                break;
            case RayDirection.up:
                rayDirection = Vector2.up;
                startCorner.x -= offsetX;
                startCorner.y += offsetY ;
                endCorner.x += offsetX;
                endCorner.y += offsetY;
                startCorner.y += skinWidth;
                endCorner.y += skinWidth;
                break;
            case RayDirection.down:
                rayDirection = Vector2.up * -1;
                startCorner.x -= offsetX;
                startCorner.y -= offsetY;
                endCorner.x += offsetX;
                endCorner.y -= offsetY;
                startCorner.y -= skinWidth;
                endCorner.y -= skinWidth;
                break;
            default:
                break;
        }

        //dray rays
        for (int i = 0; i < accuracy; i++)
        {
            if(accuracy == 1)
            {
                rays.Add(startCorner + (endCorner - startCorner) * 0.5f);
            }
            else
            {
                rays.Add(startCorner + (endCorner - startCorner) * ((float)i / ((float)accuracy-1)));
            }
            Debug.DrawRay(rays[i], rayDirection*rayDistance, Color.cyan);
            
            if (Physics2D.Raycast(rays[i], rayDirection, rayDistance))
            {
                return true;
            }
            

        }
        return false;

    }
}
