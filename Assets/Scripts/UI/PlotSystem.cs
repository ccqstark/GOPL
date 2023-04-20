using System;
using UnityEngine;

// 剧情任务系统
public class PlotSystem : MonoBehaviour
{
    [Header("击杀敌人模块")]
    public int EnemyTotalNum;
    public int EnemyAlreadyKillNum;

    [Header("剧情物品模块")] 
    public int MissionKeyItemTotalNum;
    public int MissionKeyAlreadyGet;

    public GameWin GameWinObj;
    
    private void Update()
    {
        if (EnemyAlreadyKillNum == EnemyTotalNum
            && MissionKeyAlreadyGet == MissionKeyItemTotalNum)
        {
            // 任务成功
            GameWinObj.DoGameWin();
        }
    }

}
