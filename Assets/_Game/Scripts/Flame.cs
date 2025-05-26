using System;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public GunFlame gun;

    public AudioClip soundFire;

    public AudioSource aud;

    public Collider2D col;

    public GameObject subFlame1;

    public GameObject subFlame2;

    private bool isActive;

    private List<BaseUnit> victims = new List<BaseUnit>();

    private float timeApplyDamage = 0.3f;

    private float timerDealDamage;

    protected void Awake()
    {
        this.aud.loop = true;
        this.aud.clip = this.soundFire;
      
        // this.timeApplyDamage = ((SO_GunFlameStats)this.gun.baseStats).TimeApplyDamage; // original

    }
    private void Start()
    {
        this.timeApplyDamage = ((SO_GunFlameStats)this.gun.baseStats).TimeApplyDamage; // original

    }


    protected void Update()
    {
        if (this.isActive)
        {
            this.timerDealDamage += Time.deltaTime;
            Debug.Log("Timer " + this.timerDealDamage);
            Debug.Log("Timer Max " + this.timeApplyDamage);
            if (this.timerDealDamage >= 0.1f) // this.timeApplyDamage
            {
                Debug.Log("Apply Damage");
                this.timerDealDamage = 0f;
                this.DealDamage();
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        BaseUnit baseUnit = null;
        if (other.CompareTag("Enemy Body Part") || other.CompareTag("Destructible Obstacle"))
        {
            baseUnit = Singleton<GameController>.Instance.GetUnit(other.gameObject);
        }
        else if (other.transform.root.CompareTag("Enemy"))
        {
            baseUnit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
        }
        if (baseUnit != null && !this.victims.Contains(baseUnit))
        {
            this.victims.Add(baseUnit);
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        BaseUnit baseUnit = null;
        if (other.CompareTag("Enemy Body Part") || other.CompareTag("Destructible Obstacle"))
        {
            baseUnit = Singleton<GameController>.Instance.GetUnit(other.gameObject);
        }
        else if (other.transform.root.CompareTag("Enemy"))
        {
            baseUnit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
        }
        if (baseUnit != null && this.victims.Contains(baseUnit))
        {
            this.victims.Remove(baseUnit);
        }
    }

    public void Active()
    {
        base.gameObject.SetActive(true);
        this.isActive = true;
        this.aud.Play();
        //this.timerDealDamage = 0f;
    }

    public void Deactive()
    {
        this.victims.Clear();
        this.isActive = false;
        this.aud.Stop();
        base.gameObject.SetActive(false);
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

    private void DealDamage()
    {
        Debug.Log("Victims Count " + this.victims.Count);
        if (this.victims.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < this.victims.Count; i++)
        {
            AttackData curentAttackData = ((Rambo)this.gun.shooter).GetCurentAttackData();
            this.victims[i].TakeDamage(curentAttackData);
        }
        this.gun.ConsumeAmmo(1);
    }
}
