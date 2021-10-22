using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    public Image[] Mans;
    public Color selectedManColor = Color.red;
    private Color initialColor;
    private Size currentSize = Size.medium;
    // Start is called before the first frame update
    void Start()
    {
        MyEventSystem.instance.OnResize += ChangeColor;
        initialColor = Mans[0].color;
    }

    void ChangeColor(float scaleFactor)
    {
        if(Mathf.Sign( scaleFactor)>0 && currentSize != Size.big)
        {
            currentSize++;
            for (int i = 0; i < (int)currentSize; i++)
            {
                Mans[i].color = initialColor;
            }
            Mans[(int)currentSize].color = selectedManColor;
        }
        else if (Mathf.Sign(scaleFactor) < 0 && currentSize != Size.small)
        {
            currentSize--;
            for (int i = 2; i > (int)currentSize; i--)
            {
                Mans[i].color = initialColor;
            }
            Mans[(int)currentSize].color = selectedManColor;
        }

    }
}
