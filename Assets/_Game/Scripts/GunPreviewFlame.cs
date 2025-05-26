using System;
using System.Collections;
using UnityEngine;

public class GunPreviewFlame : BaseGunPreview
{
    public BulletPreviewFlame flame;
    public float chargeRate = 2f; // 
    IEnumerator onCoroutine()
    {
        while (true)
        {
            flame.gameObject.SetActive(true);
            yield return new WaitForSeconds(chargeRate);
            flame.gameObject.SetActive(false);
            yield return new WaitForSeconds(chargeRate);
        }
    }
    private void OnEnable()
    {
        StartCoroutine (onCoroutine()); 
    }

    private void OnDisable()
    {
        StopCoroutine (onCoroutine()); 
    }
}
