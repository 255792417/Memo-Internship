using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : BasePanel
{
    public Button backButton;
    [SerializeField]
    private List<TextMeshProUGUI> mineNumTexts = new List<TextMeshProUGUI>();

    private void Start()
    {
        backButton.onClick.AddListener(BackButtonEvent);
        foreach(var item in mineNumTexts)
        {
            string targetName = item.name.Substring(0, item.name.Length - 6);
            item.text = MinesManager.Instance.GetMineScore(targetName).ToString();
        }

    }

    private void BackButtonEvent()
    {
        UIManager.Instance.ClosePanel(UIConst.InfoPanel);
        UIManager.Instance.OpenPanel(UIConst.SettingsPanel);
    }
}
