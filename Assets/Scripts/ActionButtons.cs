using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class ActionButtons : MonoBehaviour,  IPointerClickHandler, IClickable,IDropHandler
{
    public IUsable MyUseable { get; set; }
    
    public Button MyButton { get; private set; }

    private Stack<IUsable> useables = new Stack<IUsable>();

    [SerializeField]
    private Text stackSize;

    private int count;

    [SerializeField]
    private Image icon;

    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount
    {
        get
        {
            return count;
        }
    }

    public Text MyStackText
    {
        get { return stackSize; }
    }

    public Stack<IUsable> MyUseables 
    {
        get
        {
            return useables;
        }
        set
        {
            if (value.Count > 0)
            {
                MyUseable = value.Peek();
            }
            else
            {
                MyUseable = null;
            }

            
            useables = value;
        }   
            
        
    }

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
        InventoryScripts.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (HandScript.MyInstance.MyMoveable == null)
        {

            if (MyUseable != null)
            {
                MyUseable.Use();
            }
            else if(MyUseables != null && MyUseables.Count > 0)
            {
                MyUseables.Peek().Use();
            }
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUsable)
        //    {
        //        SetUseable(HandScript.MyInstance.MyMoveable as IUsable);
        //    }
        //}
    }

    public void SetUseable(IUsable useable)
    {
        if (useable is Item)
        {
            MyUseables = InventoryScripts.MyInstance.GetUsables(useable);
            if (InventoryScripts.MyInstance.FromSlot != null)
            {

                InventoryScripts.MyInstance.FromSlot.MyIcon.color = Color.white;
                InventoryScripts.MyInstance.FromSlot = null;
            }

          

           
        }
        else
        {
            MyUseables.Clear();
            this.MyUseable = useable;
        }

        count = MyUseables.Count;
        UpodateVisual(useable as IMovable);
       
    }

    public void UpodateVisual(IMovable moveable)
    {
        

        if(HandScript.MyInstance.MyMoveable != null)
        {
            HandScript.MyInstance.Drop();
        }

        MyIcon.sprite = moveable.MyIcon;
        MyIcon.color = Color.white;

        if (count > 1)
        {
            UiManager.MyInstance.UpdateStackSize(this);
        }
        else if(MyUseable is Spell)
        {
            UiManager.MyInstance.ClearStackCount(this);
        }

        
    }

    public void UpdateItemCount(Item item)
    {
        if(item is IUsable && MyUseables.Count > 0)
        {
            if (MyUseables.Peek().GetType() == item.GetType())
            {
                MyUseables = InventoryScripts.MyInstance.GetUsables(item as IUsable);

                count = MyUseables.Count;

                UiManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUsable)
            {
                SetUseable(HandScript.MyInstance.MyMoveable as IUsable);
            }
        }
    }
}
