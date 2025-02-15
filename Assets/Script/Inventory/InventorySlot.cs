using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public InventoryItem MyItem { get; set; }
    public Item settingItem;

    public void SetItem(InventoryItem item)
    {
        // 현재 슬롯에 아이템이 있는지 확인
        InventoryItem existingItem = MyItem;

        // 이전 슬롯 초기화
        if (item.ActiveSlot != null)
        {
            // 이전 슬롯의 아이템과 settingItem을 초기화
            item.ActiveSlot.MyItem = null;
            item.ActiveSlot.settingItem = null;
        }

        // 현재 슬롯에 아이템이 있다면 스왑 처리
        if (existingItem != null)
        {
            // 기존 아이템의 슬롯 업데이트
            existingItem.ActiveSlot = item.ActiveSlot;
            existingItem.transform.SetParent(item.ActiveSlot.transform);
            existingItem.transform.localPosition = Vector3.zero;
            item.ActiveSlot.MyItem = existingItem;

            // 기존 아이템의 settingItem 갱신
            item.ActiveSlot.settingItem = existingItem.ItemInfo;
        }

        // 드래그한 아이템의 슬롯 업데이트
        MyItem = item;
        MyItem.ActiveSlot = this;
        MyItem.transform.SetParent(transform);
        MyItem.transform.localPosition = Vector3.zero;
        MyItem.CanvasGroup.blocksRaycasts = true;

        // 현재 슬롯의 settingItem 갱신
        settingItem = MyItem.ItemInfo;
    }
}