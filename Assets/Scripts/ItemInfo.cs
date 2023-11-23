using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text stack;

    private int count;

    public Item MyItem { get => item; set => item = value; }

    public void Initialize(Item item, int count)
    {
        this.MyItem = item;
        this.image.sprite = item.MyIcon;
        this.title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[item.MyQuality], item.MyTitle);
        this.count = count;

        if(count > 1)
        {
            stack.enabled = true;
            stack.text = InventoryScripts.MyInstance.GetItemCount(item.MyTitle).ToString() + "/" + count.ToString();
        }
    }

    public void UpdateStackCount()
    {
        stack.text = InventoryScripts.MyInstance.GetItemCount(MyItem.MyTitle) + "/" + count.ToString();
    }

    

    public void OnPointerUp(PointerEventData eventData)
    {
        UiManager.MyInstance.HideToolTip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UiManager.MyInstance.ShowToolTip(MyItem);
    }
}
