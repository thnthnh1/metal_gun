using ET.Saveload;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S : Singleton<S>
{
    public List<ItemData> itemDatas;
    public List<ItemDat> wingsDat = new();
    public List<ItemDat> glovesDat = new();
    public List<ItemDat> knifeDat = new();
    public List<ItemDat> suitDat = new();
    public List<ItemDat> gunDat = new();
    public List<ItemDat> bootsDat = new();
    public CharacterDat characterDat = new();
    public List<string> giftCodes = new();
    public List<int> giftCodesHeroesIndex = new();
    public List<string> ramboName = new();
    public void Awake()
    {
        DontDestroyOnLoad(this);
        Load();
        foreach (var item in itemDatas)
        {
            ItemDat newDat = new(item.itemID,0);
            switch (item.itemType)
            {
                case ItemType.Wings:
                    wingsDat.Add(newDat);
                    break;
                case ItemType.Gloves:
                    glovesDat.Add(newDat);
                    break;
                case ItemType.Boots:
                    bootsDat.Add(newDat);
                    break;
                case ItemType.Suit:
                    suitDat.Add(newDat);
                    break;
                case ItemType.Knife:
                    knifeDat.Add(newDat);
                    break;
                case ItemType.Gun:
                    gunDat.Add(newDat);
                    break;
            }
        }
        Debug.Log("wingsDat count " + wingsDat.Count);
        Debug.Log("glovesDat count " + glovesDat.Count);
        Debug.Log("bootsDat count " + bootsDat.Count);
        Debug.Log("suitDat count " + suitDat.Count);
        Debug.Log("knifeDat count " + knifeDat.Count);
        Debug.Log("gunDat count " + gunDat.Count);
        RebuildData();
    }
    void RebuildData()
    {

        if (characterDat == null)
        {
            characterDat = new();
            if (string.IsNullOrEmpty(characterDat.namex)) characterDat.namex = "john digger";
            characterDat.ownedRambo = new() { 0 };
            Save();
        }
    }
    public void Save()
    {
        SaveLoadManager.Instance.Save(characterDat);
    }
    public void Load()
    {
        SaveLoadManager.Instance.Load(ref characterDat);

    }
    public void CleanData()
    {
        SaveLoadManager.Instance.CleanData();
        characterDat = null;
        RebuildData();
    }
    public bool IsShowGiftCode()
    {
        bool isTrue = true;
        foreach (var item in giftCodesHeroesIndex)
        {
            if (characterDat.ownedRambo.Contains(item))
            {
                isTrue = false; break;
            }
        }
        return isTrue;  
    }

}
