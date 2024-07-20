using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PulsingLightController : MonoBehaviour
{
    [SerializeField] private Light2D controlledLight;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseAmount = 0.1f;
    [SerializeField] private bool startOn = true;

    private float baseIntensity;
    private float time;
    private bool isOn;

    private void Start()
    {
        if (controlledLight == null)
        {
            controlledLight = GetComponent<Light2D>();
        }

        if (controlledLight == null)
        {
            Debug.LogError("No Light2D component found for PulsingLightController!");
            enabled = false;
            return;
        }

        baseIntensity = controlledLight.intensity;
        isOn = startOn;
        SetLightState(isOn);
    }

    private void Update()
    {
        if (!isOn) return;

        time += Time.deltaTime * pulseSpeed;
        float pulseIntensity = baseIntensity + Mathf.Sin(time) * pulseAmount;
        controlledLight.intensity = pulseIntensity;
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
        controlledLight.enabled = isOn;

        if (isOn)
        {
            controlledLight.intensity = baseIntensity;
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