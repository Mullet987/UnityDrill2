using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public CanvasGroup CanvasGroup { get; private set; }
    public Item ItemInfo { get; set; }
    public InventorySlot ActiveSlot { get; set; }

    private Image itemIcon;

    void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
    }

    public void Initialize(Item item, InventorySlot parent)
    {
        ItemInfo = item;
        ActiveSlot = parent;
        ActiveSlot.MyItem = this;
        itemIcon.sprite = item.sprite;
    }

    public void ShowingItemInfomation() {
        GameObject founditemName = GameObject.Find("Item_Name");
        TextMeshProUGUI itemName = founditemName.GetComponent<TextMeshProUGUI>();
        itemName.text = ItemInfo.itemName;

        GameObject founditemDescription = GameObject.Find("Item_Description");
        TextMeshProUGUI itemDescription = founditemDescription.GetComponent<TextMeshProUGUI>();
        itemDescription.text = ItemInfo.itemDescription;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ShowingItemInfomation();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ShowingItemInfomation();
        Inventory.Singleton.SetCarriedItem(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Inventory.carriedItem == null)
        {
            return;
        }

        // Raycast로 슬롯 감지
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        InventorySlot dropSlot = null;

        foreach (var result in results)
        {
            dropSlot = result.gameObject.GetComponent<InventorySlot>();
            if (dropSlot != null)
            {
                break;
            }
        }

        if (dropSlot != null)
        {
            dropSlot.SetItem(this);
        }
        else
        {
            // 슬롯이 없으면 원래 위치로 돌아가도록 처리
            transform.SetParent(ActiveSlot.transform);
            transform.localPosition = Vector3.zero;
            CanvasGroup.blocksRaycasts = true;
        }
    }
}
