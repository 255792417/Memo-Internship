using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : MonoBehaviour
{
    public Button backButton;
    public Button deleteButton;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        backButton.onClick.AddListener(BackButtonClickEvent);
        deleteButton.onClick.AddListener(DeleteButtonClickEvent);
    }

    private void BackButtonClickEvent()
    {
        gameObject.SetActive(false);
    }

    private void DeleteButtonClickEvent()
    {
    }
}
