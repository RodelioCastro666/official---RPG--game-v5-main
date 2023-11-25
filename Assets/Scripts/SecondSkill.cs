using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondSkill : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private BoxCollider2D boxCollider;

    [SerializeField]
    private Rigidbody2D myrigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        //anim = GetComponent<Animator>();
        //boxCollider = GetComponent<BoxCollider2D>();
        //myrigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (hit) return;
        //float movementSpeed = speed * Time.deltaTime * direction;
        //transform.Translate(movementSpeed, 0, 0);
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
