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

        isHeroInventoryOn = false; //주인공 인벤토리 꺼졌다고 알림
        isLootInventoryOn = false; //오브젝트 인벤토리 꺼졌다고 알림
        ItemDescriptionNull(); //아이템 설명란 공란으로.
        HEROInvenAnimation.DOPlayBackwardsById("HEROInventoryOn"); //DoTween 애니메이션 역재생.
        if (ItemLootInven != null)
        {
            StartCoroutine(WaitForOneSecond(ItemLootInven)); //주인공 인벤토리에 자식으로 간 특정 오브젝트 인벤토리를, 다시 '오브젝트 인벤토리(모임)'의 자식으로 설정.
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
                //가방을 눌러서 주인공 인벤토리가 켜질려고 한다
                //근데 이미 오브젝트 인벤토리를 감지했다
                //그럼 오브젝트 인벤토리를 null로 만들어서 주인공 인벤토리만 감지하게 한다
            }
            isHeroInventoryOn = true; //주인공 인벤토리 켜졌다고 알림
            HEROInvenAnimation.DORestartById("HEROInventoryOn"); //DoTween 애니메이션 재생.
            MasterManager.Singleton.DisablePlayerMovingManager(); //플레이어 못 움직이게 매니저 끄기
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
                ItemLootInven.transform.SetParent(HEROInventory.transform); //특정 오브젝트 인벤토리를 주인공 인벤토리의 자식으로 만듬
                HEROInvenAnimation.DORestartById("HEROInventoryOn"); //DoTween 애니메이션 재생.
                isLootInventoryOn = true; //주인공 인벤토리 켜졌다고 알림
                isHeroInventoryOn = true; //오브젝트 인벤토리 켜졌다고 알림
                MasterManager.Singleton.DisablePlayerMovingManager(); //플레이어 못 움직이게 매니저 끄기
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
        // 게임 종료
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
        MasterManager.Singleton.EnablePlayerMovingManager(); //플레이어 움직일 수 있게 매니저 켜기
    }

    public void ClickRestartButton()
    {
        SceneManager.LoadScene("InitialScreen");
    }
}