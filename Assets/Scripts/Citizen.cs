using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : Npc
{
    [SerializeField]
    private Dialogue dialogue;

    public override void Interact()
    {
        base.Interact();
        DialogueWindow.MyInstance.SetDialogue(dialogue);
    }
}
