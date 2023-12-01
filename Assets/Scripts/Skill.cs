using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Skill
{

    [SerializeField]
    private string name;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;


    [SerializeField]
    private GameObject skillPrefab;

    [SerializeField]
    private Color barColor;

    [SerializeField]
    private int manaCost;

    public string MyName { get => name; }

    public int MyDamage { get => damage;  }

    public Sprite MyIcon { get => icon;  }

    public float MySpeed { get => speed;  }

    public float MyCastTime { get => castTime;}

    public GameObject MySkillPrefab { get => skillPrefab; }

    public Color MyBarColor { get => barColor; }

    public int ManaCost { get => manaCost; set => manaCost = value; }
}
