using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IDragHandler,IDropHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text StackSize;

    public int MyIndex { get; set; }

    public BagScript MyBag { get; set; }

    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }
            return null;
        }
    }

    public Image MyIcon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }

    }

    public bool Isfull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }
            return true;
        }
    }

    public int MyCount
    {
        get { return MyItems.Count; }
    }

    public Text MyStackText => StackSize;

    public ObservableStack<Item> MyItems { get => items; }

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
         
        if (eventData.button == PointerEventData.InputButton.Left && HandScript.MyInstance.MyMoveable == null)
        {
            UseItem();
            Debug.Log("click");
        }
       
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScripts.MyInstance.OnItemCountChanged(MyItems.Pop());

        }
    }

    public void UseItem()
    {
        if (MyItem is IUsable)
        {
            (MyItem as IUsable).Use();
        }
        else if (MyItem is Armor)
        {
            (MyItem as Armor).Equip();
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }
    private void UpdateSlot()
    {
        UiManager.MyInstance.UpdateStackSize(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InventoryScripts.MyInstance.FromSlot == null && !IsEmpty)
        {
            UiManager.MyInstance.ShowToolTip(MyItem);
        }

        if (eventData.button == PointerEventData.InputButton.Left && HandScript.MyInstance.MyMoveable == null)
        {
            if (InventoryScripts.MyInstance.FromSlot == null && !IsEmpty)
            {
                if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMovable);
                    InventoryScripts.MyInstance.FromSlot = this;
                    
                }

                HandScript.MyInstance.TakeMoveable(MyItem as IMovable);
                InventoryScripts.MyInstance.FromSlot = this;
                
            }

        }
    }


    public void OnDrop(PointerEventData eventData)
    {
         UiManager.MyInstance.HideToolTip();

        if (InventoryScripts.MyInstance.FromSlot != null)
        {
            if (PutItemBack() || Mergeitems(InventoryScripts.MyInstance.FromSlot) || Swapitems(InventoryScripts.MyInstance.FromSlot) || AddItems(InventoryScripts.MyInstance.FromSlot.MyItems))
            {
                HandScript.MyInstance.Drop();
                InventoryScripts.MyInstance.FromSlot = null;
                
                
            }

            
        }
        if (InventoryScripts.MyInstance.FromSlot == null && IsEmpty)
        {
            if(HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor armor = (Armor)HandScript.MyInstance.MyMoveable;
                
                CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
                AddItem(armor);
                HandScript.MyInstance.Drop();

            }
        }

        if(InventoryScripts.MyInstance.FromSlot == null && !IsEmpty)
        {
            if(HandScript.MyInstance.MyMoveable != null)
            {
                if(HandScript.MyInstance.MyMoveable is Armor)
                {
                    if(MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.MyInstance.MyMoveable as Armor).MyArmorType)
                    {
                        (MyItem as Armor).Equip();
                       // UiManager.MyInstance.RefreshToolTip();
                        HandScript.MyInstance.Drop();
                    }
                }
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        UiManager.MyInstance.HideToolTip();

        if (!EventSystem.current.IsPointerOverGameObject() && HandScript.MyInstance.MyMoveable != null)
        {
            if (HandScript.MyInstance.MyMoveable is Item && InventoryScripts.MyInstance.FromSlot != null)
            {
                (HandScript.MyInstance.MyMoveable as Item).MySlot.Clear();

            }


            Debug.Log("Endrag");

            HandScript.MyInstance.Drop();
            InventoryScripts.MyInstance.FromSlot = null;
        }


        PutItemBack();
        HandScript.MyInstance.Drop();

    }
    

    private bool PutItemBack()
    {
        if (InventoryScripts.MyInstance.FromSlot == this)
        {
            InventoryScripts.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }
        return false;
    }





    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (Isfull)
                {
                    return false;
                }
                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }

    private bool Swapitems(SlotScript from)
    {
        if (IsEmpty) 
        {
            return false;
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            ObservableStack<Item> tmpfrom = new ObservableStack<Item>(from.MyItems);

            from.MyItems.Clear();
            from.AddItems(MyItems);
            MyItems.Clear();
            AddItems(tmpfrom);

            return true;
        }

        return false;
    }

    private bool Mergeitems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() == MyItem.GetType() && !Isfull)
        {
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }

            return true;
        }

        return false;
    }

    public void Clear()
    {
        int intCount = MyItems.Count;


        if (intCount > 0)
        {
            for (int i = 0; i < intCount; i++)
            {
                InventoryScripts.MyInstance.OnItemCountChanged(MyItems.Pop());
            }
        }




    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       

        Debug.Log("exit");
        
    }


}

    
  
