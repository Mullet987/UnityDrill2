using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager 
{
    //public TextMeshProUGUI StatusNotice;
    //private bool isButtonPressed = false;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space)) // Space 키로 예시
    //    {
    //        isButtonPressed = true;
    //    }
    //}

    //public void PrepareMiniGame(string eventCode) 
    //{
    //    if (eventCode == "MINIGAME1") {
    //        SetStatusCondition(5);
    //    }
    //}

    //public void SetStatusCondition(int num)
    //{
    //    switch (num)
    //    {
    //        case 1:
    //            StatusNotice.text = ">= 1";
    //            break;
    //        case 2:
    //            StatusNotice.text = ">= 2";
    //            break;
    //        case 3:
    //            StatusNotice.text = ">= 3";
    //            break;
    //        case 4:
    //            StatusNotice.text = ">= 4";
    //            break;
    //        case 5:
    //            StatusNotice.text = ">= 5";
    //            break;
    //        case 6:
    //            StatusNotice.text = ">= 6";
    //            break;
    //        case 7:
    //            StatusNotice.text = ">= 7";
    //            break;
    //        case 8:
    //            StatusNotice.text = ">= 8";
    //            break;
    //        case 9:
    //            StatusNotice.text = ">= 9";
    //            break;
    //        case 10:
    //            StatusNotice.text = ">= 10";
    //            break;
    //        default:
    //            // 위에 없는 모든 값에 대한 실행 코드
    //            Debug.Log("numnum is something else");
    //            break;
    //    }
    //}

    //IEnumerator PrintNumbers()
    //{
    //    branchLayoutPanelObj.SetActive(false); //선택지 패널 비활성화

    //    // 특정 버튼이 눌릴 때까지 대기
    //    while (!isButtonPressed)
    //    {
    //        yield return null; // 매 프레임 대기
    //    }
    //    Debug.Log("버튼이 눌렸습니다! 코루틴 종료.");
    //    isButtonPressed = false; // 상태 초기화

    //    branchLayoutPanelObj.SetActive(true); //선택지 패널 활성화
    //    flowPlayer.Play(0);
    //}
}