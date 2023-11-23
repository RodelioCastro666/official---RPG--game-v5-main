
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Apple", menuName = "Items/Apple", order = 2)]
public class Apple : Item
{
  
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n Use: Wala lang ");
    }
}
