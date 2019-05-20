using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;        //Stop moving
        parent.Reset();
    }

    public void Update()
    {
        parent.Direction = (parent.MyStartPosition - parent.transform.position).normalized;     //Calculates return direction based on current position and start position
                                                                                                //Will currently straight line through walls and water to return position
                                                                                                //*Fix with path finding*

        parent.transform.position = Vector2.MoveTowards
            (parent.transform.position, parent.MyStartPosition, parent.Speed * Time.deltaTime);     //Moves Enemy from current position towards start position at propper speed

        float distance = Vector2.Distance(parent.MyStartPosition, parent.transform.position);        //Distance between start pos and current pos

        if (distance <= 0)  //If at start position
        {
            parent.ChangeState(new IdleState());        //Go back to Idle Anim
        }

    }
}
