using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : BasePanel
{
    public Button continuePlaying;
    public Button infomations;
    public Button audios;
    public Button save;
    public Button load;
    public Button exitGame;

    private void Start()
    {
        continuePlaying.onClick.AddListener(ContinueButtonEvent);
        infomations.onClick.AddListener(InfoButtonEvent);
        audios.onClick.AddListener(AudiosButtonEvent);
        save.onClick.AddListener(SaveButtonEvent);
        load.onClick.AddListener(LoadButtonEvent);
        exitGame.onClick.AddListener(ExitButtonEvent);
    }

    private void ContinueButtonEvent()
    {
        UIManager.Instance.ClosePanel(UIConst.SettingsPanel);
    }

    private void InfoButtonEvent()
    {
        UIManager.Instance.ClosePanel(UIConst.SettingsPanel);
        UIManager.Instance.OpenPanel(UIConst.InfoPanel);
    }

    private void AudiosButtonEvent()
    {
        UIManager.Instance.ClosePanel(UIConst.SettingsPanel);
        UIManager.Instance.OpenPanel(UIConst.AudioPanel);
    }

    private void SaveButtonEvent()
    {
        GameManager.instance.SaveGame();
    }

    private void LoadButtonEvent()
    {
        GameManager.instance.LoadGame();
    }

    private void ExitButtonEvent()
    {
        GameManager.instance.QuitGame();
    }
    }
