using EPOOutline;
using UnityEngine;
using UnityEngine.InputSystem;
using Articy.Unity;
using TMPro;

public class InteractionManager : ManagerBase
{
    public override void EnableManager()
    {
        this.gameObject.SetActive(true);
    }

    public override void DisableManager()
    {
        this.gameObject.SetActive(false);
    }

    private DialogueManager dialogueManager;
    [SerializeField]
    private ButtonManager buttonManager;
    [SerializeField]
    private GameObject dialogueBallon;
    [SerializeField]
    private TextMeshProUGUI dialogueBallonText;

    #region interaction 메뉴 모음
    private GameObject UI_NPC;
    private GameObject UI_Loot;
    private GameObject UI_Observe;
    #endregion

    #region 상호작용 버튼 위치에 쓸 변수
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private float interactionDistance;
    private Transform heroTransform;
    private RaycastHit interactionObject;
    private Vector2 InteractionUI_offset = new Vector2(0, -40);
    private bool isInteractionMenuOn = false;
    #endregion

    #region Outline 관련 변수
    public Outliner outlineCamera;

    private InputSystem_Actions playerAction;
    private GameObject outlineTarget;
    private bool isOutlining;
    #endregion

    private void Start()
    {
        UI_NPC = MasterManager.Singleton._UI_InteractionForNPC;
        UI_Loot = MasterManager.Singleton._UI_InteractionForLoot;
        UI_Observe = MasterManager.Singleton._UI_InteractionForObserve;

        GameObject hero = GameObject.Find("HERO");
        heroTransform = hero.transform;
        GameObject dialogueManagerObj = GameObject.Find("DialogueManager");
        dialogueManager = dialogueManagerObj.GetComponent<DialogueManager>();
    }

    private void Update()
    {
        DisableInteractionMenuOfDistance();
        InteractionNPC();
        onOutlining(isOutlining);
    }

    #region Ray를 쬐어서 오브젝트와 상호작용

    private void InteractionNPC()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "NPC" || hit.collider.tag == "LootObject" || hit.collider.tag == "ObserveObject")
            {
                outlineTarget = hit.collider.gameObject;
                CanSeeOutline(outlineTarget);

                if (Input.GetMouseButtonDown(1))
                {
                    Vector3 heroPosition = heroTransform.position;
                    float distance = Vector3.Distance(hit.transform.position, heroPosition);
                    if (distance <= interactionDistance)
                    {
                        isInteractionMenuOn = true; //상호작용 메뉴가 켜졌다는 거 알림. Update문에서 이를 활용함.
                        interactionObject = hit; //Ray를 쬔 오브젝트를 다른 데에서 활용하기 위해서 변수에 할당.
                        ArticyReference articyReference = hit.collider.GetComponent<ArticyReference>(); //ArticyReference 있는지 확인

                        if (articyReference != null)
                        {
                            //ArticyReference에서 ArticyObject를 얻어 DialogueManager에 셋팅.
                            ArticyObject articyObject = articyReference.GetObject<ArticyObject>();
                            dialogueManager.SetArticyObject(articyObject);
                        }

                        if (hit.collider.tag == "NPC")
                        {
                            CahngeUIPosition(UI_NPC);
                            MasterManager.Singleton.Enable_UI_InteractionForNPC();
                        }
                        else if (hit.collider.tag == "LootObject")
                        {
                            buttonManager.SetLootInventory(outlineTarget);
                            CahngeUIPosition(UI_Loot);
                            MasterManager.Singleton.Enable_UI_InteractionForLoot();
                        }
                        else if (hit.collider.tag == "ObserveObject")
                        {
                            CahngeUIPosition(UI_Observe);
                            MasterManager.Singleton.Enable_UI_InteractionForObserve();
                        }
                    }
                    else
                    {
                        dialogueBallonText.text = "더 가까이 가보자";
                        dialogueBallon.SetActive(true);
                    }
                }
            }
            else
            {
                if (outlineTarget != null)
                {
                    //마우스 떼면 아웃라인 사라지게 하기
                    CannotSeeOutline(outlineTarget);
                    outlineTarget = null;
                }
            }
        }

    }
    #endregion

    #region 마우스를 가져다대면 outline 나오기

    public void CanSeeOutline(GameObject targetObject)
    {
        Outlinable outlinable = targetObject.GetComponent<Outlinable>();
        outlinable.OutlineLayer = 1;
    }

    public void CannotSeeOutline(GameObject targetObject)
    {
        Outlinable outlinable = targetObject.GetComponent<Outlinable>();
        outlinable.OutlineLayer = 0;
    }

    #endregion

    #region Tab키를 누르면 Outline이 계속 나오기
    void OnEnable()
    {
        // Outlining 관련
        // PlayerActionMap 객체 초기화
        playerAction = new InputSystem_Actions();
        // Outlining 액션에 대한 콜백을 등록
        playerAction.Player.Outlining.started += OnOutliningPressed;
        playerAction.Player.Outlining.canceled += OnOutliningReleased;

        playerAction.Enable();
    }

    void OnDisable()
    {
        // 이벤트 콜백을 해제
        playerAction.Player.Outlining.started -= OnOutliningPressed;
        playerAction.Player.Outlining.canceled -= OnOutliningReleased;

        playerAction.Disable();
    }

    private void OnOutliningPressed(InputAction.CallbackContext context)
    {
        isOutlining = true;
    }

    private void OnOutliningReleased(InputAction.CallbackContext context)
    {
        isOutlining = false;
    }

    private void onOutlining(bool isOutlining)
    {
        if (isOutlining == true)
        {
            outlineCamera.OutlineLayerMask = 3;
        }
        else if (isOutlining == false)
        {
            outlineCamera.OutlineLayerMask = 2;
        }
    }
    #endregion

    #region 상호작용 버튼의 위치를 조정하기
    private void CahngeUIPosition(GameObject interactionUI) //상호작용 UI가 나타날 위치를 조정
    {
        Vector2 screenPoint = Input.mousePosition;

        //상호작용 UI의 위치를 좌표계로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPoint,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        localPoint += InteractionUI_offset;   // 오프셋 추가

        RectTransform uiElement = interactionUI.GetComponent<RectTransform>();

        uiElement.localPosition = localPoint;   // UI 요소 이동
    }
    #endregion

    private void DisableInteractionMenuOfDistance()
    {
        //interaction 메뉴가 켜졌을 때, 일정 거리 이상 멀어지면 자동으로 메뉴가 꺼짐

        if (isInteractionMenuOn == true)
        {
            Vector3 heroPosition = heroTransform.position;
            float distance = Vector3.Distance(interactionObject.transform.position, heroPosition);

            if (distance >= interactionDistance)
            {
                if (UI_NPC.activeSelf == true)
                {
                    MasterManager.Singleton.Disable_UI_InteractionForNPC();
                    isInteractionMenuOn = false;
                }
                if (UI_Loot.activeSelf == true)
                {
                    MasterManager.Singleton.Disable_UI_InteractionForLoot();
                    isInteractionMenuOn = false;
                }
                if (UI_Observe.activeSelf == true)
                {
                    MasterManager.Singleton.Disable_UI_InteractionForObserve();
                    isInteractionMenuOn = false;
                }
            }
        }
    }

}