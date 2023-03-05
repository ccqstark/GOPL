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
    
    public AttackState(FSM manager)
    {
        this.stateManager = manager;
        this.enemyParameter = manager.EnemyParameter;
        this.enemyTransform = stateManager.GetEnemyTransform();
    }
    
    public void OnEnter()
    {
        enemyParameter.Animator.SetBool("Attack", true);
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
        double integerPart = Math.Truncate(playingProgressNumber); // 播放进度整数部分
        double decimalPart = playingProgressNumber - integerPart; // 播放进度小数部分

        if (decimalPart <= 0.1f)
        {
            // 面朝玩家攻击
            stateManager.TurnToPlayer();
        }
        
        // 从胸部发射出一条射线检测是否攻击到了敌人
#if UNITY_EDITOR      
        Debug.DrawRay(enemyTransform.position + Vector3.up * enemyParameter.Height * 0.5f,
            enemyTransform.forward, Color.red);
#endif
        // 如果碰到了玩家
        RaycastHit hit;
        if (Physics.Raycast(enemyTransform.position + Vector3.up * enemyParameter.Height * 0.5f,
                enemyTransform.forward, out hit, enemyParameter.AttackDistance) 
            && hit.collider.CompareTag("Player")) {
            // 检测播放进度来判断是否造成伤害
            if (animatorInfo.IsName("Base.Attack") && decimalPart >= 0.4  &&
                attackTimes < integerPart + 1) {
                // 扣除玩家的血量
                //hit.collider.GetComponent<PlayerHealth>().TakeDamage(10);
                attackTimes++;
                Debug.Log("攻击了玩家" + attackTimes + "次");
            }
        }
    }

    public void OnExit()
    {
        enemyParameter.Animator.SetBool("Attack", false);
        attackTimes = 0;
    }
}