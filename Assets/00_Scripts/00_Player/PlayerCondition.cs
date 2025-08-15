using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    SettingSound sound;
    public UICondition uiCondition;
    public WeatherSystem weatherSystem;
    public GameObject dieWindow;

    protected Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition thirst { get { return uiCondition.thirst; } }
    Condition bodyTemp { get { return uiCondition.bodyTemp; } }

    public float noHungerHealthDecay;
    public float lowBodyTempHealthDecay;

    public event Action onTakeDamage;

    private void Start()
    {
        weatherSystem = FindObjectOfType<WeatherSystem>();
        sound = FindObjectOfType<SettingSound>();
    }
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        thirst.Subtract(thirst.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }
        if(thirst.curValue == 0f)
        {
            CharacterManager.Instance.Player.controller.moveSpeed = 1.0f;
        }
        else
        {
            CharacterManager.Instance.Player.controller.moveSpeed = 2.5f;
        }




        if (health.curValue == 0f)
        {
            Die();
        }

        if (weatherSystem.isRainning)
        {
            bodyTemp.Subtract(bodyTemp.passiveValue * Time.deltaTime);
            if(bodyTemp.passiveValue == 0f)
            {
                health.Subtract(lowBodyTempHealthDecay * Time.deltaTime);
            }
        }


    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("죽었다.");
        dieWindow.SetActive(true);
        sound.EnvironmentStop();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }

    public void CallTakeDamageEvent()
    {
        onTakeDamage?.Invoke();
    }
}

