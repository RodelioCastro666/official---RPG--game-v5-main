using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{

    [SerializeField]
    private float speed;

    [SerializeField]
    private string type;

    private Vector2 direction;

    [SerializeField]
    private int level;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    

    public Character MyTarget { get; set; }
    
    public Animator MyAnimator { get; set; }

    [SerializeField]
    protected float initHealth;

   public Stack<Vector3> MyPath { get; set; }

    public bool IsAttacking { get; set; }

    

    protected Coroutine actionRoutine;

    [SerializeField]
    private Transform hitBox;

    [SerializeField]
    protected Stat health;

    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    public Stat MyHealth
    {
        get { return health;  }
    }

    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }

    public float Speed { get => speed; set => speed = value; }

    public string MyType { get => type;  }

    public int MyLevel { get => level; set => level = value; }

    public Transform MyCurrenTile { get; set; }

    public Rigidbody2D MyRigidbody { get => myRigidbody;  }

    public SpriteRenderer MySpriteRenderer { get; set; }


    public Transform MyHitBox { get => hitBox; set => hitBox = value; }

    protected virtual void Start()
    {
       

       

        MyAnimator = GetComponent<Animator>();
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    public void FixedUpdate()
    {
        Move();
    }

    protected virtual void Update()
    {
        HandleLayers();
    }

    public void Move()
    {
        if (MyPath == null)
        {
            if (IsAlive)
            {
                MyRigidbody.velocity = Direction.normalized * Speed;
            }
        }

    }



    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), SCTTYPE.HEAL, true);
    }

    public void HandleLayers()
    {
        if (IsAlive)
        {
           
            if (IsAttacking)
            {
                ActivateLayer("Attack");
                direction = Vector2.zero;
                
            }
            else if (IsMoving)
            {

                ActivateLayer("Walk");


                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);
               


            }
            
            else
            {
                ActivateLayer("Idle");
                
            }
           
        }
        else
        {
            ActivateLayer("Death");
            
        }


    }

    public void StopAttack()
    {


        // StopCoroutine(attackRoutine);
        IsAttacking = false;
        MyAnimator.SetBool("attackSword", IsAttacking);


    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

   

    public virtual void TakeDamage(float damage, Character source)
    {
        

        health.MyCurrentValue -= damage;
        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTTYPE.DAMAGE, false); 
        if (health.MyCurrentValue <= 0)
        {

            Direction = Vector2.zero;
            MyRigidbody.velocity = Direction;
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");

           
        }
    }
}
