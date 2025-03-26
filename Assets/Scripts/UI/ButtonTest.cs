using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public Button m_Button;
    public TextMeshProUGUI m_Text;
    void Start()
    {
        m_Button.onClick.AddListener(ButtonOnClickEvent);
    }
    public void ButtonOnClickEvent()
    {
        UIManager.Instance.OpenPanel(UIConst.SettingsPanel);
        
    }
}