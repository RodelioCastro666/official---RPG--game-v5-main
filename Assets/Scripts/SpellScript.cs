    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellScript : MonoBehaviour
{

    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody2D myRigidBody;

    private Character source;

    private int damage;

    public Transform MyTarget { get; private set; }

    

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        
    }

    public void Initialize(Transform target, int damage, Character source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.source = source;
    }

    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            Vector2 direction = MyTarget.position - transform.position;

            myRigidBody.velocity = direction.normalized * speed;


            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" )
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage,source);
            GetComponent<Animator>().SetTrigger("impact");
            
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
            
        }
    }

  
}
