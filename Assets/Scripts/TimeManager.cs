using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public Text text_Timer;

    float time_start;
    public float time_current; //시간 변수
    float time_Max = 1000000;
    bool isEnded;
    // Start is called before the first frame update
    void Start()
    {
        Reset_Timer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnded)
            return;
        Check_Timer();
    }

    void Check_Timer() //시간 검사
    {
        time_current = GameManagement.staticLimitTime - (Time.time - time_start);
        if (time_current > 30.0f)
        {
            text_Timer.color = Color.white;
            text_Timer.text = $"{time_current:N2}";
        }
        else if (time_current <= 30.0f && time_current > 0.0f)
        {
            text_Timer.color = Color.red;
            text_Timer.text = $"{time_current:N2}";
        }
        else if (time_current <= 0.0f)
        {
            SceneManager.LoadScene("Die");
        }
        else if (!isEnded)
        {
            End_Timer();
        }
    }

    void End_Timer() //종료시 실행
    {
        time_current = time_Max;
        text_Timer.text = $"{time_current:N2}";
        isEnded = true;
    }

    void Reset_Timer() //리셋
    {
        time_start = Time.time;
        time_current = 0;
        text_Timer.text = $"{time_current:N2}";
        isEnded = false;
    }

    public float GetCurrentTime()
    {
        return GameManagement.staticLimitTime - time_current;
    }
}
