using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName ="Items/Bag", order =1)]
public class Bag : Item, IUsable
{

    [SerializeField]
    private int slots;

    [SerializeField]
    private GameObject bagPrefab;

    public BagScript MyBagScript { get; set; }

    public BagButton MyBagButton { get; set; }

   // public int MySlotCount { get => Slots;  }

    public int MySlotCount { get => slots;  } // change from "Slot"

    public void Initialized(int slots)
    {
        this.slots = slots;
    }

    public void Use()
    {
        if (InventoryScripts.MyInstance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScripts.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(MySlotCount);

          //  InventoryScripts.MyInstance.AddBag(this);

            if (MyBagButton == null)
            {
                InventoryScripts.MyInstance.AddBag(this);
            }
            else
            {
                InventoryScripts.MyInstance.AddBag(this, MyBagButton);
            }

             MyBagScript.MyBagIndex = MyBagButton.MyBagIndex;
        }

      
    }

    public void SetUpScript()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScripts.MyInstance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots); // change from MySlotCount
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n{0} slot bag", MySlotCount); 
    }


}
