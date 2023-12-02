using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private BoxCollider2D boxCollider;

    public int damage;

    private Transform source;



   

    [SerializeField]
    private Rigidbody2D myrigidbody;

    // Start is called before the first frame update
    void Awake()
    {

        // boxCollider = GetComponent<BoxCollider2D>();
        myrigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (hit) return;
        //float movementSpeed = speed * Time.deltaTime * direction;
        //transform.Translate(movementSpeed, 0, 0);
    }

    public void Initialize(int damage, Transform source)
    {
        this.damage = damage;
        this.source = source;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            speed = 0;
            collision.GetComponent<Enemy>().TakeDamage(damage);
            GetComponent<Animator>().SetTrigger("Impacto");
            myrigidbody.velocity = Vector2.zero;
        }
    }


    public void SetUp(Vector2 velocity, Vector3 direction)
    {
        myrigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);

    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
