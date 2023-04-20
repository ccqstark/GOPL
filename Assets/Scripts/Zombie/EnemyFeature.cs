using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class EnemyFeature : MonoBehaviour
{

    private FSM stateManager;
    private EnemyHealthBar enemyHealthBar;
    private ScoreSystem scoreSystemObj;
    private PlotSystem plotSystemObj;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = GetComponent<FSM>();
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        scoreSystemObj = GameObject.Find("UI/Score").GetComponent<ScoreSystem>();
        plotSystemObj = GameObject.Find("UI/Plot").GetComponent<PlotSystem>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    // 受到伤害
    public void TakeDamage(int damageValue)
    {
        if (stateManager.CurrenStateType == StateType.Death) return;
        // 扣除血量
        stateManager.EnemyParameter.CurrentHealth -= damageValue;
        // 更新血条UI
        enemyHealthBar.UpdateHealthBar(stateManager.EnemyParameter.CurrentHealth, 
            stateManager.EnemyParameter.MaxHealth);
        // 触发死亡
        if (stateManager.EnemyParameter.CurrentHealth <= 0)
        {
            enemyHealthBar.DisableBarUI();
            stateManager.TransitionState(StateType.Death);
            // 得分
            scoreSystemObj.AddScore(1000);
            // 击杀数量+1
            plotSystemObj.AddKillNum();
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
