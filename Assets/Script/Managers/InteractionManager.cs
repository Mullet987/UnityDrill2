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

    #region interaction �޴� ����
    private GameObject UI_NPC;
    private GameObject UI_Loot;
    private GameObject UI_Observe;
    #endregion

    #region ��ȣ�ۿ� ��ư ��ġ�� �� ����
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private float interactionDistance;
    private Transform heroTransform;
    private RaycastHit interactionObject;
    private Vector2 InteractionUI_offset = new Vector2(0, -40);
    private bool isInteractionMenuOn = false;
    #endregion

    #region Outline ���� ����
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

    #region Ray�� �ؾ ������Ʈ�� ��ȣ�ۿ�

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
                        isInteractionMenuOn = true; //��ȣ�ۿ� �޴��� �����ٴ� �� �˸�. Update������ �̸� Ȱ����.
                        interactionObject = hit; //Ray�� �� ������Ʈ�� �ٸ� ������ Ȱ���ϱ� ���ؼ� ������ �Ҵ�.
                        ArticyReference articyReference = hit.collider.GetComponent<ArticyReference>(); //ArticyReference �ִ��� Ȯ��

                        if (articyReference != null)
                        {
                            //ArticyReference���� ArticyObject�� ��� DialogueManager�� ����.
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
                        dialogueBallonText.text = "�� ������ ������";
                        dialogueBallon.SetActive(true);
                    }
                }
            }
            else
            {
                if (outlineTarget != null)
                {
                    //���콺 ���� �ƿ����� ������� �ϱ�
                    CannotSeeOutline(outlineTarget);
                    outlineTarget = null;
                }
            }
        }

    }
    #endregion

    #region ���콺�� �����ٴ�� outline ������

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

    #region TabŰ�� ������ Outline�� ��� ������
    void OnEnable()
    {
        // Outlining ����
        // PlayerActionMap ��ü �ʱ�ȭ
        playerAction = new InputSystem_Actions();
        // Outlining �׼ǿ� ���� �ݹ��� ���
        playerAction.Player.Outlining.started += OnOutliningPressed;
        playerAction.Player.Outlining.canceled += OnOutliningReleased;

        playerAction.Enable();
    }

    void OnDisable()
    {
        // �̺�Ʈ �ݹ��� ����
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

    #region ��ȣ�ۿ� ��ư�� ��ġ�� �����ϱ�
    private void CahngeUIPosition(GameObject interactionUI) //��ȣ�ۿ� UI�� ��Ÿ�� ��ġ�� ����
    {
        Vector2 screenPoint = Input.mousePosition;

        //��ȣ�ۿ� UI�� ��ġ�� ��ǥ��� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPoint,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        localPoint += InteractionUI_offset;   // ������ �߰�

        RectTransform uiElement = interactionUI.GetComponent<RectTransform>();

        uiElement.localPosition = localPoint;   // UI ��� �̵�
    }
    #endregion

    private void DisableInteractionMenuOfDistance()
    {
        //interaction �޴��� ������ ��, ���� �Ÿ� �̻� �־����� �ڵ����� �޴��� ����

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