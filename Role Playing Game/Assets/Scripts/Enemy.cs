using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{

    [SerializeField]
    private CanvasGroup healthGroup;    //Show healthbar when enemy targeted

    private IState currentState;

    public float MyAttackRange { get; set; }     //Property

    public float MyAttackTime { get; set; }     //Property

    public Vector3 MyStartPosition { get; set; }

    /// <summary>
    /// Base aggro range for enemy
    /// </summary>
    [SerializeField]
    private float initAggroRange;

    /// <summary>
    /// Property that sets the distance for the enemy to aggro/ stop following
    /// </summary>
    public float MyAggroRange { get; set; }     //Initialized to initAggroRange; changes based on the distance player attacks enemy

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;      //If distance between enemy and player is less than aggro range; enemy is InRange
        }
    }
    

    protected void Awake()      //Readies items for use
    {
        MyStartPosition = transform.position;       //Sets StartPosition to position at beginning of game
        MyAggroRange = initAggroRange;              //Set to initial aggro range
        MyAttackRange = 1;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)                        //If not attacking (Used as a catch to make sure cooldown time is accurate)
            {
                MyAttackTime += Time.deltaTime;     //Track time since last attack
            }

            currentState.Update();                  //Updates enemy's current state
        }
           base.Update();                          //Allows override of character update function

    }


    public override Transform Select()
    {
        healthGroup.alpha = 1;          //When enemy selected, healthbar is visible

        return base.Select();           //makes base function from NPC still go through
    }

    public override void DeSelect()
    {
        healthGroup.alpha = 0;          //When enemy deselected, healthbar is invisible

        base.DeSelect();           //makes base function from NPC still go through
    }

    /// <summary>
    /// Makes it so enemy can take damage
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(float damage, Transform source)
    {
        if (!(currentState is ReturnState))     //If enemy not returning *Possibly change this so if enemy is returning and takes damage; ReAggro?*
        {
            SetTarget(source);

            base.TakeDamage(damage, source);        //Reduces health and updates MyCurrentValue; TakeDamage is overwritten from character script.

            OnHealthChanged(health.MyCurrentValue);     //healthChanged event is triggered by taking damage (Under UI); sending value to OnHealthChanged.
        }


    }

    public void ChangeState(IState newState)
    {
        //Before changing state must Exit old state and Enter a new state

        if (currentState != null)   //If there is a current state
        {
            currentState.Exit();    //Exit old state
        }

        currentState = newState;    //Sets current state

        currentState.Enter(this);       //Enter new state;       *Enter() references this =>Enemy *
    }

    public void SetTarget(Transform target)
    {
        if(MyTarget == null && !(currentState is ReturnState))      //If no target, and not returning  *Could remove the second check if I want player to kite enemy*
        {
            float distance = Vector2.Distance(transform.position, target.position);     //Calcualtes distance between target and enemy
            MyAggroRange = initAggroRange;  //Resets AggroRange
            MyAggroRange += distance;       //Aggro range set based on distance between character an player
            MyTarget = target;
        }
    }

    /// <summary>
    /// Resets Enemy's Aggro Range and Health; Called in Idle
    /// </summary>
    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;                         //Resets aggro range
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;    //Resets health to default                            **TODO Move this to Character and have it regen over time if out of combat**
        OnHealthChanged(health.MyCurrentValue);                     //Resets health on canvas healthbar frame

    }

}
