using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questParent;

    [SerializeField]
    private Text questDescription;

    private Quest selected;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text questCountText;

    private int currentCount;

    [SerializeField]
    private int maxCount;

    private List<QuestScript> questScripts = new List<QuestScript>();

    private List<Quest> quests = new List<Quest>();

    private static QuestLog instance;

    public static QuestLog MyInstance 
    { 
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }

            return instance;

        }
        
    }

    public List<Quest> MyQuest { get => quests; set => quests = value; }

    private void Start()
    {
       
    }

   

    public void AcceptQuest(Quest quest)
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            questCountText.text = currentCount + "/" + maxCount;
            foreach (CollectObjective o in quest.MyCollectObjectives)
            {
                InventoryScripts.MyInstance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);
                o.UpdateItemCount();
            }
            foreach (KillObjective o in quest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmedEvent += new KillConfirmed(o.UpdateKilleCount);
            }

            MyQuest.Add(quest);
            GameObject go = Instantiate(questPrefab, questParent);

            QuestScript qs = go.GetComponent<QuestScript>();
            quest.MyQuestScript = qs;
            qs.MyQuest = quest;
            questScripts.Add(qs);
            go.GetComponent<Text>().text = quest.MyTitle;

            CheckCompletion();
        }

        

    }
    
    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.Deselect();
            }

            string objectives = string.Empty;

            selected = quest;

            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrenAmount + "/" + obj.MyAmount + "\n";
            }
            foreach (Objective obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrenAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("{0} \n<size=10>{1}</size>\nObjectives\n<size=10>{2}</size>", title, quest.MyDescription, objectives);
        }

        
    }

    public void CheckCompletion()
    {
        
        foreach (QuestScript qs in questScripts)
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
            qs.IsComplete();
           
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }


    }

    public void AbandonQuest()
    {
        foreach (KillObjective o in selected.MyKillObjectives)
        {
            GameManager.MyInstance.killConfirmedEvent -= new KillConfirmed(o.UpdateKilleCount);
        }
        foreach (CollectObjective o in selected.MyCollectObjectives)
        {
            InventoryScripts.MyInstance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
           
        }
       

        RemoveQuest(selected.MyQuestScript);


    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        MyQuest.Remove(qs.MyQuest);
        questDescription.text = string.Empty;
        selected = null;
        currentCount--;
        questCountText.text = currentCount + "/" + maxCount;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null; 
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public bool HasQuest(Quest quest)
    {
        return MyQuest.Exists(x => x.MyTitle == quest.MyTitle);
    }
}
