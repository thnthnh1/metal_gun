using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NamingUI : MonoBehaviour
{
    public TextMeshProUGUI currentName;
    public TMP_InputField _InputField;
    public Button confirmBut;
    public HeroLeftPanel charaterUI;
    private void OnEnable()
    {
        currentName.text = S.Instance.characterDat.namex;
        _InputField.text = "";
    }
    public void OnValueChange(string value)
    {
        if (_InputField.text.Length > 4) confirmBut.interactable = true;
        else confirmBut.interactable = false;
    }
    public void Confirm()
    {
        S.Instance.characterDat.namex = _InputField.text;
        S.Instance.Save();
        charaterUI.ApplyInfo();
        gameObject.SetActive(false);
    }
    public void Cancle()
    {
        charaterUI.ApplyInfo();
        gameObject.SetActive(false);

    }
}
