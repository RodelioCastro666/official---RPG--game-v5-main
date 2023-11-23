using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : BagScript
{
    // Start is called before the first frame update

    private void Awake()
    {
        AddSlots(12);
    }
}
