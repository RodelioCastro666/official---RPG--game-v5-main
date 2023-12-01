using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueWindow : Window
{
    public static DialogueWindow instance;

    public static DialogueWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueWindow>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Text text;

    [SerializeField]
    private GameObject answerButtonPrefab;

    [SerializeField]
    private Transform answerTransform;

    private Dialogue dialogue;

    private DialogueNode currentNode;

    [SerializeField]
    private float speed;

    private List<DialogueNode> answers = new List<DialogueNode>();

    private List<GameObject> buttons = new List<GameObject>();

    public void SetDialogue(Dialogue dialoguee)
    {
        
        text.text = string.Empty;

        dialogue = dialoguee;

        currentNode = dialoguee.Nodes[0];

        StartCoroutine(RunDialogue(currentNode.Text));
    }

    public void CloseDialogue()
    {
        Close();
        Clear();
    }

    private IEnumerator RunDialogue(string dialogue)
    {
        for(int i = 0; i < dialogue.Length; i++)
        {
            text.text += dialogue[i];
            yield return new WaitForSeconds(speed);
            Debug.Log(dialogue.Length);
        }

        ShowAnswers();
    }

    private void ShowAnswers()
    {
        answers.Clear();

        foreach(DialogueNode node in dialogue.Nodes)
        {
            if(node.Parent == currentNode.Name)
            {
                answers.Add(node);
            }
        }
        if(answers.Count > 0)
        {
            answerTransform.gameObject.SetActive(true);

            foreach(DialogueNode node in answers)
            {
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                go.GetComponentInChildren<Text>().text = node.Answer;
                go.GetComponent<Button>().onClick.AddListener(delegate { PickAnswer(node); });
            }
        }
        else
        {
            answerTransform.gameObject.SetActive(true);
            GameObject go = Instantiate(answerButtonPrefab, answerTransform);
            buttons.Add(go);
            go.GetComponentInChildren<Text>().text = "Close";
            go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
        }
    }

    private void PickAnswer(DialogueNode node)
    {
        this.currentNode = node;
        Clear();
        StartCoroutine(RunDialogue(currentNode.Text));
    }
    
    private void Clear()
    {
        text.text = string.Empty;

        answerTransform.gameObject.SetActive(false);

        foreach(GameObject gameObject in buttons)
        {
            Destroy(gameObject);
        }

        buttons.Clear();
    }

}