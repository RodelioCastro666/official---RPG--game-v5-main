 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagButton : MonoBehaviour,  IPointerClickHandler, IDragHandler, IEndDragHandler

{
    

    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    [SerializeField]
    private int bagIndex;

    public Bag MyBag 
    { 
        get 
        {
            return bag;
        }  
        set 
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
     }

    public int MyBagIndex { get => bagIndex; set => bagIndex = value; }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(InventoryScripts.MyInstance.FromSlot != null && HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
            {
                if(MyBag != null)
                {
                    InventoryScripts.MyInstance.Swapbags(MyBag, HandScript.MyInstance.MyMoveable as Bag);
                }
                else
                {
                    Bag tmp = (Bag)HandScript.MyInstance.MyMoveable;
                    tmp.MyBagButton = this;
                    tmp.Use();
                    MyBag = tmp;
                    HandScript.MyInstance.Drop();
                    InventoryScripts.MyInstance.FromSlot = null;
                }
            }
            
                HandScript.MyInstance.TakeMoveable(MyBag);
            

          
        }

       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandScript.MyInstance.Drop();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
        if (bag != null)
        {
            bag.MyBagScript.OpenClose();
        }
    }

    public void RemoveBag()
    {
        InventoryScripts.MyInstance.RemoveBag(MyBag);
        MyBag.MyBagButton = null;


        foreach (Item item in MyBag.MyBagScript.GetItems())
        {
            InventoryScripts.MyInstance.AddItem(item);

        }
        MyBag = null;
    }

  
}
