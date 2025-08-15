using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    private float startTime;
    private float timeRate;
    public Vector3 noon; // Vector 90 0 0

    [Header("Sun")]
    [SerializeField] private GameObject sunObj;
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    [SerializeField] private GameObject moobObj;
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultyplier;


    [Header("Skybox")]
    public Material skyboxMaterial;
    public float maxExposure = 1.0f;
    public float minExposure = 0.15f;
    public AnimationCurve exposureCurve;

    private bool inNight = false;
    public bool InNight { get { return inNight; } }



    void Start()
    {
        //RandomSunMoon();
        EnemyManager.Instance.dayNightCycle = this;
        timeRate = 1.0f / fullDayLength;
        time = startTime;
        skyboxMaterial.SetFloat("_Exposure", 1.0f);
    }

    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time); // 주변광
        RenderSettings.reflectionIntensity = reflectionIntensityMultyplier.Evaluate(time); // 반사광

        float exposureValue = Mathf.Lerp(maxExposure, minExposure, exposureCurve.Evaluate(time));
        skyboxMaterial.SetFloat("_Exposure", exposureValue);

        inNight = sun.intensity <= 0.01f;
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity <= 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
        //if (go == moon.gameObject)
        //{
        //    inNight = true;
        //}
        //else
        //{
        //    inNight = false;
        //}
    }

    //private void RandomSunMoon() 밤낮 랜덤 시작
    //{
    //    startTime = UnityEngine.Random.Range(0f, 1f);
    //    if (startTime >= 0.2f && startTime <= 0.8f)
    //    {
    //        sunObj.SetActive(true);
    //        moobObj.SetActive(false);
    //        inNight = false;
    //    }
    //    else
    //    {
    //        sunObj.SetActive(false);
    //        moobObj.SetActive(true);
    //        inNight = true;
    //    }
    //}
}