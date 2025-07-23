using System.Collections;
using UnityEngine;



public class LightManager : MonoBehaviour
{
    [SerializeField, Range(1, 24)] private float timeOfDay;
    [SerializeField, Range(0, 10)] private float daySpeed;
    [SerializeField] private Light directLight;
    [SerializeField] private LightingPreset lightPreset;

    private void Start()
    {
        timeOfDay = 10;
    }

    private void Update()
    {
        if (lightPreset == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime * daySpeed;
            timeOfDay %= 24; // clamp between 0 - 24
            UpdateLighting(timeOfDay / 24f);
        }
    }

    public void UpdateTimeofDay(float sliderPosition)
    {
        timeOfDay = sliderPosition / 24f;
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = lightPreset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = lightPreset.FogColor.Evaluate(timePercent);

        if (directLight != null)
        {
            directLight.color = lightPreset.DirectionalColor.Evaluate(timePercent);
            directLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
        }
    }
}
