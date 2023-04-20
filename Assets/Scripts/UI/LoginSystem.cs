using Db;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{
    public TMP_InputField UsernameInput;
    public TMP_InputField PswInput;
    public Button GoToLogin;
    public TMP_Text WelcomeText;
    public Button BackToMainMenu;
    public TMP_Text Hint; // 提示文字

    // 当前是否已经登陆以及登陆用户信息
    public bool IsLogin;
    public string NowLoginUsername;
    public string NowLoginNickname;
    
    public void DoLogin()
    {
        // 获取输入
        var username = UsernameInput.text;
        var psw = PswInput.text;
        // 在数据库中查找
        string querySQL = @$"SELECT * FROM user WHERE username = '{username}' AND
password = '{psw}'";
        var dataResult = DbUtil.QueryData(querySQL);
        if (dataResult.Read())
        {
            // 登录成功
            IsLogin = true;
            NowLoginUsername = username;
            NowLoginNickname = dataResult[2].ToString();
            // 跳转到主菜单
            BackToMainMenu.onClick.Invoke();
            // 右下角显示用户昵称
            ShowWelcome(dataResult[2].ToString());
        }
        else
        {
            // 密码错误
            Hint.gameObject.SetActive(true);
        }
        DbUtil.CloseDbConn();
    }

    public void ShowWelcome(string nickname)
    {
        GoToLogin.gameObject.SetActive(false);
        WelcomeText.gameObject.SetActive(true);
        WelcomeText.text = "欢迎你，" + nickname;
    }
    
}