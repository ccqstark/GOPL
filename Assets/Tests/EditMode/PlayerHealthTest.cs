using System;
using UnityEngine;

public class PlayerHealthTest
{
    public int CurrentHealth { get; set; } // 当前血量

    public int MaxHealth = 100; // 最大血量

    // 受到伤害
    public void TakeDamage(int damageValue)
    {
        // 扣除生命值
        CurrentHealth -= damageValue;
        if (CurrentHealth <= 0)
        {
            Console.WriteLine("玩家死亡");
        }
    }

    // 增加生命值
    public void AddHealth(float addValue)
    {
        CurrentHealth += (int)addValue;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
}