using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop 
{
    public Item MyItem { get; set; }

    public LootTable MyLooTable { get; set; }

    public Drop(Item item, LootTable lootTable)
    {
        MyLooTable = lootTable;
        MyItem = item;
    }

    public void Remove()
    {
        MyLooTable.MyDroppedItems.Remove(this);
    }

    
}
