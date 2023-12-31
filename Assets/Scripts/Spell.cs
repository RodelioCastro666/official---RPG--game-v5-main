﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Spell : IUsable, IMovable, ICastable
{

    [SerializeField]
    private string title;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;
    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private Color barColor;

    public string MyTitle { get => title; }

    public int MyDamage { get => damage; }

    public Sprite MyIcon { get => icon; }

    public float MySpeed { get => speed; }

    public float MyCastTime { get => castTime; }

    public GameObject MySpellPrefab { get => spellPrefab; }

    public Color MyBarColor { get => barColor; }
    public bool OnCooldown { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float MyCoolDown { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Use()
    {
        Player.MyInstance.CastSpell(this);
    }
}

