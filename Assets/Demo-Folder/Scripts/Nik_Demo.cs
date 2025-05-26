using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class Nik_Demo : MonoBehaviour
{
    public static Nik_Demo instance;
    public bool isweaponMove = false;
    public bool isrun = false;
    public bool isReload = false;
    public bool isWeaponReload = false;
    public bool isJetpack = true;
    public bool isGrenade = false;
    public bool Skip = false;

    public List<GameObject> GO = new List<GameObject>();

    public GameObject fireG;
    public GameObject ExitBTN;
    public GameObject SkipBTN;
    public GameObject WeaponItem;
    public GameObject WItem;
    public GameObject GItem;
    public GameObject RunItem;
    public GameObject Arrow;

    public CnControls.SimpleJoystick MoveJS;
    public CnControls.SimpleJoystick WeaponJS;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        for (int i = 0; i < GO.Count; i++)
        {
            GO[i].SetActive(false);
        }
    }

    private int minutes;
	private int seconds;
    IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(1);
        Mp_GameController.instance.gameTime--;

        minutes = TimeSpan.FromSeconds(Mp_GameController.instance.gameTime).Minutes;
		seconds = TimeSpan.FromSeconds(Mp_GameController.instance.gameTime).Seconds;

		Mp_GameController.instance.gameTimeText.text = minutes + " : " + seconds;

        if (Mp_GameController.instance.gameTime == 0)
        {
            StopAllCoroutines();
            StopCoroutine(DelayTimer());
            ExitDemo();
            yield return null;
        }

        StartCoroutine(DelayTimer());
    }

    public void Start_Demo()
    {
        // StartCoroutine(DelayStart());
    }

    public void StartTutorialStep1()
    {
        if (Skip)
        {
            return;
        }
        StartCoroutine(DelayTimer());
        GO[0].SetActive(true);
        SkipBTN.SetActive(true);
    }
    public void StartTutorialStep2()
    {
        if (Skip)
        {
            return;
        }
        isweaponMove = true;
        GO[1].SetActive(true);
        GO[0].SetActive(false);
        Debug.Log("1 to the Tutorial Demo");
    }
    
    public void StartTutorialStep3()
    {
        if (Skip)
        {
            return;
        }
        isrun = true;
        GO[2].SetActive(true);
        GO[1].SetActive(false);
        Debug.Log("2 to the Tutorial Demo");
    }
    public void StartTutorialStep4()
    {
        if (Skip)
        {
            return;
        }
        isJetpack = true;
        // Scene 4
        GO[3].SetActive(true);
        GO[2].SetActive(false);
        Debug.Log("3 to the Tutorial Demo");
    }
    public void StartTutorialStep5()
    {
        if (Skip)
        {
            return;
        }
        // Scene 5
        GO[4].SetActive(true);
        GO[3].SetActive(false);
        Debug.Log("4 to the Tutorial Demo");

        
    }
    public void StartTutorialStep6()
    {
        if (Skip)
        {
            return;
        }
        isGrenade = false;
       // Scene 6
        GO[5].SetActive(true);
        GO[4].SetActive(false);
        Debug.Log("5 to the Tutorial Demo");

        
    }
    public void StartTutorialStep7()
    {
        if (Skip)
        {
            return;
        }
        isWeaponReload = true;
        // Scene 7
        GO[6].SetActive(true);
        GO[5].SetActive(false);
        Debug.Log("6 to the Tutorial Demo");
    }

    public void StartTutorialStep8()
    {
        if (Skip)
        {
            return;
        }
        isReload = true;
        // Scene 8
        GO[7].SetActive(true);
        GO[6].SetActive(false);
        Debug.Log("7 to the Tutorial Demo");
    }
    public void StartTutorialStep9()
    {
        // Scene 9
        if (Skip)
        {
            return;
        }
        GO[8].SetActive(true);
        GO[7].SetActive(false);
        ExitBTN.SetActive(true);
        Debug.Log("8 to the Tutorial Demo");
    }
    public void StartTutorialStep10()
    {
        if (Skip)
        {
            return;
        }
        // Scene 9
        GO[8].SetActive(false);
        SkipBTN.SetActive(false);
        // GO[7].SetActive(false);
        Debug.Log("8 to the Tutorial Demo");
    }

    IEnumerator DelayStart()
    {
        Debug.Log("Enter to the Tutorial Demo");
        // Scene 1
        GO[0].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GO[0].SetActive(false);
        Debug.Log("0 to the Tutorial Demo");

        // Scene 2
        GO[1].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GO[1].SetActive(false);
        Debug.Log("1 to the Tutorial Demo");
        
        // Scene 3
        GO[2].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GO[2].SetActive(false);
        Debug.Log("2 to the Tutorial Demo");

        // Scene 4
        GO[3].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GO[3].SetActive(false);
        Debug.Log("3 to the Tutorial Demo");

        // Scene 5
        GO[4].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GO[4].SetActive(false);
        Debug.Log("4 to the Tutorial Demo");

        // Scene 6
        GO[5].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GO[5].SetActive(false);
        Debug.Log("5 to the Tutorial Demo");

        // Scene 7
        GO[6].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GO[6].SetActive(false);
        Debug.Log("6 to the Tutorial Demo");

        // Scene 8
        GO[7].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GO[7].SetActive(false);
        Debug.Log("7 to the Tutorial Demo");

        // Scene 9
        GO[8].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        // GO[8].SetActive(false);
        Debug.Log("8 to the Tutorial Demo");

        // Scene 10
        // GO[9].SetActive(true);
        // yield return new WaitForSeconds(0.5f);
        // GO[9].SetActive(false);

        Debug.Log("Exit to the Tutorial Demo");
    }

    public void ExitDemo()
    {
        if (PlayerPrefs.GetInt("Tutotial") == 0)
        {
            PlayerPrefs.SetInt("Tutotial", 1);
            PlayerPrefs.SetInt("AfterTutotial", 0);
            PlayerPrefs.Save();
        }
        
        SceneManager.LoadScene("Menu");
    }

    public void SkipBTN_Click()
    {
        Skip = true;
        isweaponMove = true;
        isrun = true;
        isJetpack = true;
        isGrenade = false;
        isWeaponReload = true;
        isReload = true;
        for (int i = 0; i < GO.Count; i++)
        {
            GO[i].SetActive(false);
        }
        ExitBTN.SetActive(true);
        SkipBTN.SetActive(false);
    }

}
