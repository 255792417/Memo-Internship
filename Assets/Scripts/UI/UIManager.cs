using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    private static UIManager instance;
    private Transform _uiRoot;
    private Dictionary<string, string> pathDict;
    // Ԥ�Ƽ������ֵ�
    public Dictionary<string, GameObject> prefabDict;
    // �Ѵ򿪽��滺���ֵ�
    public Dictionary<string, BasePanel> panelDict;

    public static UIManager Instance { 
        get
        {
            if(instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    public Transform UIRoot
    {
        get
        {
            if(_uiRoot == null)
            {
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }

    private UIManager()
    {
        InitDicts();
    }

    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();
        pathDict = new Dictionary<string, string>()
        {
            {UIConst.SettingsPanel, "Menu/SettingsMenu" },
            {UIConst.InfoPanel, "Menu/InfoMenu" },
            {UIConst.AudioPanel, "Menu/AudioMenu" }
        };
    }

    // �򿪽���
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        if (panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("�����Ѵ�: " +  name);
            return null;
        }

        string path = "";
        if(! pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("���治����: " + name);
            return null;
        }

        GameObject panelPrefab = null;
        if(! prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = "Prefabs/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }

        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);

        panel.OpenPanel(name);
        return panel;
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("����δ��: " + name);
            return false;
        }
        panel.ClosePanel();
        return true;
    }
}

public class UIConst
{
    public const string SettingsPanel = "SettingsPanel";
    public const string InfoPanel = "InfoPanel";
    public const string AudioPanel = "AudioPanel";
}
