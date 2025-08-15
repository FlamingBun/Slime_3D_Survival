using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    private Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();        
    }

    private void Start()
    {
        btn.onClick.AddListener(SoundManager.Instance.PlaySFXSource);
        btn.onClick.AddListener(OpenSettingUI);
    }

    private void OpenSettingUI()
    {
        UIManager.Instance.OpenUI(UIKey.SoundUI);
    }
}
