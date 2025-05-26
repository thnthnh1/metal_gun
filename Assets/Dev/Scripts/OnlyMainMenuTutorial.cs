using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyMainMenuTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.GetInt("Tutotial") == 0)
        {
            Mp_Armory.instance.Tutorial.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("AfterTutotial") == 0)
        {
            Mp_Armory.instance.AfterTutorial[0].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
