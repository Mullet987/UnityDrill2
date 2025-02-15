using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Articy.Unity;
using Articy.Articyproject_unitydrill2;

public class StatusPannelManager : MonoBehaviour
{
    [SerializeField]
    GameObject CookSlot;
    [SerializeField]
    GameObject MechanicSlot;
    [SerializeField]
    TextMeshProUGUI StatusPannelCookLevel;
    [SerializeField]
    TextMeshProUGUI StatusPannelMechanicLevel;

    public TextMeshProUGUI AvailableStatPointsText;

    public Sprite[] NumberSprite;
    public List<Image> SlotSprite1 = new List<Image>();
    public List<Image> SlotSprite2 = new List<Image>();
    public List<Button> ButtonSlots = new List<Button>();

    private int AvailableStatPointsTextInt;
    public int CookLevel = 1;
    public int MechanicLevel = 1;

    void Start()
    {
        InitializeStatusSlot();
        AvailableStatPointsText.text = "4";
        AvailableStatPointsTextInt = int.Parse(AvailableStatPointsText.text);
    }

    public void UpdateAvailableStatPointsText(int AvailableStatPointsTextInt)
    {
        AvailableStatPointsText.text = AvailableStatPointsTextInt.ToString();
    }

    public void ClickCookSlotMius()
    {
        CookSlot.transform.localPosition -= new Vector3(0, 100f, 0);

        if (CookSlot.transform.localPosition == new Vector3(0, 0, 0))
        {
            ButtonSlots[1].interactable = false;
        }
        ButtonSlots[0].interactable = true;
        CookLevel = CookLevel - 1;
        AvailableStatPointsTextInt = AvailableStatPointsTextInt + 1;
        UpdateAvailableStatPointsText(AvailableStatPointsTextInt);

        if (AvailableStatPointsTextInt != 0)
        {
            ButtonSlots[0].interactable = true;
            ButtonSlots[2].interactable = true;
        }
    }

    public void ClickCookSlotPlus()
    {
        CookSlot.transform.localPosition += new Vector3(0, 100f, 0);

        if (CookSlot.transform.localPosition == new Vector3(0, 400, 0))
        {
            ButtonSlots[0].interactable = false;
        }

        ButtonSlots[1].interactable = true;
        CookLevel = CookLevel + 1;
        AvailableStatPointsTextInt = AvailableStatPointsTextInt - 1;
        UpdateAvailableStatPointsText(AvailableStatPointsTextInt);

        if (AvailableStatPointsTextInt == 0)
        {
            ButtonSlots[0].interactable = false;
            ButtonSlots[2].interactable = false;
        }
    }

    public void ClickMechanicSlotMius()
    {
        MechanicSlot.transform.localPosition -= new Vector3(0, 100f, 0);

        if (MechanicSlot.transform.localPosition == new Vector3(0, 0, 0))
        {
            ButtonSlots[3].interactable = false;
        }
        ButtonSlots[2].interactable = true;
        MechanicLevel = MechanicLevel - 1;
        AvailableStatPointsTextInt = AvailableStatPointsTextInt + 1;
        UpdateAvailableStatPointsText(AvailableStatPointsTextInt);

        if (AvailableStatPointsTextInt != 0)
        {
            ButtonSlots[0].interactable = true;
            ButtonSlots[2].interactable = true;
        }
    }

    public void ClickMechanicSlotPlus()
    {
        MechanicSlot.transform.localPosition += new Vector3(0, 100f, 0);

        if (MechanicSlot.transform.localPosition == new Vector3(0, 400, 0))
        {
            ButtonSlots[2].interactable = false;
        }

        ButtonSlots[3].interactable = true;
        MechanicLevel = MechanicLevel + 1;
        AvailableStatPointsTextInt = AvailableStatPointsTextInt - 1;
        UpdateAvailableStatPointsText(AvailableStatPointsTextInt);

        if (AvailableStatPointsTextInt == 0)
        {
            ButtonSlots[0].interactable = false;
            ButtonSlots[2].interactable = false;
        }
    }

    private void InitializeStatusSlot()
    {
        for (int i = 0; i < 5; i++)
        {
            SlotSprite1[i].sprite = NumberSprite[i];
        }

        for (int i = 0; i < 5; i++)
        {
            SlotSprite2[i].sprite = NumberSprite[i];
        }
    }

    public void ClickStatusApplyButton() { 
    
        var HEROStatusTemplate = ArticyDatabase.GetObject<HERO_Status>("Ntt_D5A1D9E5");

        if (HEROStatusTemplate != null) {
            HEROStatusTemplate.Template.HEROStatus.CookLevelValue = CookLevel;
            HEROStatusTemplate.Template.HEROStatus.MechanicLevelValue = MechanicLevel;
        }

        StatusPannelCookLevel.text = CookLevel.ToString();
        StatusPannelMechanicLevel.text = MechanicLevel.ToString();

        Destroy(this.gameObject);
    }
}
