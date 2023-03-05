﻿using System;
using UnityEngine;

/*
 * 敌人参数
 */
[Serializable]
public class EnemyParameter
{
    public int HP; // 血量

    public float MoveSpeed; // 行走速度

    public float ChaseSpeed; // 追击速度

    public float IdleTime; // 每次原地空闲事件

    public float PatrolDistance; // 每次巡逻移动距离

    public float PatrolMaxTime; // 每次巡逻时间上限
    
    public float HatredAreaRadius; // 仇恨范围半径

    public float AttackDistance; // 攻击距离
    
    public Transform InitialPoint; // 初始位置

    public Animator Animator; // 动画控制器

    public float DistanceFromPlayer; // 与玩家的距离

    public float TurnToSmoothTime; // 转向平滑参数

    public float InjuryProbability; // 受伤概率
}