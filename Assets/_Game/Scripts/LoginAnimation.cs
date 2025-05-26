using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoginAnimation : MonoBehaviour
{
    public Mask mask;

    public LoginLightEffect lightEffect;

    public GameObject touchToPlay;

    public Image spriteRenderer;
    public float waitTime = 0.07f;
    public Sprite[] cycle;
    public void ActiveLightEffect()
    {
        this.mask.enabled = true;
        this.lightEffect.gameObject.SetActive(true);
        this.touchToPlay.SetActive(true);
        Camera.main.DOShakePosition(0.5f, 0.5f, 10, 90f, true);
        SoundManager.Instance.PlayMusic("music_menu", 0f);

        StartCoroutine(LogoCycle());

    }

    IEnumerator LogoCycle()
    {
        int i;
        i = 0;
        while (i < cycle.Length)
        {
            spriteRenderer.sprite = cycle[i];
            i++;
            yield return new WaitForSeconds(waitTime);
            yield return 0;

        }
        StartCoroutine(LogoCycle());
    }

}
