using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class LightFlickerEffect : MonoBehaviour
{
    [SerializeField] private new Light light;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField, Range(1, 5)] private int smoothing = 5;

    [SerializeField] private float lastSum;
     private Queue<float> smoothQueue;

    public void Reset()
    {
        smoothQueue.Clear();
        lastSum = 0;
    }

    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);

        if (light == null)
        {
            light = GetComponent<Light>();
        }

        StartCoroutine(DoLightFlicker());
    }

    private IEnumerator DoLightFlicker()
    {
        while (true)
        {
            while (smoothQueue.Count >= smoothing)
            {
                lastSum -= smoothQueue.Dequeue();
            }

            float rndmVal = Random.Range(minIntensity, maxIntensity);
            smoothQueue.Enqueue(rndmVal);
            lastSum += rndmVal;

            light.intensity = lastSum / smoothQueue.Count;
            yield return null;
        }
    }
}