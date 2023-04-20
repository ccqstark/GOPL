using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public LoginSystem Login;
    public Button GoToLogin;

    
    private void Awake()
    {
        // 避免从游戏中退出后没有动画和声音
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    // 开始游戏，默认进行关卡一
    public void PlayGame()
    {
        // 判断是否登录
        if (Login.IsLogin)
        {
            SceneManager.LoadScene("LevelOne");
        }
        else
        {
            GoToLogin.onClick.Invoke();
        }
    }

    // 退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }
}