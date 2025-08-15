using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    [Header("Light")]
    public Light light;

    [Header("Rain")]
    SettingSound sound;
    public ParticleSystem rainEffect;
    public float sectionDuration = 30f;
    public float minRainyDuration;
    public float maxRainyDuration;

    [Header("Light in Rainning")]
    public float rainyIntensity = 0.1f;
    public float clearIntensity = 0.4f;
    public float lerpSpeed = 1f;
    private float targetIntensity;

    [Header("Skybox")]
    public Material skyBoxMaterial;

    private Color cleanColor = new Color(128 / 255f, 128 / 255f, 128 / 255f, 128 / 255f);
    private Color rainColor = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
    public float colorChangeSpeed = 0.2f;

    public bool isRainning = false;

    void Start()
    {
        sound = FindObjectOfType<SettingSound>();
        sound.EnvironmentStop();
        rainEffect = GetComponentInChildren<ParticleSystem>();
        RenderSettings.skybox = skyBoxMaterial;
        skyBoxMaterial.SetColor("_Tint", cleanColor);

        rainEffect.Stop();
        targetIntensity = clearIntensity;
        StartCoroutine(Rainny());
    }

    void Update()
    {
        light.intensity = Mathf.Lerp(light.intensity, targetIntensity, Time.deltaTime * 2 * lerpSpeed);


        skyBoxMaterial = RenderSettings.skybox;
        if (isRainning)
        {
            Color currentColor = skyBoxMaterial.GetColor("_Tint");
            Color targetColor = rainColor;
            Color changerain = ColorMoveTowards(currentColor, targetColor, colorChangeSpeed * Time.deltaTime);
            skyBoxMaterial.SetColor("_Tint", changerain);
        }
        else
        {
            Color currentColor = skyBoxMaterial.GetColor("_Tint");
            Color targetColor = cleanColor;
            Color changerain = ColorMoveTowards(currentColor, targetColor, colorChangeSpeed * Time.deltaTime);
            skyBoxMaterial.SetColor("_Tint", changerain);
        }
    }

    Color ColorMoveTowards(Color current, Color target, float maxDelta)
    {
        float r = Mathf.MoveTowards(current.r, target.r, maxDelta);
        float g = Mathf.MoveTowards(current.g, target.g, maxDelta);
        float b = Mathf.MoveTowards(current.b, target.b, maxDelta);
        float a = Mathf.MoveTowards(current.a, target.a, maxDelta);
        return new Color(r, g, b, a);
    }

    IEnumerator Rainny()
    {
        while (true)
        {
            float rainyStartTime = Random.Range(0f, sectionDuration);
            float rainyDuration = Random.Range(minRainyDuration, maxRainyDuration);

            if (rainyStartTime + 10 <= sectionDuration)
            {
                yield return new WaitForSeconds(rainyStartTime);
                StartRainy();
                yield return new WaitForSeconds(rainyDuration);
                EndRainy();
                yield return new WaitForSeconds(sectionDuration - rainyStartTime - rainyDuration);
            }
            else
            {
                Debug.Log("이번 60초는 비 안내림");
                yield return new WaitForSeconds(sectionDuration);
            }
        }
    }

    private void StartRainy()
    {
        isRainning = true;
        targetIntensity = rainyIntensity;
        rainEffect.Play();
        sound.EnvironmentPlay();
    }

    private void EndRainy()
    {
        isRainning = false;
        targetIntensity = clearIntensity;
        rainEffect.Stop();
        sound.EnvironmentStop();
    }

}
