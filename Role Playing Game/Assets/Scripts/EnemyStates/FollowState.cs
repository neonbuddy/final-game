using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Enemy Follow State 
/// Transitions: From=>Idle : To=>Attack
/// </summary>
class FollowState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        //Debug.Log("Exit FollowState");         //Testing state functionality
    }

    public void Update()
    {
        //Debug.Log("Enter FollowState");       //Testing state functionality

        //If there is target Follow
        if (parent.MyTarget != null)   
        {
            //Generates direction based on targets position - enemy position
            parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized;

            //Move enemy towards target
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MyTarget.position, parent.Speed * Time.deltaTime);


            //*To=> AttackState*

            //Check enemy and player distance
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);   

            if (distance <= parent.MyAttackRange)           //If distance within AggroRange
            {
                parent.ChangeState(new AttackState());      //Change to AttackState
            }
        }
        if (!parent.InRange)        //If player is not in range of Enemy
        {
            parent.ChangeState(new ReturnState());   //Return to start position
        }
      
    }
}
