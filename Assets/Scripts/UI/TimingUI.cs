using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimingUI : MonoBehaviour
{
    private float startTime; // 开始时间
    public TMP_Text TimingText; // 时间显示文本
    
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");

        TimingText.text = minutes + " : " + seconds;
    }
    
}