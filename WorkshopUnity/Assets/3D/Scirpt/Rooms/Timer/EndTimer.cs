using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTimer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "Player")
        {
            TimerManager.instance.Finish();
        }
    }
}
