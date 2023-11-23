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

    public void Cast(ICastable castable)
    {
       

        castingBar.color = castable.MyBarColor;

        castingBar.fillAmount = 0;

        currentSpell.text = castable.MyTitle;

        icon.sprite = castable.MyIcon;

        spellRoutine = StartCoroutine(Progress(castable));

        fadeRoutine = StartCoroutine(FadeBar());

        
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

    private  IEnumerator Progress (ICastable castable)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / castable.MyCastTime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (castable.MyCastTime - timePassed).ToString("F2");

            if (castable.MyCastTime - timePassed < 0)
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
        Spell spell = Array.Find(spells, x => x.MyTitle == spellName);

        return spell;
    }

}
