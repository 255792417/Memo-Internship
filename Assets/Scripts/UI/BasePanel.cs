using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour //UI面板基类
{
    protected bool isRemoved = false;
    protected new string name;

    // 打开面板
    public virtual void OpenPanel(string name)
    {
        this.name = name;
        gameObject.SetActive(true);
    }

    // 关闭面板
    public virtual void ClosePanel()
    {
        isRemoved = true;

        if (!string.IsNullOrEmpty(name) && UIManager.Instance != null && UIManager.Instance.panelDict != null)
        {
            // 移除缓存
            if (UIManager.Instance.panelDict.ContainsKey(name))
            {
                UIManager.Instance.panelDict.Remove(name);
            }
        }

        Destroy(gameObject);
    }

}
