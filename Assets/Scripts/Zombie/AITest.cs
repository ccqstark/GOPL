using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    idle,
    run,
    attack
}

public class AITest : MonoBehaviour
{
    // 状态
    public EnemyState CurrentState = EnemyState.idle;
    // 动画控制器
    private Animator ani;
    // 玩家
    private Transform player;
    // 导航
    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        ani.SetFloat("Distance", distance);
        Debug.Log("当前距离：" + distance);
        
        // 判断状态
        switch (CurrentState)
        {
            case EnemyState.idle:
                if (distance > 2 && distance <= 6)
                {
                    CurrentState = EnemyState.run;
                }
                // 导航停止
                agent.isStopped = true;
                break;
            case EnemyState.run:
                if (distance > 6)
                {
                    CurrentState = EnemyState.idle;
                }
                // 导航开始
                agent.isStopped = false;
                agent.SetDestination(player.position);
                if (distance < 2)
                {
                    agent.isStopped = true;
                    CurrentState = EnemyState.attack;
                }
                break;
            case EnemyState.attack:
                if (distance >= 2)
                {
                    CurrentState = EnemyState.idle;
                }
                break;
        }

    }
}
