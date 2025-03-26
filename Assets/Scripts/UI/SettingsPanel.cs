using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : BasePanel
{
    public Button continuePlaying;
    public Button infomations;
    public Button audios;
    public Button exitGame;

    private void Start()
    {
        continuePlaying.onClick.AddListener(ContinueButtonEvent);
        infomations.onClick.AddListener(InfoButtonEvent);
        audios.onClick.AddListener(AudiosButtonEvent);
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

    private void ExitButtonEvent()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }
    }
