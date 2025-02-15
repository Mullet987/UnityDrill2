using Articy.Articyproject_unitydrill2.GlobalVariables;
using Unity.VisualScripting;
using UnityEngine;

public class ItemAddRemove : MonoBehaviour
{
    [SerializeField]
    Item[] items;
    [SerializeField]
    InventorySlot[] HEROInventorySlots;
    [SerializeField]
    Inventory inventory;

    public void destroyItem(int itemNumber)
    {
        for (int i = 0; i < HEROInventorySlots.Length; ++i)
        {
            if (HEROInventorySlots[i].settingItem != null)
            {
                if (HEROInventorySlots[i].settingItem.itemIndex == itemNumber)
                {
                    Transform child = HEROInventorySlots[i].transform.GetChild(0);
                    Destroy(child.gameObject);
                    HEROInventorySlots[i].settingItem = null;
                    setState(itemNumber, false);
                    return;
                }
            }
        }
    }

    public void addItem(int itemNumber)
    {
        for (int i = 0; i < HEROInventorySlots.Length; ++i)
        {
            if (HEROInventorySlots[i].settingItem == null)
            {
                int number = i;
                inventory.SpawnInventoryItem(items[itemNumber], number);
                setState(itemNumber, true);
                return;
            }
        }
    }

    private void setState(int itemNumber, bool addORremove)
    {
        switch (itemNumber)
        {
            case 0:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem0 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem0 = false;
                }
                break;
            case 1:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem1 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem1 = false;
                }
                break;
            case 2:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem2 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem2 = false;
                }
                break;
            case 3:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem3 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem3 = false;
                }
                break;
            case 4:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem4 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem4 = false;
                }
                break;
            case 5:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem5 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem5 = false;
                }
                break;
            case 6:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem6 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem6 = false;
                }
                break;
            case 7:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem7 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem7 = false;
                }
                break;
            case 8:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem8 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem8 = false;
                }
                break;
            case 9:
                if (addORremove == true)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem9 = true;
                }
                else if (addORremove == false)
                {
                    ArticyGlobalVariables.Default.Var_ItemState.ownItem9 = false;
                }
                break;
        }
    }
}
