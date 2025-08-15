using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxBlender : MonoBehaviour
{
    DayNightCycle dayNightCycle;

    [Header("Blended Skybox Material")]
    public Material blendedSkyboxMaterial;

    [Header("Blend Settings")]
    public float blendSpeed;

    private float blend = 0f;
    private bool fadingToB = true;

    public float skyRotationSpeed = 2f;

    void Start()
    {
        dayNightCycle = FindAnyObjectByType<DayNightCycle>();
        RenderSettings.skybox = blendedSkyboxMaterial;
        blendSpeed = 2/ dayNightCycle.fullDayLength;
    }

    void Update()
    {
        float skyAngle = Time.time * skyRotationSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", skyAngle);

        // Blend 값 변경
        blend += (fadingToB ? 1 : -1) * blendSpeed * Time.deltaTime;
        blend = Mathf.Clamp01(blend);

        blendedSkyboxMaterial.SetFloat("_Blend", blend);

        // 방향 전환 (완전히 바뀌면 반대로)
        if (blend == 1f || blend == 0f)
        {
            fadingToB = !fadingToB;
        }
    }
}
