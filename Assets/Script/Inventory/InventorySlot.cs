using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public InventoryItem MyItem { get; set; }
    public Item settingItem;

    public void SetItem(InventoryItem item)
    {
        // ���� ���Կ� �������� �ִ��� Ȯ��
        InventoryItem existingItem = MyItem;

        // ���� ���� �ʱ�ȭ
        if (item.ActiveSlot != null)
        {
            // ���� ������ �����۰� settingItem�� �ʱ�ȭ
            item.ActiveSlot.MyItem = null;
            item.ActiveSlot.settingItem = null;
        }

        // ���� ���Կ� �������� �ִٸ� ���� ó��
        if (existingItem != null)
        {
            // ���� �������� ���� ������Ʈ
            existingItem.ActiveSlot = item.ActiveSlot;
            existingItem.transform.SetParent(item.ActiveSlot.transform);
            existingItem.transform.localPosition = Vector3.zero;
            item.ActiveSlot.MyItem = existingItem;

            // ���� �������� settingItem ����
            item.ActiveSlot.settingItem = existingItem.ItemInfo;
        }

        // �巡���� �������� ���� ������Ʈ
        MyItem = item;
        MyItem.ActiveSlot = this;
        MyItem.transform.SetParent(transform);
        MyItem.transform.localPosition = Vector3.zero;
        MyItem.CanvasGroup.blocksRaycasts = true;

        // ���� ������ settingItem ����
        settingItem = MyItem.ItemInfo;
    }
}