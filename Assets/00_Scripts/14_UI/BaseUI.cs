using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    protected abstract UIKey uiKey { get; }

    protected virtual void Start()
    {
        Initialize();
        SetUIActive(false);
    }

    protected virtual void Initialize()
    {
        UIManager.Instance.SetUI(uiKey, this);
    }

    public virtual void SetUIActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

}