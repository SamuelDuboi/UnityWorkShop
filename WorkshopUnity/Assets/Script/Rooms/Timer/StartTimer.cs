using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTimer : MonoBehaviour
{

    bool isWorking;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isWorking)
            {
                TimerManager.instance.EndTimer();
            }
            else
            {
                TimerManager.instance.StartTimer();
            }
            isWorking = !isWorking;

        }
    }
}
