using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBook : MonoBehaviour
{
    [SerializeField]
    public Image castingBar;

    [SerializeField]
    private Text spellName;

    [SerializeField]
    private Text castTime;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Skill[] skills;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private Coroutine skillRoutine;

    private Coroutine fadeRoutine;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Skill CastSkill(int index)
    {
        castingBar.color = skills[index].MyBarColor;
        castingBar.fillAmount = 0;

        spellName.text = skills[index].MyName;

        icon.sprite = skills[index].MyIcon;

        skillRoutine =  StartCoroutine(ProgressSkill(index));

        fadeRoutine = StartCoroutine(fadeBar());

        return skills[index];
    }

    private IEnumerator CastOnCoolDown(int index)
    {
        skills[index].OnCastDone();
        skills[index].OnCoolDown = true;
        yield return new WaitForSeconds(skills[index].MyCoolDown);
        skills[index].OnCoolDown = false;
    }

    private IEnumerator ProgressSkill(int index)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / skills[index].MyCastTime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            castTime.text = (skills[index].MyCastTime - timePassed).ToString("F2");

            if (skills[index].MyCastTime - timePassed < 0)
            {
                castTime.text = "0";

                if(skills[index].MyCoolDown > 0)
                {
                    StartCoroutine(CastOnCoolDown(index));
                }
            }

            yield return null;
        }

        StopSkill();
    }

    private IEnumerator fadeBar()
    {
       

        float rate = 1.0f / 0.50f;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            

            yield return null;
        }
    }

    public void StopSkill()
    {
        if(fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }
        if(skillRoutine != null)
        {
            StopCoroutine(skillRoutine);
            skillRoutine = null;
        }
    }
}
