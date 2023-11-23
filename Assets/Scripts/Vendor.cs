using UnityEngine;

public class Vendor : Npc, IInteractable
{
    [SerializeField]
    private VendorItem[] items;

    public VendorItem[] MyItems { get => items; }
}
