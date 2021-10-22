using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    public TextMeshProUGUI firstText;
    public TextMeshProUGUI secondText;

    private int bestTimer;
    private bool started;
    private float timer;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        bestTimer = 19870;

        
    }
    private void Update()
    {
        if (started)
        {
            timer += Time.deltaTime;
            if(bestTimer != 0)
            {
                secondText.text = CalculateTime((int)timer);
            }
            else
            {
                firstText.text = CalculateTime((int)timer);
            }
        }
    }


    public void StartTimer()
    {
        started = true;

        timer = 0;
        bestTimer = PlayerPrefs.GetInt("BestTimer");
        firstText.gameObject.SetActive(true);
        if (bestTimer != 0)
        {
            firstText.text = "Best Score  ";
            firstText.text += CalculateTime(bestTimer);
            firstText.color = Color.blue;
            secondText.gameObject.SetActive(true);
        }
        else
        {
            secondText.gameObject.SetActive(false);
        }
    }
    public void EndTimer()
    {
        timer = 0;
        started = false;
        firstText.gameObject.SetActive(false);
        secondText.gameObject.SetActive(false);
        secondText.color = Color.white;
        firstText.color = Color.white;

    }
    public void Finish()
    {
        started = false;

        PlayerPrefs.SetInt("BestTimer", (int)timer);
        PlayerPrefs.Save();

        if (bestTimer > 0)
        {
            if(bestTimer < timer)
            {
                secondText.text = "New Best Score  " + secondText.text;
                secondText.color = Color.green;
            }
            else
            {
                secondText.color = Color.red;
            }
        }
        else
        {
            firstText.text = "New best Score" + CalculateTime((int)timer);
        }
        StartCoroutine(EndingGame());
    }

    IEnumerator EndingGame()
    {
        yield return new WaitForSeconds(10f);
        EndTimer();
    }

    private string CalculateTime(int timer)
    {
        string _string = string.Empty;
        int hour = timer / 3600;
        int min = (timer - (hour * 3600)) / 60;
        int sec = timer - (hour * 3600) - min * 60;
        if (min < 10)
            _string += hour + " : 0" + min + " : ";
        else
            _string += hour + " : " + min + " : ";
        if (sec < 10)
            _string += "0" + sec;
        else
            _string += sec;

      
        return _string;
    }
}
