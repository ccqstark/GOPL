using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    public TMP_Text Score;
    public TMP_Text TimeUsed;
    public TMP_Text KillEnemyNum;

    public ScoreSystem ScoreSystemObj;
    public TimingSystem TimingSystemObj;
    public PlotSystem PlotSystemObj;

    private bool IsWin = false;
    
    public void DoGameWin()
    {
        if (IsWin) return;
        // 停止游戏
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // 本局结算
        Score.text = ScoreSystemObj.GetScore() + "";
        TimeUsed.text = TimingSystemObj.GetTimeUsedString();
        KillEnemyNum.text = PlotSystemObj.EnemyAlreadyKillNum + "";
        gameObject.SetActive(true);
        IsWin = true;
    }

    // 下一关
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    // 回到主菜单
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
