using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeroRightPanel : MonoBehaviour
{
    public TextMeshProUGUI itemType;

    public ItemSingleBox itemSingleBox;
    public TextMeshProUGUI battlePower;
    public TextMeshProUGUI level;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI firedamage;
    public TextMeshProUGUI critdamage;
    public TextMeshProUGUI critrate;

    ItemDat _curDat;
    ItemData _curData;
    public void Show(ItemDat itemDat)
    {
        if (itemDat != null)
        {
            ItemData data = S.Instance.itemDatas.Find(x => x.itemID == itemDat.itemID);
            if (!data.isDefault)
            {
                gameObject.SetActive(true);
                ApplyData(itemDat, data);



            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void ApplyData(ItemDat dat, ItemData data)
    {
        _curDat = dat;
        _curData = data;
        itemType.text = dat.GetTypeText();
        itemSingleBox.Show(dat);
        level.text =$"{dat.level}";
        damage.text = $"{data.damageMin + dat.level * 3}/{data.damageMax + dat.level * 3}";
        firedamage.text = $"{data.fireRate}";
        critdamage.text = $"{data.critDamage}%";
        critrate.text = $"{data.critRate}%";
        battlePower.text = $"{data.damageMin + data.damageMax + 10 + dat.level*10}";
    }
    public void TouchUpdate()
    {
        _curDat.level += 1;
        ApplyData(_curDat, _curData);
    }
    public void Hide()
    {
    }

}
