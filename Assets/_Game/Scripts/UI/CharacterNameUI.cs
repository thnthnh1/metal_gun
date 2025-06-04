using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterNameUI : MonoBehaviour
{
    [SerializeField]
    private InputField _inputField;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnContinuePressed()
    {
        if (!string.IsNullOrEmpty(_inputField.text))
        {
            PlayerPrefs.SetString("playerName", _inputField.text);
            gameObject.SetActive(false);
        }
    }
}
