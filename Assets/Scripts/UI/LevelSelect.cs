using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void PlayLevelOne()
    {
        SceneManager.LoadScene("LevelOne");
    }
    
    public void PlayLevelTwo()
    {
        SceneManager.LoadScene("LevelTwo");
    }
    
    public void PlayLevelThree()
    {
        SceneManager.LoadScene("LevelThree");
    }

}
