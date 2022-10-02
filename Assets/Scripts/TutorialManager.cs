using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Text tutorial;
    public GameObject timeManager;

    // Start is called before the first frame update
    void Start()
    {
        tutorial.text = "오른쪽 컨트롤러로 방향을 정하고 \n 왼쪽 컨트롤러를 이용해 움직여 보세요 \n 쉬프트 버튼을 눌러 달릴 수도 있습니다";
    }

    // Update is called once per frame
    void Update()
    {
        if (10.0f >= timeManager.GetComponent<TimeManager>().GetCurrentTime())
        {
            tutorial.text = "라이터에 다가가 F 버튼을 누르면 획득할 수 있습니다 \n G 버튼을 눌러 라이터를 켜고 끌 수 있습니다";
        }
        else if (20.0f >= timeManager.GetComponent<TimeManager>().GetCurrentTime() && 10.0f < timeManager.GetComponent<TimeManager>().GetCurrentTime())
        {
            tutorial.text = "촛대에 다가가 E 버튼을 누르면 불을 붙일 수 있습니다. \n 몬스터를 피해 10개의 촛불을 모두 켜면 탈출구가 열립니다 행운을 빕니다";
        }
        else if (30.0f > timeManager.GetComponent<TimeManager>().GetCurrentTime() && 20.0f < timeManager.GetComponent<TimeManager>().GetCurrentTime())
        {
            tutorial.text = "";
        }
    }
}
