using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        StartNewGame();
    }

    // ��ʼ����Ϸ
    public void StartNewGame()
    {
        // ���ؿմ浵��������������
        SaveManager.instance.LoadEmptySave();
    }

    // ������Ϸ
    public void LoadGame()
    {
        SaveManager.instance.LoadGame();
    }

    // ������Ϸ
    public void SaveGame()
    {
        SaveManager.instance.SaveGame();
    }

    // �˳���Ϸ
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                StartNewGame ();
        #else
                Application.Quit();
        #endif
    }
}
