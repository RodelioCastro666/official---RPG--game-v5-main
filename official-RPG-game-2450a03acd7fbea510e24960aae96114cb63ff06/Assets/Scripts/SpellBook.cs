using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpellBook : MonoBehaviour
{
    private static SpellBook instance;


    public static SpellBook MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Image castingBar;

    [SerializeField]
    private Text currentSpell;

    [SerializeField]
    private Text castTime;

    [SerializeField]
    private Spell[] spells;


    [SerializeField]
    private Image icon;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private Coroutine spellRoutine;

    private Coroutine fadeRoutine;
   
    void Start()
    {
        
    }

    
    void Update()
    {
       
    }

    public Spell CastSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        castingBar.color = spell.MyBarColor;

        castingBar.fillAmount = 0;

        currentSpell.text = spell.MyName;

        icon.sprite = spell.MyIcon;

        spellRoutine = StartCoroutine(Progress(spell));

        fadeRoutine = StartCoroutine(FadeBar());

        return spell;
    }

    private IEnumerator FadeBar()
    {
       

        float rate = 1.0f / 0.25f;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
           canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    private  IEnumerator Progress (Spell spell)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / spell.MyCastTime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (spell.MyCastTime - timePassed).ToString("F2");

            if (spell.MyCastTime - timePassed < 0)
            {
                castTime.text = "0";
            }

            yield return null;
        }

        StopCasting();
    }

    public void StopCasting()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }

        if (spellRoutine != null)
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        return spell;
    }

}
