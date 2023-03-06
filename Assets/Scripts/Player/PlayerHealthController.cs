using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    
    public int CurrentHealth; // 当前血量

    public int MaxHealth; // 最大血量

    public Image BloodSplatterImage; // 受伤血迹图片

    void Start()
    {
        CurrentHealth = MaxHealth;
        BloodSplatterImage.enabled = false;
    }
    
    void Update()
    {
        // 血量小于50%时屏幕出现血迹
        BloodSplatterImage.enabled = CurrentHealth <= MaxHealth * 0.5f;
    }

    public void TakeDamage(int damageValue)
    {
        CurrentHealth -= damageValue;
        if (CurrentHealth <= 0)
        {
            // 触发死亡
            Debug.Log("玩家死亡");
        }
    }
}
