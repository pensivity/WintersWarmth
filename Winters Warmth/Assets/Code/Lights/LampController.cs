using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WristLampController : MonoBehaviour
{
    [SerializeField] private float swayAmount = 0.1f;
    [SerializeField] private float swaySpeed = 2f;

    [SerializeField] private SpriteRenderer lampSprite;
    [SerializeField] private Light2D lampLight;

    private Vector3 initialLocalPosition;
    private float swayTime;

    private void Start()
    {
        if (lampSprite == null) lampSprite = GetComponent<SpriteRenderer>();
        if (lampLight == null) lampLight = GetComponent<Light2D>();

        initialLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        SwayLamp();
    }

    private void SwayLamp()
    {
        swayTime += Time.deltaTime * swaySpeed;
        Vector3 swayOffset = new Vector3(
            Mathf.Sin(swayTime) * swayAmount,
            Mathf.Cos(swayTime * 0.5f) * swayAmount * 0.5f,
            0
        );
        transform.localPosition = initialLocalPosition + swayOffset;
    }

    public void ToggleLamp()
    {
        if (lampSprite != null) lampSprite.enabled = !lampSprite.enabled;
        if (lampLight != null) lampLight.enabled = !lampLight.enabled;
    }

    public void TurnOnLamp(Component sender, object data)
    {
        if (lampSprite != null) lampSprite.enabled = true;
        if (lampLight != null) lampLight.enabled = true;
    }

    public void TurnOffLamp(Component sender, object data)
    {
        if (lampSprite != null) lampSprite.enabled = false;
        if (lampLight != null) lampLight.enabled = false;
    }
}