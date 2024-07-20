using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light2D sunLight;
    [SerializeField] private Transform player;
    [SerializeField] private float dayDuration = 24f;
    [SerializeField] private float startTime = 0.25f;
    [SerializeField] private float maxLightIntensity = 1f;
    [SerializeField] private float minLightIntensity = 0.1f;
    [SerializeField] private Color dayColor = Color.white;
    [SerializeField] private Color nightColor = new Color(0.2f, 0.2f, 0.5f);
    [SerializeField] private Vector3 offsetFromCharacter = new Vector3(0, 2, 0);

    [SerializeField] private GameEvent nightEvent;
    [SerializeField] private GameEvent dayEvent;

    private float timeOfDay;
    private bool isDay = true;

    private void Start()
    {
        timeOfDay = startTime;
        if (sunLight == null)
        {
            sunLight = GetComponent<Light2D>();
        }

        if (player == null)
        {
            Debug.LogError("Character transform not set in DayNightCycle script!");
        }

        // Raise initial day/night event based on start time
        CheckAndRaiseDayNightEvent();
    }

    private void Update()
    {
        float previousTime = timeOfDay;

        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay >= 1f)
        {
            timeOfDay -= 1f;
        }

        float intensity = Mathf.Clamp01(-Mathf.Cos(timeOfDay * 2f * Mathf.PI));
        sunLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, intensity);

        sunLight.color = Color.Lerp(nightColor, dayColor, intensity);

        if (player != null)
        {
            transform.position = player.position + offsetFromCharacter;
        }

        // Check if we've crossed the day/night threshold
        if (CrossedDayNightThreshold(previousTime, timeOfDay))
        {
            CheckAndRaiseDayNightEvent();
        }
    }

    private bool CrossedDayNightThreshold(float previousTime, float currentTime)
    {
        // Define day as 6:00 AM to 6:00 PM (0.25 to 0.75 in normalized time)
        const float dayStart = 0.25f;
        const float dayEnd = 0.75f;

        bool wasDayBefore = (previousTime >= dayStart && previousTime < dayEnd);
        bool isDayNow = (currentTime >= dayStart && currentTime < dayEnd);

        return wasDayBefore != isDayNow;
    }

    private void CheckAndRaiseDayNightEvent()
    {
        // Define day as 6:00 AM to 6:00 PM (0.25 to 0.75 in normalized time)
        const float dayStart = 0.25f;
        const float dayEnd = 0.75f;

        if (timeOfDay >= dayStart && timeOfDay < dayEnd)
        {
            if (!isDay)
            {
                isDay = true;
                if (dayEvent != null)
                {
                    dayEvent.Raise();
                    Debug.Log("Day event raised");
                }
            }
        }
        else
        {
            if (isDay)
            {
                isDay = false;
                if (nightEvent != null)
                {
                    nightEvent.Raise();
                    Debug.Log("Night event raised");
                }
            }
        }
    }
}