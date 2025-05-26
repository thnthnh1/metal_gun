using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string namex;
    public ItemType itemType;
    public Sprite iconSprite;
    public bool isDefault;
    [Header("Data")]
    public int stats;

    public int damageMin;
    public int damageMax;
    public int fireRate;
    public int critDamage;
    public int critRate;
}
public enum ItemType
{
    Wings,
    Gloves,
    Boots,
    Suit,
    Knife,
    Gun
}


public class ItemDat
{
    public string itemID;
    public int level;

    public ItemDat(string itemID, int level)
    {
        this.itemID = itemID;
        this.level = level;
    }

    public string GetTypeText()
    {
        switch (S.Instance.itemDatas.Find(x=>x.itemID == itemID).itemType)
        {
            case ItemType.Wings:
                return "Wings";
            case ItemType.Gloves:
                return "Gloves";
            case ItemType.Boots:
                return "Boots";
            case ItemType.Suit:
                return "Suit";
            case ItemType.Knife:
                return "Knife";
            case ItemType.Gun:
                return "Gun";
            default:
                return "";
        }
    }

}