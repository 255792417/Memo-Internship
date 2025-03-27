using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    public Button backButton;
    public Button deleteButton;

    void Start()
    {
        backButton.onClick.AddListener(BackButtonClickEvent);
        deleteButton.onClick.AddListener(DeleteButtonClickEvent);
    }

    private void BackButtonClickEvent()
    {
        UIManager.Instance.ClosePanel(UIConst.BagPanel);
    }

    private void DeleteButtonClickEvent()
    {
    }
}
