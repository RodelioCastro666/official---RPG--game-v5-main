using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{

    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    [SerializeField]
    private KillObjective[] killObjectives;

    [SerializeField]
    private int xp;

    [SerializeField]
    private int level;

    public QuestScript MyQuestScript { get; set; }

    public QuestGiver MyQuestGiver { get; set; }

    public string MyTitle { get => title; set => title = value; }


    public string MyDescription { get => description; set => description = value; }

    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }

    public bool IsComplete
    {
        get
        {
            foreach (Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                  
                }
            }
            foreach (Objective o in MyKillObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                   
                }
            }

            return true;
        }
    }

    public KillObjective[] MyKillObjectives { get => killObjectives; set => killObjectives = value; }

    public int MyXp { get => xp; set => xp = value; }

    public int MyLevel { get => level; set => level = value; }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;


    [SerializeField]
    private string type;

    private int currentAmount;


    public int MyAmount { get => amount; }

    public int MyCurrenAmount { get => currentAmount; set => currentAmount = value; }

    public string MyType { get => type; }

    public bool IsComplete
    {
        get
        {
            return MyCurrenAmount >= MyAmount;
            

        }
    }
}

[System.Serializable]
public class CollectObjective : Objective 
{ 
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrenAmount = InventoryScripts.MyInstance.GetItemCount(item.MyTitle);

            if (MyCurrenAmount <= MyAmount)
            {
                MessageFeedManager.MyInstace.WriteMessage(string.Format("{0}: {1}/{2}", item.MyTitle, MyCurrenAmount, MyAmount));
            }

           
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }
    public void UpdateItemCount()
    { 
        MyCurrenAmount = InventoryScripts.MyInstance.GetItemCount(MyType);
        QuestLog.MyInstance.UpdateSelected();
        QuestLog.MyInstance.CheckCompletion();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScripts.MyInstance.GetItems(MyType, MyAmount);

        foreach(Item item in items)
        {
            item.Remove();
        }
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKilleCount(Character character)
    {
        if (MyType == character.MyType)
        {
            if (MyCurrenAmount < MyAmount)
            {
                MyCurrenAmount++;
                MessageFeedManager.MyInstace.WriteMessage(string.Format("{0}: {1}/{2}", MyType, MyCurrenAmount, MyAmount));
                QuestLog.MyInstance.UpdateSelected();
                QuestLog.MyInstance.CheckCompletion();
            }

          
        }
    }
}


