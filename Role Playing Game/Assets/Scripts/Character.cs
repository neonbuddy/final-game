using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class which player, NPC's, and Enemies will inherit from
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]             //These requirements will automatically add components, if you try to add script to a game object that
[RequireComponent(typeof(Animator))]                //inherits from character.
public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// Speed of the Character
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// Refrence Characters animator; Property that allows AttackState to access
    /// </summary>
    public Animator MyAnimator { get; set; }

    /// <summary>
    /// Indicates if character is attacking; Property that allows AttackState to access
    /// </summary>
    public bool IsAttacking { get; set; }

    /// <summary>
    /// Reference to attack coroutine
    /// </summary>
    protected Coroutine attackRoutine;          //Reference to attack coroutine; fixes bug w/ attack animation canceling early

    /// <summary>
    /// Characters hitbox
    /// </summary>
    [SerializeField]
    protected Transform hitBox;

    /// <summary>
    /// Characters health
    /// </summary>
    [SerializeField]
    protected Stat health;

    public Transform MyTarget { get; set; }

    //Gets character health so it can be used in frame and elsewhere
    public Stat MyHealth
    {
        get { return health; }
    }

    /// <summary>
    /// Character starting health, can set this on enemy and player in Unity inspector
    /// </summary>
    [SerializeField]
    private float startHealth;

    /// <summary>
    /// Character's Direction
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// References character's rigidbody
    /// </summary>
    private Rigidbody2D myRigidbody;

    /// <summary>
    /// Determines if character is moving or not
    /// </summary>
    public bool IsMoving
    {
        get
            {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }    //Property that allows access to private field, can be accessed by Enemy States

    public float Speed { get => speed; set => speed = value; }                  //Property that allows access to private speed field, can be accessed by Enemy States


    /// <summary>
    /// Property used to determine if character alive or dead
    /// </summary>
    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;       //Returns true
        }
    }


    /// <summary>
    /// Virtual Start() allows it to be overritten in player class
    /// </summary>
    protected virtual void Start()
    {
        health.Initialize(startHealth, startHealth);

        myRigidbody = GetComponent<Rigidbody2D>();         //Refrences characters rigid body

        MyAnimator = GetComponent<Animator>();             //Refrences characters Animator controller
    }

    /// <summary>
    /// Virtual Update() allows it to be overritten in player and enemy classes
    /// </summary>
    protected virtual void Update()              //Allows override of player update function (Called once per frame)
    {
        HandleLayers();             //Animation layers
    }

    private void FixedUpdate()                  //Used every time rigid body need manipulated
    {
        Move();
    }


    /// <summary>
    /// Moves the Character
    /// </summary>
    public void Move()
    {
        if (IsAlive)
        {
            myRigidbody.velocity = Direction.normalized * Speed;        //Moves character
        }

        

    }

    /// <summary>
    /// Handles animation layers
    /// Checks if character is stationary, moving, or attacking; plays animation for each instance
    /// </summary>
    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)   //Check for character: if moving, idling, attacking, or dying
            {
                ActivateLayer("WalkLayer");

                //Sets animation paramater so Character faces right direction
                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);
            }
            else if (IsAttacking)
            {
                ActivateLayer("AttackLayer");       //Player will attack with no input; Enemy will
            }
            else
            {
                ActivateLayer("IdleLayer");         //Player will idle with no input; Enemy will idle with no Player in AggroRange
            }
        }
        else
        {
            ActivateLayer("DeathLayer");            //Player and Enemy will die if health stat goes below 0 (TakeDamage function)
        }
    }

    /// <summary>
    /// Activates proper animation layer
    /// </summary>
    /// <param name="layerName"></param>
    public void ActivateLayer(string layerName)         
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)         //Disables all other layers
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);     //Uses layer name to get index and enables it, activating propper animation
    }

    /// <summary>
    /// Function makes character take damage
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage, Transform source)
    {
        health.MyCurrentValue -= damage;

        if(health.MyCurrentValue <= 0)              //If character health falls below 0
        {

            Direction = Vector2.zero;
            myRigidbody.velocity = Direction;       //Makes it so character cannot continue moving when dead

            MyAnimator.SetTrigger("die");           //Trigger Death Animation
                                                    //(Links to "die" paramater under Animator for Enemy and Player)
        }

    }


}