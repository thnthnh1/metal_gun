using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPreviewFlame : BaseBulletPreview
{
    public float timeApplyDamage = 0.3f;

    public Collider2D col;

    public GameObject subFlame1;

    public GameObject subFlame2;

    private bool isActive;

    private List<Transform> victims = new List<Transform>();

    private float lastTimeDealDamage;


    protected override void Update()
    {
        float time = Time.time;
        float num = time - this.lastTimeDealDamage;
        if (num > this.timeApplyDamage)
        {
            this.lastTimeDealDamage = time;
            this.DealDamage();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy") && !this.victims.Contains(other.transform))
        {
            this.victims.Add(other.transform);
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy") && this.victims.Contains(other.transform))
        {
            this.victims.Remove(other.transform);
        }
    }

    private void DealDamage()
    {
        if (this.victims.Count <= 0)
        {
            return;
        }
        EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
    }

    public void ActiveSubFlame1()
    {
        if (!this.subFlame1.activeSelf)
        {
            this.subFlame1.SetActive(true);
        }
    }

    public void ActiveSubFlame2()
    {
        if (!this.subFlame2.activeSelf)
        {
            this.subFlame2.SetActive(true);
        }
    }


}
