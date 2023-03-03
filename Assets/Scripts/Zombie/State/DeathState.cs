using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState
{
    private FSM stateManager;
    
    private EnemyParameter enemyParameter;
    
    public DeathState(FSM manager)
    {
        this.stateManager = manager;
        this.enemyParameter = manager.EnemyParameter;
    }
    
    public void OnEnter()
    {
        enemyParameter.Animator.SetBool("Death", true);
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        enemyParameter.Animator.SetBool("Death", false);
    }
}