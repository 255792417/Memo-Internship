using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Sprite[] digitSprites; // 0-9的数字精灵
    [SerializeField] private Image[] digitImages;   // 固定5个Image组件

    private int score = 0;

    private void Start()
    {
        // 确保有5个数字位置
        if (digitImages.Length != 5)
        {
            Debug.LogError("需要正好5个数字图像位置!");
        }

        // 初始化显示为00000
        UpdateScoreDisplay();
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        // 将分数转换为5位字符串，不足的用0填充
        string scoreText = score.ToString().PadLeft(5, '0');

        // 如果超过5位，只取最后5位
        if (scoreText.Length > 5)
        {
            scoreText = scoreText.Substring(scoreText.Length - 5);
        }

        // 设置每个位置的数字图像
        for (int i = 0; i < 5; i++)
        {
            int digitValue = int.Parse(scoreText[i].ToString());
            digitImages[i].sprite = digitSprites[digitValue];
            digitImages[i].gameObject.SetActive(true); // 确保所有位置都显示
        }
    }
}