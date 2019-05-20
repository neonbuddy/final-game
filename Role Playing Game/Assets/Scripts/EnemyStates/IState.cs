using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the functions that the enemy must have to work properly;
/// </summary>
public interface IState
{

    void Enter(Enemy parent);   //Used to prepare enemy states for execution

    void Update();      //Enemy must update in all states besides Idle

    void Exit();        //Changes values back to default after state is exited




}
