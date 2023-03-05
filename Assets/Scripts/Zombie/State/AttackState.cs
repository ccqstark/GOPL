﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private FSM stateManager;
    
    private EnemyParameter enemyParameter;

    public AttackState(FSM manager)
    {
        this.stateManager = manager;
        this.enemyParameter = manager.EnemyParameter;
    }
    
    public void OnEnter()
    {
        enemyParameter.Animator.SetBool("Attack", true);
        // 面朝玩家攻击
        stateManager.TurnToPlayer();
    }

    public void OnUpdate()
    {
        // 超出攻击范围则继续追击
        if (enemyParameter.DistanceFromPlayer > enemyParameter.AttackDistance)
        {
            stateManager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {
        enemyParameter.Animator.SetBool("Attack", false);
    }
}