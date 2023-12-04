using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ArmorType { Helmet, Shoulders, Chest, Gloves, Boots, Sword, Orb, Necklace, Ring, Belt}

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]

public class Armor : Item
{
    [SerializeField]
    private ArmorType armorType;

    [SerializeField]
    private int intellect;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int vitality;

    internal ArmorType MyArmorType { get => armorType; set => armorType = value; }

    public int Intellect { get => intellect; set => intellect = value; }


    public int Vitality { get => vitality; set => vitality = value; }

    public int Strength { get => strength; set => strength = value; }

    public override string GetDescription()
    {
        string stats = string.Empty;

        if (Intellect > 0)
        {
            stats += string.Format("\n +{0} intellect", Intellect);
        }
        if (Strength > 0)
        {
            stats += string.Format("\n +{0} strength", Strength);
        }
        if (Vitality> 0)
        {
            stats += string.Format("\n +{0} stamina", Vitality);
        }
       
        return base.GetDescription() + stats;
    }

    public void Equip()
    {
        CharacterPanel.MyInstance.EquipArmor(this);
    }
}
