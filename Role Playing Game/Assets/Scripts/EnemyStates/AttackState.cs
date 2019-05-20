using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Attack State
/// </summary>
public class AttackState : IState       
{
    //Reference to State's parent (Enemy)
    private Enemy parent;

    private float attackCooldown = 2;       //Cooldown for enemy attacks

    private float extraRange = .1f;         //Adds additional range onto enemy's radius after attack happens; This makes it so the full attack animation goes through

    public void Enter(Enemy parent)
    {
        this.parent = parent;       //References parent
    }

    public void Exit()
    {
        //Debug.Log("Exit AttackState");                   //Testing state functionality
    }

    public void Update()
    {
        //Debug.Log("Enemy Attacking");                   //Testing state functionality


        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)       //If have waited past cooldown time, and Enemy not attacking; It can attack
        {
            parent.MyAttackTime = 0;            //Resets attack CD

            parent.StartCoroutine(Attack());    //Starts attack Coroutine
        }

        if (parent.MyTarget != null)      //Check for Enemy having target
        {
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);   //Distance between enemy and player

            if(distance >= parent.MyAttackRange+extraRange && !parent.IsAttacking)            //If Enemy outside of attack range and is not attacking (must complete attack animation first)
            {
                parent.ChangeState(new FollowState());      //Transition: To FollowState
            }
 
        }
        else
        {
            parent.ChangeState(new IdleState());  //If enemy loses target; Transition: To IdleState
        }
    }

    /// <summary>
    /// Enemy Attack Coroutine
    /// </summary>
    /// <returns></returns>
    public IEnumerator Attack()
    {
        parent.IsAttacking = true;              //Property that accesses Characters IsAttacking   

        parent.MyAnimator.SetTrigger("attack");     //Property that accesses Characters Animator

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);       //Waits for attack animation to finish before resetting IsAttacking. (Located on layer 2 of Animator)

        parent.IsAttacking = false;           //Resets attack animation
    }

}
