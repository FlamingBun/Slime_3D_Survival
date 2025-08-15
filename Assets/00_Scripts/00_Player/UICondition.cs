using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICondition : MonoBehaviour
{
    [Header("Condition")]
    public Condition health;
    public Condition hunger;
    public Condition stamina;
    public Condition thirst;
    public Condition bodyTemp;

    [Header("Condition Bar")]
    public Image hungerBar;
    public Image staminaBar;
    public Image thirstBar;
    public Image bodyTempBar;
    public Image bodyTempFilled;

    [Header("Condition Bar Color")]
    private Color emptyColor = new Color(255 / 255f, 50 / 255f, 50 / 255f, 200 / 255f);
    private Color halfColor = new Color(255 / 255f, 255 / 255f, 50 / 255f, 200 / 255f);
    private Color fullColor = new Color(50 / 255f, 255 / 255f, 50 / 255f, 200 / 255f);

    private Color lowerTemp = new Color(50 / 255f, 255 / 255f, 255 / 255f, 200 / 255f);
    private Color lowestTemp = new Color(50 / 255f, 50 / 255f, 255 / 255f, 200 / 255f);

    public TextMeshProUGUI bodyTempText;



    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
        CharacterManager.Instance.Player.combat.uiCondition = this;
        bodyTempText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        LerpColor(hungerBar.fillAmount, hungerBar);
        LerpColor(staminaBar.fillAmount, staminaBar);
        LerpColor(thirstBar.fillAmount, thirstBar);
        BodyTempLerpColor(bodyTempFilled.fillAmount, bodyTempBar);
        bodyTempText.text = (bodyTemp.curValue+26.5f).ToString("N1")+"℃";

    }

    private void LerpColor(float amount, Image condition)
    {
        if (amount > 0.5f)
        {
            float lerpT = (amount - 0.5f) * 2f;
            condition.color = Color.Lerp(halfColor, fullColor, lerpT);
        }
        else
        {
            float lerpT = amount * 2f;
            condition.color = Color.Lerp(emptyColor, halfColor, lerpT);
        }
    }

    private void BodyTempLerpColor(float amount, Image condition)
    {
        if (amount > 0.75f)
        {
            float lerpT = (amount - 0.75f) / 0.25f;
            condition.color = Color.Lerp(halfColor, emptyColor, lerpT);
        }
        else if(amount > 0.5f)
        {
            float lerpT = (amount - 0.5f) / 0.25f;
            condition.color = Color.Lerp(fullColor, halfColor, lerpT);
        }
        else if (amount > 0.25f)
        {
            float lerpT = (amount - 0.25f) / 0.25f;
            condition.color = Color.Lerp(lowerTemp, fullColor, lerpT);
        }
        else
        {
            float lerpT = amount / 0.25f;
            condition.color = Color.Lerp(lowestTemp, lowerTemp, lerpT);
        }
    }
}
