using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class PulsingLightController : MonoBehaviour
{
    [SerializeField] private Light2D[] controlledLights;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseAmount = 0.1f;
    [SerializeField] private bool startOn = true;

    private float[] baseIntensities;
    private float time;
    private bool isOn;

    private void Start()
    {
        if (controlledLights == null || controlledLights.Length == 0)
        {
            controlledLights = GetComponents<Light2D>();
        }

        if (controlledLights == null || controlledLights.Length == 0)
        {
            Debug.LogError("No Light2D components found for PulsingLightController!");
            enabled = false;
            return;
        }

        baseIntensities = controlledLights.Select(light => light.intensity).ToArray();
        isOn = startOn;
        SetLightState(isOn);
    }

    private void Update()
    {
        if (!isOn) return;

        time += Time.deltaTime * pulseSpeed;
        float pulseFactor = Mathf.Sin(time) * pulseAmount;

        for (int i = 0; i < controlledLights.Length; i++)
        {
            float pulseIntensity = baseIntensities[i] + pulseFactor;
            controlledLights[i].intensity = pulseIntensity;
        }
    }

    public void ToggleLight()
    {
        if (isOn)
            LightOff(this, null);
        else
            LightOn(this, null);
    }

    private void SetLightState(bool state)
    {
        isOn = state;
        for (int i = 0; i < controlledLights.Length; i++)
        {
            controlledLights[i].enabled = isOn;
            if (isOn)
            {
                controlledLights[i].intensity = baseIntensities[i];
            }
        }
    }

    public void LightOff(Component sender, object data)
    {
        SetLightState(false);
    }

    public void LightOn(Component sender, object data)
    {
        SetLightState(true);
    }
}