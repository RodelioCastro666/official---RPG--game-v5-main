using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour,ICastable
{
    [SerializeField]
    private CraftingMaterial[] materials;

    [SerializeField]
    private Item output;

    [SerializeField]
    private int outputCount;

    [SerializeField]
    private string description;

    [SerializeField]
    private Image highLight;

    [SerializeField]
    private float craftTime;

    [SerializeField]
    private Color barColor;

    private Sprite icon;

    public Item OutPut { get => output;  }

    public int MyOutputCount { get => outputCount; set => outputCount = value; }

    public string MyDescription { get => description;  }

    public CraftingMaterial[] Materials { get => materials; }

    public string MyTitle  { get => output.MyTitle; }

    public Sprite MyIcon { get => output.MyIcon;  }

    public float MyCastTime { get => craftTime;  }

    public Color MyBarColor { get => barColor; }

    void Start()
    {
        GetComponent<Text>().text = output.MyTitle;
    }

   
   public void Select()
    {
        Color c = highLight.color;
        c.a = .3f;
        highLight.color = c;
    }

    public void Deselect()
    {
        Color c = highLight.color;
        c.a = 0f;
        highLight.color = c;
    }
}
