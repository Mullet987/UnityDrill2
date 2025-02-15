using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;
    public Transform draggablesTransform;

    [SerializeField]
    InventoryItem itemPrefab;
    InventorySlot[] inventorySlots;

    void Awake()
    {
        Singleton = this;
        inventorySlots = GetComponentsInChildren<InventorySlot>();
    }

    private void Start()
    {
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            if (inventorySlots[i].settingItem != null)
            {
                int number = i;
                SpawnInventoryItem(inventorySlots[number].settingItem, number);
            }
        }
    }

    public void SpawnInventoryItem(Item startingItem, int number)
    {
        Item _item = startingItem;
        InventoryItem newItem = Instantiate(itemPrefab, inventorySlots[number].transform);
        inventorySlots[number].settingItem = _item;
        newItem.Initialize(_item, inventorySlots[number]);
    }

    public void SetCarriedItem(InventoryItem item)
    {
        carriedItem = item;
        carriedItem.CanvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }
}
