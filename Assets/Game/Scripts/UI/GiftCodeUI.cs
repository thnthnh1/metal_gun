using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GiftCodeUI : MonoBehaviour, ISimpleUI
{
    public GameObject group0;
    public GameObject group1;
    public TMP_InputField inputCode;
    public TextMeshProUGUI ramboName;
    private int _newIndex = 0;
    public MainMenuSwitchCharacterButton mainMenuSwitchCharacterButton;
    public MainMenu mainMenu;
    public List<GameObject> heroesAvatar;

    private void OnEnable()
    {
        group0.SetActive(true);
        group1.SetActive(false);
    }
    public void CollectGift()
    {
        if (S.Instance.giftCodes.Contains(inputCode.text))
        {
            group1.SetActive(true);
            group0.SetActive(false);
            _newIndex = S.Instance.giftCodesHeroesIndex[S.Instance.giftCodes.IndexOf(inputCode.text)];
            ramboName.text = S.Instance.ramboName[_newIndex];
            foreach (GameObject avatar in heroesAvatar)
            {
                if (avatar.name == _newIndex.ToString()) avatar.SetActive(true);
                else avatar.SetActive(false);
            }
            S.Instance.characterDat.ownedRambo.Add(_newIndex);

            S.Instance.characterDat.curRambo = _newIndex;
            ProfileManager.UserProfile.ramboId.Set(S.Instance.characterDat.curRambo);
            ProfileManager.UserProfile.isClaimHeroPack.Set(true);
            S.Instance.Save();

            GameData.playerRambos[_newIndex].state = PlayerRamboState.Unlock;
            GameData.playerRambos.Save();

            //Rambo ramboPrefab = GameResourcesUtils.GetRamboPrefab(ProfileManager.UserProfile.ramboId);
            //Rambo rambo = UnityEngine.Object.Instantiate<Rambo>(ramboPrefab);
            mainMenu.ShowClaimHero();
        }
    }
    public void Continue()
    {
        Close();
    }
    public void Use()
    {
        mainMenuSwitchCharacterButton.SwitchCharacter(_newIndex);
        Close();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
