using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

// 剧情任务系统
public class PlotSystem : MonoBehaviour
{
    [Header("击杀敌人模块")]
    public int EnemyAlreadyKillNum;
    public int EnemyTotalNum;
    public TMP_Text EnemyNumText;

    [Header("剧情物品模块")]
    public int MissionKeyAlreadyGet;
    public int MissionKeyTotalNum;
    public TMP_Text MissionKeyText;

    public GameWin GameWinObj;

    private void Start()
    {
        EnemyNumText.text = EnemyAlreadyKillNum + " / " + EnemyTotalNum;
        MissionKeyText.text = MissionKeyAlreadyGet + " / " + MissionKeyTotalNum;
    }

    private void Update()
    {
        if (EnemyAlreadyKillNum == EnemyTotalNum
            && MissionKeyAlreadyGet == MissionKeyTotalNum)
        {
            // 任务成功
            GameWinObj.DoGameWin();
        }
    }

    public void AddKillNum()
    {
        EnemyAlreadyKillNum++;
        EnemyNumText.text = EnemyAlreadyKillNum + " / " + EnemyTotalNum;
    }

    public void AddMissionNum()
    {
        MissionKeyAlreadyGet++;
        MissionKeyText.text = MissionKeyAlreadyGet + " / " + MissionKeyTotalNum;
    }
    
}
