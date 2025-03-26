using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemoved = false;
    protected new string name;

    public virtual void OpenPanel(string name)
    {
        this.name = name;
        gameObject.SetActive(true);
    }

    public virtual void ClosePanel()
    {
        isRemoved = true;

        if (!string.IsNullOrEmpty(name) && UIManager.Instance != null && UIManager.Instance.panelDict != null)
        {
            // ÒÆ³ý»º´æ
            if (UIManager.Instance.panelDict.ContainsKey(name))
            {
                UIManager.Instance.panelDict.Remove(name);
            }
        }

        Destroy(gameObject);
    }

}
