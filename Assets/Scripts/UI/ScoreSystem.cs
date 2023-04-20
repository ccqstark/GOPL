using System;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private int score; // 本局得分
    private int speed = 50; // 分数变化速度
    public TMP_Text ScoreText; // 分数显示文本
    public int GetScore() => score;
    
    private void Start()
    {
        score = 0;
    }

    private void Update()
    {
        int currentScore = int.Parse(ScoreText.text);
        if (currentScore == score) return;
        int updateScore = (int) Mathf.MoveTowards(currentScore, score, speed);
        ScoreText.text = updateScore.ToString();
    }

    public void AddScore(int value)
    {
        score += value;
    }

}