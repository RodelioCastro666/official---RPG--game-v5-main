using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandScript : MonoBehaviour 
{
    [SerializeField]
    private Vector3 offset;

    public static HandScript instance;

    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }

    public IMovable MyMoveable { get; set; }

    private Image icon;

    void Start()
    {
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
                
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPositin = Camera.main.ScreenToWorldPoint(touch.position);
            icon.transform.position = touchPositin;
        }

        icon.transform.position = Input.mousePosition;
    }

    public void TakeMoveable(IMovable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }

    public IMovable Put()
    {
        IMovable tmp = MyMoveable;
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
        return tmp;
    }

    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
        InventoryScripts.MyInstance.FromSlot = null;
        
    }

    public void DeleteItem()
    {


        if (MyMoveable is Item && InventoryScripts.MyInstance.FromSlot != null)
        {
            (MyMoveable as Item).MySlot.Clear();

        }

        Drop();

        InventoryScripts.MyInstance.FromSlot = null;







    }

   
}