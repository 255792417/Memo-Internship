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

    // 开始新游戏
    public void StartNewGame()
    {
        // 加载空存档来重置所有数据
        SaveManager.instance.LoadEmptySave();
    }

    // 继续游戏
    public void LoadGame()
    {
        SaveManager.instance.LoadGame();
    }

    // 保存游戏
    public void SaveGame()
    {
        SaveManager.instance.SaveGame();
    }

    // 退出游戏
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
