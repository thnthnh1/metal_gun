using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NewCharaterUI : MonoBehaviour, ISimpleUI
{
    public Transform charBody;
    public int curWing;
    public int curSuit;
    public int curGlove;
    public int curKnife;
    public int curGun;
    public int curBoot;
    public ItemDat IDWing => S.Instance.wingsDat[curWing];
    public ItemDat IDSuit => S.Instance.suitDat[curSuit];
    public ItemDat IDGlove => S.Instance.glovesDat[curGlove];
    public ItemDat IDKnife => S.Instance.knifeDat[curKnife];
    public ItemDat IDGun => S.Instance.gunDat[curGun];
    public ItemDat IDBoot => S.Instance.bootsDat[curBoot];
    public ItemSingleBox boxWing;
    public ItemSingleBox boxGlove;
    public ItemSingleBox boxKnife;
    public ItemSingleBox boxSuit;
    public ItemSingleBox boxGun;
    public ItemSingleBox boxBoot;

    public HeroRightPanel heroRightPanel;
    public HeroLeftPanel heroLeftPanel;
    public NamingUI namingUI;

    private List<GameObject> items = new();
    private void Start()
    {
        foreach (Transform item in charBody)
        {
            items.Add(item.gameObject);
        }
    }
    private void OnEnable()
    {
        curWing = 0;
        curSuit = 0;
        curGlove = 0;
        curKnife = 0;
        curGun = 0;
        curBoot = 0;
        UpdateAll();
        heroLeftPanel.ApplyInfo();
        if (S.Instance.characterDat.namex == "john digger") namingUI.gameObject.SetActive(true);
    }
    public void ChangeWing()
    {
        IncreaseDat(ref curWing, S.Instance.wingsDat);
        UpdateAll(IDWing);
    }
    public void ChangeSuit()
    {
        IncreaseDat(ref curSuit, S.Instance.suitDat);
        UpdateAll(IDSuit);
    }
    public void ChangeGlove()
    {
        IncreaseDat(ref curGlove, S.Instance.glovesDat);
        UpdateAll(IDGlove);
    }
    public void ChangeKnife() 
    {
        IncreaseDat(ref curKnife, S.Instance.knifeDat);
        UpdateAll(IDKnife);
    }
    public void ChangeGun()
    { 
        IncreaseDat(ref curGun, S.Instance.gunDat);
        UpdateAll(IDGun);
    }
    public void ChangeBoot()
    {
        IncreaseDat(ref curBoot, S.Instance.bootsDat);
        UpdateAll(IDBoot);
    }
    public void IncreaseDat(ref int curDat, List<ItemDat> itemDats)
    {
        Debug.Log("Hit point");
        curDat += 1;
        if (curDat >= itemDats.Count) curDat = 0;
    }
    public void UpdateAll(ItemDat curSelect = null)
    {
        boxWing.Show(S.Instance.wingsDat[curWing]);
        boxSuit.Show(S.Instance.suitDat[curSuit]);
        boxGlove.Show(S.Instance.glovesDat[curGlove]);
        boxKnife.Show(S.Instance.knifeDat[curKnife]);
        boxGun.Show(S.Instance.gunDat[curGun]);
        boxBoot.Show(S.Instance.bootsDat[curBoot]);
        if (curSelect!=null)
        foreach (GameObject item in items)
        {
            if (item.name != "avartar") item.gameObject.SetActive(false);
            boxWing.ShowSelect(curSelect.itemID);
            boxSuit.ShowSelect(curSelect.itemID);
            boxGlove.ShowSelect(curSelect.itemID);
            boxKnife.ShowSelect(curSelect.itemID);
            boxGun.ShowSelect(curSelect.itemID);
            boxBoot.ShowSelect(curSelect.itemID);
        }
        charBody.Find(S.Instance.wingsDat[curWing].itemID).gameObject.SetActive(true);
        charBody.Find(S.Instance.suitDat[curSuit].itemID).gameObject.SetActive(true);
        charBody.Find(S.Instance.glovesDat[curGlove].itemID).gameObject.SetActive(true);
        charBody.Find(S.Instance.bootsDat[curBoot].itemID).gameObject.SetActive(true);
        charBody.Find(S.Instance.knifeDat[curKnife].itemID).gameObject.SetActive(true);
        charBody.Find(S.Instance.gunDat[curGun].itemID).gameObject.SetActive(true);
        heroRightPanel.Show(curSelect);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void UpdateName()
    {

    }
}
