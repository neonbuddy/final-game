using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidBody;    //Allows movement of spell sprite

    /// <summary>
    /// Controls how fast spell moves
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// Gets target of the spell
    /// </summary>
    public Transform MyTarget { get; private set; }

    private Transform source;   //Where the spell comes from

    private int damage;
    
    // Start is called before the first frame update
    void Start()
    {
        //Creates reference to spell rigid body
        myRigidBody = GetComponent<Rigidbody2D>();

        //target = GameObject.Find("Target").transform;      //For Testing by hardcoding object name as "Target"
    }

    public void Initialize(Transform target, int damage, Transform source)
    {
        this.MyTarget = target;     //Target put into initialize is MyTarget
        this.damage = damage;       //Initializes damage
        this.source = source;       //Initializes source
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (MyTarget != null)        //Used with stopping spell after collision    
        {
            //Calculates spell direction
            Vector2 direction = MyTarget.position - transform.position;

            //Moves spell with rigid body
            myRigidBody.velocity = direction.normalized * speed;

            //Calculates rotation angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //Rotates spell towards target
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }

    /// <summary>
    /// Detects collision with enemy, setting off impact animation
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == MyTarget)       //Makes sure spell hits collision box of selected target. This prevents it from accidently hitting enemy in front of target
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;                                                          //Used to prevent glitches with spell moving after enemy dies
            c.TakeDamage(damage, source);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;                                //Removes target after collision
        }
    }

}
