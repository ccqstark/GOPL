using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class EnemyFeature : MonoBehaviour
{

    private FSM stateManager;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = GetComponent<FSM>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageValue)
    {
        stateManager.EnemyParameter.HP -= damageValue;
        // 触发死亡
        if (stateManager.EnemyParameter.HP <= 0)
        {
            stateManager.TransitionState(StateType.Death);
        }
        
        // 概率触发受伤硬直动画
        if (ProbabilisticTrigger(stateManager.EnemyParameter.InjuryProbability))
        {
            stateManager.TransitionState(StateType.Hurt);
        }
    }
    
    // 概率函数
    public bool ProbabilisticTrigger(float probability)
    {
        Random random = new Random();
        double number = random.NextDouble();
        return number < probability;
    }

}
