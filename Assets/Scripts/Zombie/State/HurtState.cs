using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : IState
{
    private FSM stateManager;
    
    private EnemyParameter enemyParameter;
    
    public HurtState(FSM manager)
    {
        this.stateManager = manager;
        this.enemyParameter = manager.EnemyParameter;
    }
    
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}