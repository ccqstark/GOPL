using System;
using Db;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterSystem : MonoBehaviour
{
    // 输入框
    public TMP_InputField UsernameInput;
    public TMP_InputField NicknameInput;
    public TMP_InputField PswInput;
    public TMP_InputField PswConfirmInput;
    public TMP_Text Hint; // 提示文字
    public Button BackToMainMenuButton;
    public LoginSystem Login;
    
    public void DoRegister()
    {
        // 读取输入框内容
        var username = UsernameInput.text;
        var nickname = NicknameInput.text;
        var psw = PswInput.text;
        var pswConfirm = PswConfirmInput.text;
        if (!psw.Equals(pswConfirm))
        {
            Hint.text = "两次密码输入不一致";
            Hint.gameObject.SetActive(true);
            return;
        }
        // 检测用户名是否已被注册过
        string querySQL = $"SELECT * FROM user WHERE username = '{username}'";
        var dataResult = DbUtil.QueryData(querySQL);
        if (dataResult.Read())
        {
            Hint.text = "此账号名已被注册";
            Hint.gameObject.SetActive(true);
            return;
        }
        
        // 写入数据库
        string insertSQL = @$"INSERT INTO user (username, nickname, password) 
values ('{username}', '{nickname}', '{psw}')";
        DbUtil.InsertData(insertSQL);
        DbUtil.CloseDbConn();
        // 跳转到主菜单
        BackToMainMenuButton.onClick.Invoke();
        // 右下角显示用户昵称
        Login.ShowWelcome(nickname);
    }
    
}
