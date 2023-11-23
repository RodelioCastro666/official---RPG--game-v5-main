using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UiManager : MonoBehaviour
{

    public static UiManager instance;

    public static UiManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UiManager>();
            }

            return instance;
        }
    }

   
    

    private Stat healthStat;

    private GameObject[] keybindButtons;

    [SerializeField]
    private Image portraitFrame;


    [SerializeField]
    private CanvasGroup keybindsMenu;

    [SerializeField]
    private CanvasGroup spellBook;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private ActionButtons[] actionButtons;

    [SerializeField]
    private CanvasGroup[] menus;
   

    [SerializeField]
    private GameObject targetFrame;

    [SerializeField]
    private GameObject toolTip;

    [SerializeField]
    private CharacterPanel charPanel;

    private Text toolTipText;

   

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        toolTipText = toolTip.GetComponentInChildren<Text>();
    }

    void Start()
    {
       

       

        healthStat = targetFrame.GetComponentInChildren<Stat>();

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            OpenClose(menus[0]);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            OpenClose(menus[1]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScripts.MyInstance.OpenClose();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenClose(menus[2]);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OpenClose(menus[3]);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(menus[6]);
        }





        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    OpenClose(keybindsMenu);
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    OpenClose(spellBook);
        //}
        //if(Input.GetKeyDown(KeyCode.B))
        //{
        //    InventoryScripts.MyInstance.OpenClose();
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    charPanel.OpenClose();
        //}

    }



    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        portraitFrame.sprite= target.MyPortrait;

        levelText.text = target.MyLevel.ToString();

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        if (target.MyLevel >= Player.MyInstance.MyLevel +5)
        {
            levelText.color = Color.red;
        }
        else if(target.MyLevel == Player.MyInstance.MyLevel +3 || target.MyLevel == Player.MyInstance.MyLevel + 4)
        {
            levelText.color = new Color32(255, 124, 0, 255);
        }
        else if(target.MyLevel >= Player.MyInstance.MyLevel - 2 && target.MyLevel <= Player.MyInstance.MyLevel + 2)
        {
            levelText.color = Color.yellow;
        }
        else if (target.MyLevel <= Player.MyInstance.MyLevel -3 && target.MyLevel >Xpmanager.CalculateGrayLevel())
        {
            levelText.color = Color.green;
        }
        else
        {
            levelText.color = Color.grey;
        }
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }

    

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
        
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach(CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);
        }

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void UpdateStackSize(IClickable clickable)
    {

        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;

        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }

        if (clickable.MyCount == 0)
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.color = new Color(0, 0, 0, 0);
        clickable.MyIcon.color = Color.white;
    }

    public void ShowToolTip(IDescribable description)
    {
        
        toolTip.SetActive(true);
        toolTipText.text= description.GetDescription();
    }

    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }

    public void RefreshToolTip(IDescribable description)
    {
        toolTipText.text = description.GetDescription();
    }
}
