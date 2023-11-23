using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{

    [SerializeField]
    private Text dateTime;

    [SerializeField]
    private Image health;

    [SerializeField]
    private Image mana;

    [SerializeField]
    private Image xp;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text manaText;

    [SerializeField]
    private Text xpText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private GameObject visuals;

    [SerializeField]
    private int index;

    public int MyIndex { get => index; }

    private void Awake()
    {
        visuals.SetActive(false);
    }

    public void ShowInfo(SaveData savedata)
    {
        visuals.SetActive(true);
        dateTime.text = "Date: " + savedata.MyDateTime.ToString("dd/MM/yyyy") + "- Time " + savedata.MyDateTime.ToString("H:mm");
        health.fillAmount = savedata.MyPlayerData.MyHealth / savedata.MyPlayerData.MyMaxHealth;
        healthText.text = savedata.MyPlayerData.MyHealth + " / " + savedata.MyPlayerData.MyMaxHealth;

        mana.fillAmount = savedata.MyPlayerData.MyMana / savedata.MyPlayerData.MyMaxMana;
        manaText.text = savedata.MyPlayerData.MyMana + " / " + savedata.MyPlayerData.MyMaxMana;

        xp.fillAmount = savedata.MyPlayerData.MyXp / savedata.MyPlayerData.MyMaxXp;
        xpText.text = savedata.MyPlayerData.MyXp + " / " + savedata.MyPlayerData.MyMaxXp;

        levelText.text = savedata.MyPlayerData.MyLevel.ToString();
    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }
}
