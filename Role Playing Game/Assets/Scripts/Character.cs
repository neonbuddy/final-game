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
    /// Refrence Characters animator
    /// </summary>
    protected Animator myAnimator;

    /// <summary>
    /// Indicates if character is attacking
    /// </summary>
    protected bool isAttacking = false;

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
    protected Vector2 direction;

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
            return direction.x != 0 || direction.y != 0;
        }
    }


    /// <summary>
    /// Virtual Start() allows it to be overritten in player class
    /// </summary>
    protected virtual void Start()
    {
        health.Initialize(startHealth, startHealth);

        myRigidbody = GetComponent<Rigidbody2D>();         //Refrences characters rigid body

        myAnimator = GetComponent<Animator>();             //Refrences characters Animator controller
    }

    /// <summary>
    /// Virtual Update() allows it to be overritten in player class
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
        myRigidbody.velocity = direction.normalized * speed;

    }

    /// <summary>
    /// Handles animation layers
    /// Checks if character is stationary, moving, or attacking; plays animation for each instance
    /// </summary>
    public void HandleLayers()
    {
        if (IsMoving)
        {
            ActivateLayer("WalkLayer");

            //Sets animation paramater so Character faces right direction
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);

            StopAttack();                                   //Character cant move while attacking
        }
        else if (isAttacking)
        {
            ActivateLayer("AttackLayer");
        }
        else
        {
            ActivateLayer("IdleLayer");         //Character will idle if no input
        }

        //TODO *Need death animation!*


    }


    /// <summary>
    /// Uses Character direction to change animation
    /// </summary>
    /// <param name="direction"></param>
    public void AnimateMovement(Vector2 direction)     
    {


    }

    /// <summary>
    /// Activates proper animation layer
    /// </summary>
    /// <param name="layerName"></param>
    public void ActivateLayer(string layerName)         
    {
        for (int i = 0; i < myAnimator.layerCount; i++)         //Disables all other layers
        {
            myAnimator.SetLayerWeight(i, 0);
        }

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);     //Uses layer name to get index and enables it, activating propper animation
    }

    /// <summary>
    /// Stops character attack
    /// </summary>
    public virtual void StopAttack()
    {

        isAttacking = false;
        myAnimator.SetBool("attack", isAttacking);  //Stops attack animation

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            //Debug.Log("attack stopped");
        }
    }

    /// <summary>
    /// Function makes character take damage
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;

        if(health.MyCurrentValue <= 0)
        {
            myAnimator.SetTrigger("die");           //Links to "die" paramater under Animator for enemy and Player
        }

    }


}
