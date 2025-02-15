using UnityEngine;

public class MasterManager : MonoBehaviour
{
    #region �ٸ� manager ���� �ѱ��� �⺻
    public void EnableManager(ManagerBase manager)
    {
        manager.EnableManager();
    }

    public void DisableManager(ManagerBase manager)
    {
        manager.DisableManager();
    }
    #endregion

    public static MasterManager Singleton;

    [Header("Managers")]
    [SerializeField]
    private PlayerMovingManager _playerMovingManager;
    [SerializeField]
    private InteractionManager _interactionManager;
    [SerializeField]
    private ButtonManager _buttonManager;
    [Header("Interaction Menus")]
    public GameObject _UI_InteractionForNPC;
    public GameObject _UI_InteractionForLoot;
    public GameObject _UI_InteractionForObserve;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 45; //45���������� ����
    }

    #region PlayerMovingManger ���� �ѱ�
    public void EnablePlayerMovingManager()
    {
        _interactionManager.outlineCamera.OutlineLayerMask = 2;
        EnableManager(_playerMovingManager);
    }

    public void DisablePlayerMovingManager()
    {
        _interactionManager.outlineCamera.OutlineLayerMask = 0;
        DisableManager(_playerMovingManager);
    }
    #endregion

    #region interaction ��ư���� �Ѱ� ����
    public void Enable_UI_InteractionForNPC()
    {
        _UI_InteractionForNPC.SetActive(true);
    }

    public void Enable_UI_InteractionForLoot()
    {
        _UI_InteractionForLoot.SetActive(true);
    }

    public void Enable_UI_InteractionForObserve()
    {
        _UI_InteractionForObserve.SetActive(true);
    }

    public void Disable_UI_InteractionForNPC()
    {
        _UI_InteractionForNPC.SetActive(false);
    }

    public void Disable_UI_InteractionForLoot()
    {
        _UI_InteractionForLoot.SetActive(false);
    }

    public void Disable_UI_InteractionForObserve()
    {
        _UI_InteractionForObserve.SetActive(false);
    }
    #endregion
}
