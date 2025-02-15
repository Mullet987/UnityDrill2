using UnityEngine;
using Articy.Articyproject_unitydrill2.GlobalVariables;
using System.Linq;
using System.Collections.Generic;
public class ItemCheck : MonoBehaviour
{
    [SerializeField]
    InventorySlot[] HeroInventorySlot;

    private List<int> OwnedItemsList = new List<int>();

    private int ownItem0 = 0;
    private int ownItem1 = 1;
    private int ownItem2 = 2;
    private int ownItem3 = 3;
    private int ownItem4 = 4;
    private int ownItem5 = 5;
    private int ownItem6 = 6;
    private int ownItem7 = 7;
    private int ownItem8 = 8;
    private int ownItem9 = 9;
    public void CheckHeroInventorySlot()
    {
        OwnedItemsList.Clear();

        for (int i = 0; i < HeroInventorySlot.Length; ++i)
        {
            if (HeroInventorySlot[i].settingItem != null)
            {
                if (HeroInventorySlot[i].settingItem.itemIndex == 0)
                {
                    OwnedItemsList.Add(ownItem0);
                }
                if (HeroInventorySlot[i].settingItem.itemIndex == 1)
                {
                    OwnedItemsList.Add(ownItem1);
                }
                if (HeroInventorySlot[i].settingItem.itemIndex == 2)
                {
                    OwnedItemsList.Add(ownItem2);
                }
                if (HeroInventorySlot[i].settingItem.itemIndex == 3)
                {
                    OwnedItemsList.Add(ownItem3);
                }
                if (HeroInventorySlot[i].settingItem.itemIndex == 4)
                {
                    OwnedItemsList.Add(ownItem4);
                }
                if (HeroInventorySlot[i].settingItem.itemIndex == 5)
                {
                    OwnedItemsList.Add(ownItem5);
                }
                if (HeroInventorySlot[i].settingItem.itemIndex == 6)
                {
                    OwnedItemsList.Add(ownItem6);
                }
                if (HeroInventorySlot[i].settingItem.itemIndex == 7)
                {
                    OwnedItemsList.Add(ownItem7);
                }
                if (HeroInventorySlot[i].settingItem.itemIndex == 8)
                {
                    OwnedItemsList.Add(ownItem8);
                }
                if (HeroInventorySlot[i].settingItem.itemIndex == 9)
                {
                    OwnedItemsList.Add(ownItem9);
                }
            }

        }


        if (OwnedItemsList.Contains(0) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem0 = true;
        }
        else if (OwnedItemsList.Contains(0) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem0 = false;
        }

        if (OwnedItemsList.Contains(1) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem1 = true;
        }
        else if (OwnedItemsList.Contains(1) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem1 = false;
        }

        if (OwnedItemsList.Contains(2) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem2 = true;
        }
        else if (OwnedItemsList.Contains(2) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem2 = false;
        }

        if (OwnedItemsList.Contains(3) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem3 = true;
        }
        else if (OwnedItemsList.Contains(3) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem3 = false;
        }

        if (OwnedItemsList.Contains(4) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem4 = true;
        }
        else if (OwnedItemsList.Contains(4) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem4 = false;
        }

        if (OwnedItemsList.Contains(5) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem5 = true;
        }
        else if (OwnedItemsList.Contains(5) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem5 = false;
        }

        if (OwnedItemsList.Contains(6) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem6 = true;
        }
        else if (OwnedItemsList.Contains(6) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem6 = false;
        }

        if (OwnedItemsList.Contains(7) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem7 = true;
        }
        else if (OwnedItemsList.Contains(7) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem7 = false;
        }

        if (OwnedItemsList.Contains(8) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem8 = true;
        }
        else if (OwnedItemsList.Contains(8) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem8 = false;
        }

        if (OwnedItemsList.Contains(9) == true)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem9 = true;
        }
        else if (OwnedItemsList.Contains(9) == false)
        {
            ArticyGlobalVariables.Default.Var_ItemState.ownItem9 = false;
        }
    }
}
