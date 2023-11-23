using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{

    private static QuestGiverWindow instance;


    public static QuestGiverWindow MyInstance 
    { 
        get
        {
            if (instance ==  null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }
            return instance;
        }
        
    }

    [SerializeField]
    private GameObject backbtn, acceptbtn, questDescription, completebtn;

    private QuestGiver questGiver;

    [SerializeField]
    private Transform questArea;

    [SerializeField]
    private GameObject questPrefab;

    private Quest selectedQuest;

    private List<GameObject> quests = new List<GameObject>();

    public void ShowQuest(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach (GameObject go in quests)
        {
            Destroy(go);
        }


        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (Quest quest in questGiver.MyQuests)
        {
            if (quest != null)
            {
                GameObject go = Instantiate(questPrefab, questArea);

                go.GetComponent<Text>().text = "["+ quest.MyLevel +"] " +quest.MyTitle;

                go.GetComponent<QGQuestScripts>().MyQuest = quest;

                quests.Add(go);

                if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    go.GetComponent<Text>().text += "(C)";
                }

                else if (QuestLog.MyInstance.HasQuest(quest))
                {
                    Color c = go.GetComponent<Text>().color;
                    c.a = 0.5f;

                    go.GetComponent<Text>().color = c;
                }
            }

           
        }
    }

    public override void Open(Npc npc)
    {
        ShowQuest((npc as QuestGiver));
        base.Open(npc);
    }

    public void ShowQuestInfo(Quest quest)
    {
        this.selectedQuest = quest;

        if (QuestLog.MyInstance.HasQuest(quest) && quest.IsComplete)
        {
            acceptbtn.SetActive(false);
            completebtn.SetActive(true);
        }
        else if (!QuestLog.MyInstance.HasQuest(quest))
        {
            acceptbtn.SetActive(true);
        }

        backbtn.SetActive(true);
        
        
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);



        string objectives = string.Empty;
        


        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrenAmount + "/" + obj.MyAmount + "\n";
        }

        questDescription.GetComponent<Text>().text = string.Format("{0} \n<size=10>{1}</size>\n\nObjectives\n<size=10>{2}</size>", quest.MyTitle, quest.MyDescription, objectives);
    }

    public void Back()
    {
        backbtn.SetActive(false);
        acceptbtn.SetActive(false);
        ShowQuest(questGiver);
        completebtn.SetActive(false);
    }
    public void Accept()
    {
        QuestLog.MyInstance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void Close()
    {
        completebtn.SetActive(false);
        base.Close();
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if (selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyCompletedQuest.Add(selectedQuest.MyTitle);
                    questGiver.MyQuests[i] = null;
                    selectedQuest.MyQuestGiver.UpdateQuestStatus();
                }
            }

            foreach (CollectObjective o in selectedQuest.MyCollectObjectives)
            {
                InventoryScripts.MyInstance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
                o.Complete(); 
            }
            foreach (KillObjective o in selectedQuest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmedEvent -= new KillConfirmed(o.UpdateKilleCount);
            }
            Player.MyInstance.GainXP(Xpmanager.CalculateXP(selectedQuest));

            QuestLog.MyInstance.RemoveQuest(selectedQuest.MyQuestScript);

            Back();
        }
    }
}
