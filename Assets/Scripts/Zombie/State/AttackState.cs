using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private FSM stateManager;

    private EnemyParameter enemyParameter;

    private Transform enemyTransform;

    private AnimatorStateInfo animatorInfo;

    private int attackTimes; // 攻击次数

    private Vector3 attackDetectDirection; // 攻击检测方向

    public AttackState(FSM manager)
    {
        this.stateManager = manager;
        this.enemyParameter = manager.EnemyParameter;
        this.enemyTransform = stateManager.GetEnemyTransform();
    }

    public void OnEnter()
    {
        enemyParameter.Animator.SetBool("Attack", true);
        stateManager.TurnToPlayer();
        attackDetectDirection = enemyTransform.forward;
    }

    public void OnUpdate()
    {
        // 超出攻击范围则继续追击
        if (enemyParameter.DistanceFromPlayer > enemyParameter.AttackDistance)
        {
            stateManager.TransitionState(StateType.Chase);
        }

        // 获取动画播放进度数据
        animatorInfo = enemyParameter.Animator.GetCurrentAnimatorStateInfo(0);
        double playingProgressNumber = animatorInfo.normalizedTime;
        double playingIntegerPart = Math.Truncate(playingProgressNumber); // 播放进度整数部分
        double playingDecimalPart = playingProgressNumber - playingIntegerPart; // 播放进度小数部分

        // 每次攻击前都调整下攻击方向
        if (playingDecimalPart <= 0.1f)
        {
            stateManager.TurnToPlayer();
            attackDetectDirection = enemyTransform.forward;
        }

        // debug: 发射出一条射线检测是否攻击到了敌人
#if UNITY_EDITOR
        Debug.DrawRay(enemyTransform.position + Vector3.up * enemyParameter.Height * 0.75f,
            attackDetectDirection, Color.red);
#endif

        // 检测攻击动画播放进度到大概40%时，进行攻击检测和造成伤害
        if (animatorInfo.IsName("Base.Attack") && playingDecimalPart >= 0.4 &&
            attackTimes < playingIntegerPart + 1)
        {
            // 此变量用于保证每个攻击动画播放循环，只能检测一次攻击
            attackTimes++;
            // 当射线检测到玩家时，伤害才有效
            RaycastHit hit;
            if (Physics.Raycast(enemyTransform.position + Vector3.up * enemyParameter.Height * 0.75f,
                    attackDetectDirection, out hit, enemyParameter.AttackDistance)
                && hit.collider.CompareTag("Player"))
            {
                // 调用玩家血量模块进行扣血
                hit.collider.GetComponent<PlayerHealthController>().TakeDamage(20);
            }
        }
    }

    public void OnExit()
    {
        enemyParameter.Animator.SetBool("Attack", false);
        attackTimes = 0;
    }
}