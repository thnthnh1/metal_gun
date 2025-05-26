using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HeroLeftPanel : MonoBehaviour
{
    public TextMeshProUGUI charactername;
    public TextMeshProUGUI characterLevel;

    public void ApplyInfo()
    {
        charactername.text = S.Instance.characterDat.namex;
        characterLevel.text =$"{S.Instance.characterDat.level}" ;
    }
    public void TouchUpgrade ()
    {
        S.Instance.characterDat.level += 1;
        ApplyInfo();
    }
}
