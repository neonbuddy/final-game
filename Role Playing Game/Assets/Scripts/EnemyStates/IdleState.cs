using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class IdleState : IState
{
    private Enemy parent;       //reference to parent: Enemy

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.parent.Reset();            //Resets Enemy stats
    }

    public void Exit()
    {
        //Debug.Log("Exit IdleState");                   //Testing state functionality
    }

    public void Update()    //parent is reference to Enemy
    {
        //Debug.Log("Enter IdleState");                   //Testing state functionality

        //When there is a target; change into follow state
        if (parent.MyTarget != null)      
        {
            parent.ChangeState(new FollowState());  
        }



        
    }
}
