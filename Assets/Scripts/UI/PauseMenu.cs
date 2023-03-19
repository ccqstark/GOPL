using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        
        public static bool IsPaused = false;
        
        public GameObject PausePanel;
        
        private void Awake()
        {
            // 隐藏鼠标并锁定
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused)
                {
                    KeepOnGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
        
        private void PauseGame()
        {
            IsPaused = true;
            // 把 time scale 设置为0就可以暂停游戏
            Time.timeScale = 0f;
            // 显示并解锁鼠标
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // 显示暂停面板
            PausePanel.SetActive(true);
        }

        public void KeepOnGame()
        {
            IsPaused = false;
            // 恢复游戏
            Time.timeScale = 1f;
            // 隐藏并锁定鼠标
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            // 隐藏暂停面板
            PausePanel.SetActive(false);
        }

        // 回到主菜单
        public void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}