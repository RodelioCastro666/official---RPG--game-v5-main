using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }


    [SerializeField]
    private Stat mana;

    [SerializeField]
    private Stat xpStat;

    
    private FirstSkill firstskill;

    private int intelligence;

    private int vitality;

    private int strength;

    

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Transform[] exitPoints;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private Blocks[] blocks;

    [SerializeField]
    private Animator ding;

    [SerializeField]
    private Crafting profession;

    [SerializeField]
    private Image cooldownOverlay;

    [SerializeField]
    private Text countDown;

    private float coolDowntime;

    [SerializeField]
    private Image sccooldownOverlay;

    [SerializeField]
    private Text sccountDown;

    private float sccoolDowntime;

    [SerializeField]
    private Image thcooldownOverlay;

    [SerializeField]
    private Text thcountDown;

    private float thcoolDowntime;

    private Vector2 initPos;

    public Coroutine MyInitRoutine { get; set; }

    private int exitIndex = 2;

    private Vector3 max, min;

    private List<Enemy> attackers = new List<Enemy>();

    private float initMana = 50;

    private List<IInteractable> interactables = new List<IInteractable>();

    [SerializeField]
    private Transform minimapIcon;

    

    [SerializeField]
    private FixedJoystick JoyStick;

    
    private SkillBook skillBook;

    public int MyGold { get; set; }

    public bool inCombat { get; set; } = false;

    public List<IInteractable> MyInteractables { get => interactables; set => interactables = value; }

    public Stat MyXp
    {
        get => xpStat; set => xpStat = value;
    }

    public Stat MyMana { get => mana; set => mana = value; }
    public FirstSkill MyFirstskill { get => firstskill; set => firstskill = value; }

    private bool isDashing = false;

    protected override void Start()
    {
        base.Start();
        skillBook = GetComponent<SkillBook>();
        StartCoroutine(Regen());
    }

    protected override void Update()
    {
        GetInput();
        //ClickToMove();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);

        base.Update();

    }

    

    public void SetDefaultValues()
    {
        MyGold = 10000;
        vitality = 50;
        intelligence = 10;
        strength = 2;
        ResetStats();
        
        MyXp.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        levelText.text = MyLevel.ToString();
        initPos = transform.parent.position;
        UiManager.MyInstance.UpdateStatsText(intelligence, vitality, strength);
    }

    private void ResetStats()
    {
        MyHealth.Initialize(vitality * VitalityMultiplier(), vitality * VitalityMultiplier());
        MyMana.Initialize(intelligence * IntelligenceMultiplier(), intelligence * IntelligenceMultiplier());
        
    }

    
    private void UpdateMaxStats()
    {
        MyHealth.SetMaxValue(vitality * VitalityMultiplier());
        MyMana.SetMaxValue(intelligence * IntelligenceMultiplier());
        



             
    }

    private int VitalityMultiplier()
    {
        if(MyLevel < 10)
        {
            return 2;
        }
        else if(MyLevel > 10)
        {
            return 5;
        }

        return 3;
    }

    private int IntelligenceMultiplier()
    {
        if (MyLevel < 10)
        {
            return 5;
        }
        else if (MyLevel > 10)
        {
            return 10;
        }

        return 15;
    }

    private int StrengthMultiplier()
    {
        if (MyLevel < 10)
        {
            return 2;
        }
        else if (MyLevel > 10)
        {
            return 4;
        }

        return 10;
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    

    public void GetInput()
    {
        direction = Vector2.zero;

        direction.x = JoyStick.Horizontal;
        direction.y = JoyStick.Vertical;

       
        // if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        //  {
        //  direction.y = 0;
        if (direction.x > 0)
        {
            Debug.Log("right");
            exitIndex = 1;
            
            if (direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 270);
            }
        }

        if (direction.x < 0)
        {
            Debug.Log("left");
            exitIndex = 3;
            if (direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 90);
            }

        }
        // }
        //  else
        //  {
       // direction.x = 0;

        if (direction.y > 0)
        {
            Debug.Log("up");
            exitIndex = 0;

            minimapIcon.eulerAngles = new Vector3(0, 0, 0);

        }

        if (direction.y < 0)
        {
            Debug.Log("down");
            exitIndex = 2;
            minimapIcon.eulerAngles = new Vector3(0, 0, 180);


        }
        //  } 

        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(30);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            MyMana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            MyMana.MyCurrentValue += 10;
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["UPB"]))
        {
            exitIndex = 0;
            Direction += Vector2.up;
            minimapIcon.eulerAngles = new Vector3(0, 0, 0);
        }


        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["DOWNB"]))
        {
            exitIndex = 2;
            Direction += Vector2.down;
            minimapIcon.eulerAngles = new Vector3(0, 0, 180);
        }


        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["LEFTB"]))
        {
            exitIndex = 3;
            Direction += Vector2.left;
            if (Direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 90);
            }

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDashing = true;
        }
        


        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["RIGHTB"]))
        {
            exitIndex = 1;
            Direction += Vector2.right;

            if (Direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 270);
            }
        }

        if (IsMoving)
        {
            StopAction();
            StopInit();
        }

       


    }

    private void  FixedUpdate()
    {
        MyRigidbody.velocity = direction * Speed;

        if (isDashing)
        {
            float dashAmount = 3f;
            Vector3 dashPosition = transform.position + direction * dashAmount ;

            DashEffect.CreateDashEffect(transform.position, direction, Vector3.Distance(transform.position, dashPosition));
            MyRigidbody.MovePosition(dashPosition);
            isDashing = false;
        }
    }


    private IEnumerator Ding()
    {
        while (!MyXp.IsFUll)
        {
            yield return null;
        }

        MyLevel++;
        ding.SetTrigger("Ding");
        levelText.text = MyLevel.ToString();
        MyXp.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f);
        MyXp.MyMaxValue = Mathf.Floor(MyXp.MyMaxValue);
        MyXp.MyCurrentValue = MyXp.MyOverFlow;
        MyXp.Reset();
        vitality += IncreaseBaseStat();
        intelligence += IncreaseBaseStat();
        
        ResetStats();
        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public void EquipGear(Armor armor)
    {
        vitality += armor.Vitality;
        intelligence += armor.Intellect;
        strength += armor.Strength;
        UpdateMaxStats();
        UiManager.MyInstance.UpdateStatsText(intelligence, vitality, strength);
    }

    public void DEquipGear(Armor armor)
    {
        vitality -= armor.Vitality;
        intelligence -= armor.Intellect;
        strength -= armor.Strength;
        UpdateMaxStats();
        UiManager.MyInstance.UpdateStatsText(intelligence, vitality, strength);
    }

    private int IncreaseBaseStat()
    {
        if(MyLevel < 10)
        {
            return 3;
        }

        return 10;
    }

    public IEnumerator Respawn()
    {
        MySpriteRenderer.enabled = false;
        yield return new WaitForSeconds(5f);
        health.Initialize(initHealth, initHealth);
        MyMana.Initialize(initMana, initMana);
        transform.parent.position = initPos;
        MySpriteRenderer.enabled = true;
        MyAnimator.SetTrigger("respawn");
    }

    private void StopInit()
    {
        if (MyInitRoutine != null)
        {
            StopCoroutine(MyInitRoutine);
        }
    }

    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }

    public void GainXP(int xp)
    {
        MyXp.MyCurrentValue += xp;
        CombatTextManager.MyInstance.CreateText(transform.position, xp.ToString(), SCTTYPE.XP, false);

        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    private IEnumerator GatherRoutine(ICastable castable, List<Drop> items)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        LootWindow.MyInstance.CreatePages(items);
    }

    private IEnumerator AttackRoutine(ICastable castable)
    {
        Transform currentTarget = MyTarget.MyHitBox;

        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        if (currentTarget != null && InLineOfSight())
        {
            Spell newSpell = SpellBook.MyInstance.GetSpell(castable.MyTitle);

            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.Initialize(currentTarget, newSpell.MyDamage, this);
        }

        StopAction();
    }

    private IEnumerator ActionRoutine(ICastable castable)
    {
        SpellBook.MyInstance.Cast(castable);

        IsAttacking = true;

        MyAnimator.SetBool("attack", IsAttacking);

        yield return new WaitForSeconds(castable.MyCastTime);

        StopAction();
    }

    

    private IEnumerator AttackSword()
    {
        if (!IsAttacking)
        {
            IsAttacking = true;
            MyAnimator.SetBool("attackSword", true);

            yield return new WaitForSeconds(0.5f);

            StopAttack();
        }


    }

    

   
    public void CastSpell(ICastable castable)
    {
        Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving && InLineOfSight())
        {
            MyInitRoutine= StartCoroutine(AttackRoutine(castable));
        }

      
    }

    public void Gather(ICastable castable, List<Drop> items)
    {
        if (!IsAttacking)
        {
            MyInitRoutine = StartCoroutine(GatherRoutine(castable, items));
        }
    }
    
    public IEnumerator CraftRoutine(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));
        profession.AddItemsToInventory();
    }

    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
        }
      

        return false;
    }

    private IEnumerator Regen()
    {
        while (true)
        {
            if (!inCombat)
            {
                if (health.MyCurrentValue < health.MyMaxValue)
                {
                    int value = Mathf.FloorToInt(health.MyMaxValue * 0.05f);
                    health.MyCurrentValue += value;

                    CombatTextManager.MyInstance.CreateText(transform.position, value.ToString(), SCTTYPE.HEAL, false);
                }

               

                if (mana.MyCurrentValue < mana.MyMaxValue)
                {
                    int value = Mathf.FloorToInt(mana.MyMaxValue * 0.05f);
                    mana.MyCurrentValue += value;

                    CombatTextManager.MyInstance.CreateText(transform.position, value.ToString(), SCTTYPE.MANA, false);
                }
            }

            yield return new WaitForSeconds(1.5f);
        }

        
    }

    private void Block()
    {
        foreach (Blocks b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }

   

    public  void StopAction()
    {
        SpellBook.MyInstance.StopCasting();

        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
            IsAttacking = false;
            MyAnimator.SetBool("attack", IsAttacking);
        }

        
    }

    public override void AddAttacker(Character attacker)
    {
        int count = base.Attackers.Count;

        base.AddAttacker(attacker);

        if(count == 0)
        {
            inCombat = true;
            CombatTextManager.MyInstance.CreateText(transform.position, "COMBAT MODE", SCTTYPE.TEXT, false);
        }

        
    }

    public override void RemoveAttacker(Character attacker)
    {
        base.RemoveAttacker(attacker);
        if (base.Attackers.Count == 0)
        {
            inCombat = false;
            CombatTextManager.MyInstance.CreateText(transform.position, "SAFE MODE", SCTTYPE.TEXT, false);
        }
       
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();

            if (!MyInteractables.Contains(interactable))
            {
                MyInteractables.Add(interactable);
            }

           
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        

        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            Debug.Log("jkjkj");
            if (MyInteractables.Count > 0)
            {
                IInteractable interactable = MyInteractables.Find(x => x == collision.GetComponent<IInteractable>());

                if(interactable != null)
                {
                    interactable.StopInteract();
                }

                MyInteractables.Remove(interactable);
            }

           
        }
    }

    private IEnumerator ProgressSkill()
    {
        cooldownOverlay.fillAmount = 1;
        countDown.enabled = true;
        cooldownOverlay.enabled = true;

        float timePassed = Time.deltaTime;

        float rate = 1.0f / coolDowntime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            cooldownOverlay.fillAmount = Mathf.Lerp(1, 0, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            countDown.text = (coolDowntime - timePassed).ToString("F1");

            if (coolDowntime - timePassed < 0)
            {
                countDown.text = "0.0";
                cooldownOverlay.enabled = false;
                countDown.enabled = false;
            }

            yield return null;
        }

        
    }

    public void RunCooldown()
    {
        StartCoroutine(ProgressSkill());
    }

    private IEnumerator ScProgressSkill()
    {
        sccooldownOverlay.fillAmount = 1;
        sccountDown.enabled = true;
        sccooldownOverlay.enabled = true;

        float timePassed = Time.deltaTime;

        float rate = 1.0f / sccoolDowntime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            sccooldownOverlay.fillAmount = Mathf.Lerp(1, 0, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            sccountDown.text = (sccoolDowntime - timePassed).ToString("F1");

            if (sccoolDowntime - timePassed < 0)
            {
                sccountDown.text = "0.0";
                sccooldownOverlay.enabled = false;
                sccountDown.enabled = false;
            }

            yield return null;
        }


    }
    public void ScRunCooldown()
    {
        StartCoroutine(ScProgressSkill());
    }
    private IEnumerator ThProgressSkill()
    {
        thcooldownOverlay.fillAmount = 1;
        thcountDown.enabled = true;
        thcooldownOverlay.enabled = true;

        float timePassed = Time.deltaTime;

        float rate = 1.0f / thcoolDowntime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            thcooldownOverlay.fillAmount = Mathf.Lerp(1, 0, progress);

            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;

            thcountDown.text = (thcoolDowntime - timePassed).ToString("F1");

            if (thcoolDowntime - timePassed < 0)
            {
                thcountDown.text = "0.0";
                thcooldownOverlay.enabled = false;
                thcountDown.enabled = false;
            }

            yield return null;
        }


    }
    public void ThRunCooldown()
    {
        StartCoroutine(ThProgressSkill());
    }

    private IEnumerator FirstSkill()
    {
            

        Skill skill = skillBook.CastSkill(0);

       

        if (!skill.OnCoolDown)
        {
            if (skill.ManaCost <= mana.MyCurrentValue)
            {

                if (!isUsingFirstSkill)
                {
                    

                    isUsingFirstSkill = true;
                    MyAnimator.SetBool("FSattack", isUsingFirstSkill);

                    coolDowntime = skill.MyCoolDown;
                    skill.CastDone += new Done(RunCooldown);

                    yield return new WaitForSeconds(skill.MyCastTime);

                    Vector2 temp = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));

                    FirstSkill firstSkill = Instantiate(skill.MySkillPrefab, transform.position, Quaternion.identity).GetComponent<FirstSkill>();
                    firstSkill.SetUp(temp, ChooseFirstSkillDirection());
                    firstSkill.Initialize(skill.MyDamage * strength, transform);

                    yield return new WaitForSeconds(0.5f);

                    

                    mana.MyCurrentValue -= skill.ManaCost;

                    StopFirstSkill();
                }

            }
        }

      
         
    }

    private IEnumerator SecondSkill()
    {
        Skill skill = skillBook.CastSkill(1);

        if (!isUsingSecondSkill)
        {
            isUsingSecondSkill = true;
            MyAnimator.SetBool("SSattack", isUsingSecondSkill);

            sccoolDowntime = skill.MyCoolDown;
            skill.CastDone += new Done(ScRunCooldown);

            yield return new WaitForSeconds(skill.MyCastTime);

            Vector2 temp = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));
            SecondSkill secondtSkill = Instantiate(skill.MySkillPrefab, transform.position, Quaternion.identity).GetComponent<SecondSkill>();
            secondtSkill.SetUp(temp, ChooseFirstSkillDirection());
            secondtSkill.Initialize(skill.MyDamage * strength, transform);
            yield return new WaitForSeconds(1);

            StopSecondSkill();
        }


    }

    private IEnumerator ThirdSkill()
    {
        Skill skill = skillBook.CastSkill(2);

        if (!isUsingThidSkill)
        {
            isUsingThidSkill = true;
            MyAnimator.SetBool("TSattack", isUsingThidSkill);

            thcoolDowntime = skill.MyCoolDown;
            skill.CastDone += new Done(ThRunCooldown);

            yield return new WaitForSeconds(skill.MyCastTime);

            Vector2 temp = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));
            ThirdSkill thirdSkill = Instantiate(skill.MySkillPrefab, transform.position, Quaternion.identity).GetComponent<ThirdSkill>();
            thirdSkill.SetUp(temp, ChooseFirstSkillDirection());
            thirdSkill.Initialize(skill.MyDamage * strength, transform);
            yield return new WaitForSeconds(0.5f);

            StopThirdSkill();
        }


    }

    private IEnumerator NormalSkill()
    {
        Skill skill = skillBook.CastSkill(3);

        if(skill.ManaCost <= mana.MyCurrentValue)
        {
            if (!isUsingNormalSkill)
            {
                isUsingNormalSkill = true;
                MyAnimator.SetBool("Nattack", isUsingNormalSkill);

                yield return new WaitForSeconds(skill.MyCastTime);

                Vector2 temp = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));
                NormalAttack normalAttack = Instantiate(skill.MySkillPrefab, transform.position, Quaternion.identity).GetComponent<NormalAttack>();
                normalAttack.SetUp(temp, ChooseFirstSkillDirection());
                normalAttack.Initialize(skill.MyDamage * strength, transform);



                StopNormalSkill();
            }
        }

      


    }

    public Vector3 ChooseFirstSkillDirection()
    {
        float temp = Mathf.Atan2(MyAnimator.GetFloat("y"),MyAnimator.GetFloat("x")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    protected virtual void StopFirstSkill()
    {
        skillBook.StopSkill();

        isUsingFirstSkill = false;
        MyAnimator.SetBool("FSattack", isUsingFirstSkill);
    }
    protected virtual void StopSecondSkill()
    {
        skillBook.StopSkill();
        isUsingSecondSkill = false;
        MyAnimator.SetBool("SSattack", isUsingSecondSkill);
    }
    protected virtual void StopThirdSkill()
    {
        skillBook.StopSkill();
        isUsingThidSkill = false;
        MyAnimator.SetBool("TSattack", isUsingThidSkill);
    }
    protected virtual void StopNormalSkill()
    {
        skillBook.StopSkill();
        isUsingNormalSkill = false;
        MyAnimator.SetBool("Nattack", isUsingNormalSkill);
    }
    public void CastFirstSkill()
    {
        
        StartCoroutine(FirstSkill());
    }
    public void CastSecondSkill()
    {
        StartCoroutine(SecondSkill());
    }
    public void CastThirdSkill()
    {
        StartCoroutine(ThirdSkill());
    }
    public void CastNormalSkill()
    {
        StartCoroutine(NormalSkill());
    }

    public void Dash()
    {
        isDashing = true;
    }
}
