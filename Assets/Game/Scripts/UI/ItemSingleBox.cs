using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSingleBox : MonoBehaviour, IPointerDownHandler
{
    public Sprite[] framesSprite;
    public GameObject[] stars;
    public Image frame;
    public Image icon;
    public Image noffIcon;
    public bool isClickable;
    public UnityEvent onClick;

    private ItemDat curDat;
    bool isSellect = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public void Show(ItemDat itemDat)
    {
        ItemData data = S.Instance.itemDatas.Find(x => x.itemID == itemDat.itemID);
        curDat = itemDat;
        icon.sprite = data.iconSprite;
        foreach (var item in stars)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < data.stats; i++)
        {
            stars[i].SetActive(true);
        }
    }
    public void ShowSelect(string IDItem)
    {
        if (curDat.itemID == IDItem)
        {
            frame.sprite = framesSprite[1];
            noffIcon.gameObject.SetActive(true);
            isSellect = true;
        }
        else
        {
            frame.sprite = framesSprite[0];
            noffIcon.gameObject.SetActive(false);
            isSellect = false;
        }
    }
}
