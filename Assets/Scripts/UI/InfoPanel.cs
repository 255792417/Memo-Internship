using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : BasePanel
{
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(BackButtonEvent);
    }

    private void BackButtonEvent()
    {
        UIManager.Instance.ClosePanel(UIConst.InfoPanel);
        UIManager.Instance.OpenPanel(UIConst.SettingsPanel);
    }
}
