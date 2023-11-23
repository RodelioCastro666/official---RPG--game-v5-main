using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Npc
{
    [SerializeField]
    private Quest[] quests;

    [SerializeField]
    private Sprite question, questionSilver, exclamation;

    [SerializeField]
    private Sprite mini_question, mini_questionSilver, mini_exclamation;

    [SerializeField]
    private SpriteRenderer statusRenderer;

    [SerializeField]
    private int questGiverID;

    [SerializeField]
    private SpriteRenderer minimapRenderer;

    private List<string> completeQuest = new List<string>();

    public Quest[] MyQuests { get => quests;  }

    public int MyQuestGiverID { get => questGiverID;  }

    public List<string> MyCompletedQuest 
    { 
        get => completeQuest; 
        
        set
        {
            completeQuest = value;

            foreach(string title in completeQuest)
            {
                for(int i = 0; i < quests.Length; i++)
                {
                    if( quests[i] != null && quests[i].MyTitle == title)
                    {
                        quests[i] = null; 
                    }
                }
            }
        }
    }

    private void Start()
    {
        foreach (Quest quest in quests)
        {
            quest.MyQuestGiver = this;
        }
    }

    public void UpdateQuestStatus()
    {
        int count = 0;

        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                if (quest.IsComplete && QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    minimapRenderer.sprite = mini_question;
                    break;
                }
                else if (!QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclamation;
                    minimapRenderer.sprite = mini_exclamation;
                    break;
                }
                else if (!quest.IsComplete &&   QuestLog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                    minimapRenderer.sprite = mini_questionSilver;
                }
            }
            else
            {
                count++;

                if (count == quests.Length)
                {
                    statusRenderer.enabled = false;
                    minimapRenderer.enabled = false;
                }
            }
        }
    }
}
