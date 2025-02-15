using UnityEngine;
using UnityEngine.UI;
using Articy.Unity;
using Articy.Unity.Interfaces;

public class BranchManager : MonoBehaviour
{
    private ArticyFlowPlayer flowPlayer;
    private Branch branch;
    private Text branchText;

    internal void AssignBranch(ArticyFlowPlayer aFlowPlayer, Branch aBranch)
    {
        GetComponent<Button>().onClick.AddListener(OnBranchSelected);
        branchText = GetComponentInChildren<Text>();

        branch = aBranch;
        flowPlayer = aFlowPlayer;

        var target = aBranch.Target;
        branchText.text = string.Empty;

        if (target is IDialogueFragment)
        {
            if (target is IObjectWithMenuText objWithMenuText)
                branchText.text = objWithMenuText.MenuText;
            else if (target is IObjectWithLocalizableMenuText objWithLocalizableMenuText)
                branchText.text = objWithLocalizableMenuText.MenuText;
            if (branchText.text == "")
            {
                branchText.text = "CONTINUE";
            }
        }
    }

    public void OnBranchSelected()
    {
        flowPlayer.Play(branch);
    }
}
