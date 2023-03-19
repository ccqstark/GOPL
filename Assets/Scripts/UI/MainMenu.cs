using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        // 开始游戏，默认进行关卡一
        public void PlayGame()
        {
            SceneManager.LoadScene("LevelTwo");
        }

        // 退出游戏
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}