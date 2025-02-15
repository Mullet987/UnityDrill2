using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Articy.Articyproject_unitydrill2;
using Articy.Unity;
using Articy.Unity.Interfaces;
using TMPro;
using System.Collections;
using Articy.Articyproject_unitydrill2.GlobalVariables;
using System;

public class DialogueManager : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    #region 변수 모음
    [SerializeField]
    GameObject dialogueWidget;
    [SerializeField]
    Image previewImagePanel;
    [SerializeField]
    ScrollRect scrollRect;
    [SerializeField]
    GameObject closeButton;
    [SerializeField]
    GameObject branchButton;
    [SerializeField]
    TextMeshProUGUI textLabel;
    [SerializeField]
    GameObject branchLayoutPanelObj;
    [SerializeField]
    GameObject ButtonUninteractable_Panel;
    [SerializeField]
    TextMeshProUGUI StatusNotice;
    [SerializeField]
    GameObject MinigamePanel;
    [SerializeField]
    GameObject CookStatusPannel;
    [SerializeField]
    GameObject MechanicStatusPannel;
    [SerializeField]
    SignalScript SignalScript;
    [SerializeField]
    AudioClip DialogueManagerAudioClip;

    private ItemAddRemove itemAddRemove;
    private MinigameManager MinigameManager;
    private RectTransform branchLayoutPanel;
    private ArticyFlowPlayer flowPlayer;
    private ArticyObject articyObject;
    private AudioSource DialogueManagerAudioSource;
    public bool DialogueActive { get; set; }
    #endregion

    void Start()
    {
        branchLayoutPanel = branchLayoutPanelObj.GetComponent<RectTransform>();
        itemAddRemove = GetComponent<ItemAddRemove>();
        flowPlayer = GetComponent<ArticyFlowPlayer>();
        MinigameManager = GetComponent<MinigameManager>();
        DialogueManagerAudioSource = GetComponent<AudioSource>();
    }

    #region Articy 동작 구문
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        // Branch 초기화
        ClearAllBranches();
        bool dialogueIsFinished = true;

        //branch.Target(다음에 올 대상을 의미함)이 DialogueFragment인지 확인하고 변수에 대입.
        foreach (var branch in aBranches)
        {
            if (branch.Target is IDialogueFragment && dialogueIsFinished != false)
            {
                dialogueIsFinished = false;
            }
        }

        if (!dialogueIsFinished)
        {
            foreach (var branch in aBranches)
            {
                // branchLayoutPanel에 branchButton을 자식으로서 생성함
                GameObject btn = Instantiate(branchButton, branchLayoutPanel);
                // Let the BranchChoice component fill the button content
                btn.GetComponent<BranchManager>().AssignBranch(flowPlayer, branch);
            }
        }
        else
        {
            // 다이얼로그가 끝났으면 branchLayoutPanel에 closeButton을 자식으로서 생성함
            GameObject btn = Instantiate(closeButton, branchLayoutPanel);
            // Clicking this button will close the Dialogue UI
            var btnComp = btn.GetComponent<Button>();
            btnComp.onClick.AddListener(CloseDialogueBox);
        }
    }

    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        // If the object has a "Speaker" property try to fetch the speaker (객체에 "Speaker" 속성이 있다면 스피커를 가져오라.)
        var objectWithSpeaker = aObject as IObjectWithSpeaker;
        var speakerEntity = objectWithSpeaker.Speaker as Entity;

        //if (aObject is DialogueClose objWithLocalizableText2)
        if (aObject is DialogueClose)
        {
            CloseDialogueBox();
        }
        else if (aObject is MiniGame objWithLocalizableText1)
        {
            string eventCode = objWithLocalizableText1.Text;
            PrepareMiniGame(eventCode);
        }
        else if (aObject is IObjectWithLocalizableText objWithLocalizableText)
        {
            textLabel.text += "\n";
            textLabel.text += speakerEntity.DisplayName + " - ";
            textLabel.text += "\n";
            textLabel.text += objWithLocalizableText.Text;
            textLabel.text += "\n";
            
            Canvas.ForceUpdateCanvases(); //즉시 캔버스 업데이트
            ScrollToBottomPosition();    //스크롤바의 스크롤을 맨아래로 이동
        }
        else
        {
            textLabel.text = string.Empty;
        }

        ExtractCurrentPausePreviewImage(aObject); //화상(portrait) 가져오기
    }
    #endregion

    #region 동작을 보조하는 메소드들

    public void StartDialogue(IArticyObject aObject)
    {
        ButtonUninteractable_Panel.SetActive(true);
        InitTextbox();
        DialogueActive = true;
        dialogueWidget.SetActive(DialogueActive);
        flowPlayer.StartOn = aObject;
        MasterManager.Singleton.Disable_UI_InteractionForNPC();
        MasterManager.Singleton.DisablePlayerMovingManager();
    }

    public void CloseDialogueBox()
    {
        ButtonUninteractable_Panel.SetActive(false);
        DialogueActive = false;
        dialogueWidget.SetActive(DialogueActive);
        flowPlayer.FinishCurrentPausedObject();
        MasterManager.Singleton.EnablePlayerMovingManager();
    }

    public void SetArticyObject(ArticyObject articyObj)
    {
        articyObject = articyObj;
    }

    public void onClickTalk()
    {
        if (!DialogueManagerAudioSource.isPlaying)
        {
            DialogueManagerAudioSource.PlayOneShot(DialogueManagerAudioClip);
        }
        StartDialogue(articyObject);
    }

    private void InitTextbox()
    {
        textLabel.text = string.Empty;

        for (int i = 0; i < 11; ++i)
        {
            textLabel.text += "\n";
        }
    }

    private void ClearAllBranches()
    {
        foreach (Transform child in branchLayoutPanel)
            Destroy(child.gameObject);
    }

    private void ScrollToBottomPosition()
    {
        if (scrollRect != null)
        {
            StartCoroutine(ScrollToBottomCoroutine());
        }
    }

    IEnumerator ScrollToBottomCoroutine()
    {
        yield return null; // 1 프레임 대기
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private void ExtractCurrentPausePreviewImage(IFlowObject aObject)
    {
        IAsset articyAsset = null;
        previewImagePanel.sprite = null;
        var dlgSpeaker = aObject as IObjectWithSpeaker;

        if (dlgSpeaker != null)
        {
            ArticyObject speaker = dlgSpeaker.Speaker;
            if (speaker != null)
            {
                var speakerWithPreviewImage = speaker as IObjectWithPreviewImage;
                if (speakerWithPreviewImage != null)
                {
                    articyAsset = speakerWithPreviewImage.PreviewImage.Asset;
                }

                if (articyAsset == null)
                {
                    var objectWithPreviewImage = aObject as IObjectWithPreviewImage;
                    if (objectWithPreviewImage != null)
                    {
                        articyAsset = objectWithPreviewImage.PreviewImage.Asset;
                    }
                }
                if (articyAsset != null)
                {
                    previewImagePanel.sprite = articyAsset.LoadAssetAsSprite();
                }
            }
        }
        if (articyAsset == null)
        {
            var objectWithPreviewImage = aObject as IObjectWithPreviewImage;
        }
    }

    #endregion

    #region Minigame 관련 메서드들

    IEnumerator MinigameStart(string StatusType, int StatusValue, int ThresholdValue)
    {
        branchLayoutPanelObj.SetActive(false); //선택지 패널 비활성화

        if (StatusType == "Cook")
        {
            CookStatusPannel.SetActive(true);
            MechanicStatusPannel.SetActive(false);
        }
        else if (StatusType == "Mechanic")
        {
            CookStatusPannel.SetActive(false);
            MechanicStatusPannel.SetActive(true);
        }

        MinigamePanel.SetActive(true);

        // 특정 버튼이 눌릴 때까지 대기
        while (MinigameManager.isMinigameOver == false)
        {
            yield return null; // 매 프레임 대기
        }

        MinigameManager.isMinigameOver = false;

        MinigamePanel.SetActive(false);
        branchLayoutPanelObj.SetActive(true); //선택지 패널 활성화

        if (MinigameManager.ResultIndex == 1 || MinigameManager.ResultIndex == 10)
        {
            if (MinigameManager.ResultIndex == 1)
            {
                ArticyGlobalVariables.Default.Var_Minigame.MinigameResult = false;
                textLabel.text += "\n";
                textLabel.text += "<color=red><b>미니게임 대실패?!?!</b></color>";
                textLabel.text += "\n";
            }
            else if (MinigameManager.ResultIndex == 10)
            {
                ArticyGlobalVariables.Default.Var_Minigame.MinigameResult = true;
                textLabel.text += "\n";
                textLabel.text += "<color=blue><b>미니게임 대성공!!!!</b></color>";
                textLabel.text += "\n";
            }
        }
        else
        {
            if (MinigameManager.ResultIndex + StatusValue >= ThresholdValue)
            {
                ArticyGlobalVariables.Default.Var_Minigame.MinigameResult = true;
                textLabel.text += "\n";
                textLabel.text += "<color=blue>미니게임 성공</color>";
                textLabel.text += "\n";
            }
            else
            {
                ArticyGlobalVariables.Default.Var_Minigame.MinigameResult = false;
                textLabel.text += "\n";
                textLabel.text += "<color=red>미니게임 실패</color>";
                textLabel.text += "\n";
            }
        }

        flowPlayer.Play(0);
    }

    public void PrepareMiniGame(string eventCode)
    {
        if (eventCode == "MINIGAME1")
        {
            var HEROStatusTemplate = ArticyDatabase.GetObject<HERO_Status>("Ntt_D5A1D9E5");
            int MechanicLevel = HEROStatusTemplate.Template.HEROStatus.MechanicLevelValue;
            SetMinigameCondition(6);
            StartCoroutine(MinigameStart("Mechanic", MechanicLevel, 6));
        }
        if (eventCode == "MINIGAME2")
        {
            var HEROStatusTemplate = ArticyDatabase.GetObject<HERO_Status>("Ntt_D5A1D9E5");
            int MechanicLevel = HEROStatusTemplate.Template.HEROStatus.MechanicLevelValue;
            SetMinigameCondition(9);
            StartCoroutine(MinigameStart("Mechanic", MechanicLevel, 9));
        }
        if (eventCode == "MINIGAME3")//엘리베이터 비밀번호를 얻다
        {
            itemAddRemove.addItem(0);
            flowPlayer.Play(0);
        }
        if (eventCode == "MINIGAME10")
        {
            var HEROStatusTemplate = ArticyDatabase.GetObject<HERO_Status>("Ntt_D5A1D9E5");
            int CookLevel = HEROStatusTemplate.Template.HEROStatus.CookLevelValue;
            SetMinigameCondition(12);
            StartCoroutine(MinigameStart("Cook", CookLevel, 12));
        }
        if (eventCode == "MINIGAME11")
        {
            var HEROStatusTemplate = ArticyDatabase.GetObject<HERO_Status>("Ntt_D5A1D9E5");
            int CookLevel = HEROStatusTemplate.Template.HEROStatus.CookLevelValue;
            SetMinigameCondition(9);
            StartCoroutine(MinigameStart("Cook", CookLevel, 9));
        }
        if (eventCode == "MINIGAME12")//생선요리를 얻다
        {
            itemAddRemove.addItem(2);
            itemAddRemove.destroyItem(3);
            flowPlayer.Play(0);
        }
        if (eventCode == "MINIGAME13")//자동차핸들을 잃다
        {
            itemAddRemove.destroyItem(1);
            flowPlayer.Play(0);
        }
        if (eventCode == "MINIGAME100")
        {
            var HEROStatusTemplate = ArticyDatabase.GetObject<HERO_Status>("Ntt_D5A1D9E5");
            int MechanicLevel = HEROStatusTemplate.Template.HEROStatus.MechanicLevelValue;
            SetMinigameCondition(99);
            StartCoroutine(MinigameStart("Mechanic", MechanicLevel, 99));
        }
        if (eventCode == "MINIGAME101")
        {
            CloseDialogueBox();
            SignalScript.Timeline1.Play();
        }
        if (eventCode == "MINIGAME102")//일기장을 얻다
        {
            itemAddRemove.addItem(9);
            flowPlayer.Play(0);
        }
    }

    public void SetMinigameCondition(int difficulty)
    {
        switch (difficulty)
        {
            case 6:
                StatusNotice.text = ">= 6";
                break;
            case 9:
                StatusNotice.text = ">= 9";
                break;
            case 12:
                StatusNotice.text = ">= 12";
                break;
            case 99:
                StatusNotice.text = ">= 99";
                break;
            default:
                // 위에 없는 모든 값에 대한 실행 코드
                Debug.Log("difficulty is wrong");
                break;
        }
    }

    #endregion
}
