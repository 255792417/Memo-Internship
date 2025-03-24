using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Sprite[] digitSprites; // 0-9�����־���
    [SerializeField] private Image[] digitImages;   // �̶�5��Image���

    private int score = 0;

    private void Start()
    {
        // ȷ����5������λ��
        if (digitImages.Length != 5)
        {
            Debug.LogError("��Ҫ����5������ͼ��λ��!");
        }

        // ��ʼ����ʾΪ00000
        UpdateScoreDisplay();
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        // ������ת��Ϊ5λ�ַ������������0���
        string scoreText = score.ToString().PadLeft(5, '0');

        // �������5λ��ֻȡ���5λ
        if (scoreText.Length > 5)
        {
            scoreText = scoreText.Substring(scoreText.Length - 5);
        }

        // ����ÿ��λ�õ�����ͼ��
        for (int i = 0; i < 5; i++)
        {
            int digitValue = int.Parse(scoreText[i].ToString());
            digitImages[i].sprite = digitSprites[digitValue];
            digitImages[i].gameObject.SetActive(true); // ȷ������λ�ö���ʾ
        }
    }
}