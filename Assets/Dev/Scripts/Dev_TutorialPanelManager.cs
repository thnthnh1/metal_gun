using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dev_TutorialPanelManager : MonoBehaviour
{
    public int currentImageIndex;
    public Image mainImage;
    public List<Sprite> listOfTutorialSprites = new List<Sprite>();

    void OnEnable()
    {
        currentImageIndex = 0;

        mainImage.sprite = listOfTutorialSprites[0];
    }

    void Start()
    {

    }

    public void RightLeftNavigation(bool isRight)
    {
        Debug.Log("<color=red>Nik Log is the Tutorial Enter </color>");
        if (listOfTutorialSprites.Count - 1 == currentImageIndex)
        {
            Mp_Armory.instance.OtherTutorial.SetActive(true);
            PlayerPrefs.SetInt("NotifyTutorial", 1);
            PlayerPrefs.Save();
            if (FindObjectOfType<MainMenuAnimationEvent>())
            {
                Debug.Log("Nik log return 2 call ");
                FindObjectOfType<MainMenuAnimationEvent>().OnAnimationComplete();
            }
            Debug.Log("<color=red>Nik Log is the Tutorial : </color>" + listOfTutorialSprites.Count + " : " + currentImageIndex);
            this.gameObject.SetActive(false);
            Debug.Log("<color=red>Nik Log is the Tutorial Complete</color>");
        }
        if (isRight && currentImageIndex < listOfTutorialSprites.Count - 1)
        {
            currentImageIndex += 1;
            mainImage.sprite = listOfTutorialSprites[currentImageIndex];
        }
        else if (!isRight && currentImageIndex > 0)
        {
            currentImageIndex -= 1;
            mainImage.sprite = listOfTutorialSprites[currentImageIndex];
        }
    }
}
