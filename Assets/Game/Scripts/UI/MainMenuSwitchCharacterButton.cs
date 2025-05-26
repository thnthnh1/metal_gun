using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.EventSystems.StandaloneInputModule;
using ET.SupportKit;

public class MainMenuSwitchCharacterButton : MonoBehaviour
{
    public TextMeshProUGUI currentCharacter;
    private void OnEnable()
    {
        //UpdateText();
    }
    
    public void UpdateText()
    {
        ProfileManager.UserProfile.ramboId.Set(S.Instance.characterDat.curRambo);
        currentCharacter.text = GameResourcesUtils.GetRamboName(ProfileManager.UserProfile.ramboId);
    }

    public void SwitchCharacter()
    {
        if (S.Instance.characterDat.ownedRambo.Count>1)
        {
            S.Instance.characterDat.curRambo = S.Instance.characterDat.curRambo.GetNextValueInList(S.Instance.characterDat.ownedRambo);
            S.Instance.Save();
            UpdateText();
        }

    }
    public void SwitchCharacter(int index)
    {
        S.Instance.characterDat.curRambo = index;
        S.Instance.Save();
        UpdateText();

    }
}
