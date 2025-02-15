using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : ManagerBase
{
    public override void DisableManager()
    {
        this.gameObject.SetActive(false);
    }

    public override void EnableManager()
    {
        this.gameObject.SetActive(true);
    }

    [SerializeField]
    private GameObject HEROInventory;
    [SerializeField]
    private GameObject LootInventoryParent;
    [SerializeField]
    private GameObject UI_GamePausePanel;

    private DOTweenAnimation HEROInvenAnimation;
    private GameObject UI_InteractionForNPC;
    private GameObject UI_InteractionForLoot;
    private GameObject lootItem;
    private GameObject ItemLootInven;
    private bool isHeroInventoryOn = false;
    private bool isLootInventoryOn = false;
    private ItemCheck itemCheck;
    private AudioSource ButtonManagerAudioSource;

    private void Start()
    {
        HEROInvenAnimation = HEROInventory.GetComponent<DOTweenAnimation>();
        itemCheck = GetComponent<ItemCheck>();
        ButtonManagerAudioSource = GetComponent<AudioSource>();

        UI_InteractionForNPC = MasterManager.Singleton._UI_InteractionForNPC;
        UI_InteractionForLoot = MasterManager.Singleton._UI_InteractionForLoot;
    }

    public void ClickCloseButton()
    {
        if (!ButtonManagerAudioSource.isPlaying)
        {
            ButtonManagerAudioSource.Play();
        }

        isHeroInventoryOn = false; //���ΰ� �κ��丮 �����ٰ� �˸�
        isLootInventoryOn = false; //������Ʈ �κ��丮 �����ٰ� �˸�
        ItemDescriptionNull(); //������ ����� ��������.
        HEROInvenAnimation.DOPlayBackwardsById("HEROInventoryOn"); //DoTween �ִϸ��̼� �����.
        if (ItemLootInven != null)
        {
            StartCoroutine(WaitForOneSecond(ItemLootInven)); //���ΰ� �κ��丮�� �ڽ����� �� Ư�� ������Ʈ �κ��丮��, �ٽ� '������Ʈ �κ��丮(����)'�� �ڽ����� ����.
        }
        itemCheck.CheckHeroInventorySlot();
        MasterManager.Singleton.EnablePlayerMovingManager();
    }

    public void ClickHEROInventory()
    {
        if (!ButtonManagerAudioSource.isPlaying)
        {
            ButtonManagerAudioSource.Play();
        }

        DeactiveInteractionMenu();

        if (isHeroInventoryOn == false)
        {
            if (lootItem != null)
            {
                lootItem = null;
                //������ ������ ���ΰ� �κ��丮�� �������� �Ѵ�
                //�ٵ� �̹� ������Ʈ �κ��丮�� �����ߴ�
                //�׷� ������Ʈ �κ��丮�� null�� ���� ���ΰ� �κ��丮�� �����ϰ� �Ѵ�
            }
            isHeroInventoryOn = true; //���ΰ� �κ��丮 �����ٰ� �˸�
            HEROInvenAnimation.DORestartById("HEROInventoryOn"); //DoTween �ִϸ��̼� ���.
            MasterManager.Singleton.DisablePlayerMovingManager(); //�÷��̾� �� �����̰� �Ŵ��� ����
        }
        else if (isHeroInventoryOn == true)
        {
            ClickCloseButton();
        }
    }

    public void ClickLootInventory()
    {
        if (!ButtonManagerAudioSource.isPlaying)
        {
            ButtonManagerAudioSource.Play();
        }

        DeactiveInteractionMenu();

        if (lootItem != null)
        {
            if (isLootInventoryOn == false)
            {
                ItemLootInven.transform.SetParent(HEROInventory.transform); //Ư�� ������Ʈ �κ��丮�� ���ΰ� �κ��丮�� �ڽ����� ����
                HEROInvenAnimation.DORestartById("HEROInventoryOn"); //DoTween �ִϸ��̼� ���.
                isLootInventoryOn = true; //���ΰ� �κ��丮 �����ٰ� �˸�
                isHeroInventoryOn = true; //������Ʈ �κ��丮 �����ٰ� �˸�
                MasterManager.Singleton.DisablePlayerMovingManager(); //�÷��̾� �� �����̰� �Ŵ��� ����
            }
        }
    }

    public void ClickPauseButton()
    {
        if (UI_GamePausePanel.activeSelf == true)
        {
            UI_GamePausePanel.SetActive(false);
            MasterManager.Singleton.EnablePlayerMovingManager();
        }
        else if (UI_GamePausePanel.activeSelf == false)
        {
            UI_GamePausePanel.SetActive(true);
            MasterManager.Singleton.DisablePlayerMovingManager();
        }
    }

    public void QuitGame()
    {
        // ���� ����
        Application.Quit();
    }


    private void DeactiveInteractionMenu()
    {
        if (UI_InteractionForNPC.activeSelf == true || UI_InteractionForLoot.activeSelf == true)
        {
            MasterManager.Singleton._UI_InteractionForNPC.SetActive(false);
            MasterManager.Singleton._UI_InteractionForLoot.SetActive(false);
        }
    }

    private void ItemDescriptionNull()
    {
        GameObject founditemName = GameObject.Find("Item_Name");
        TextMeshProUGUI itemName = founditemName.GetComponent<TextMeshProUGUI>();
        itemName.text = null;

        GameObject founditemDescription = GameObject.Find("Item_Description");
        TextMeshProUGUI itemDescription = founditemDescription.GetComponent<TextMeshProUGUI>();
        itemDescription.text = null;
    }

    public void SetLootInventory(GameObject setlootItem)
    {
        lootItem = setlootItem;
        ItemLootInven = lootItem.GetComponent<InteractableObject_Infomation>().LootInven;
    }

    IEnumerator WaitForOneSecond(GameObject lootInven)
    {
        yield return new WaitForSeconds(1f);
        lootInven.transform.SetParent(LootInventoryParent.transform);
        lootInven.transform.localPosition = new Vector3(0, 1f, 0);
        MasterManager.Singleton.EnablePlayerMovingManager(); //�÷��̾� ������ �� �ְ� �Ŵ��� �ѱ�
    }

    public void ClickRestartButton()
    {
        SceneManager.LoadScene("InitialScreen");
    }
}