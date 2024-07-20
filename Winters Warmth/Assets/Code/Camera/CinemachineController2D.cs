using UnityEngine;
using Unity.Cinemachine;

public class CinemachineController2D : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;

    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minOrthoSize = 5f;
    [SerializeField] private float maxOrthoSize = 15f;

    [Header("Zoom In/Out"), Space(25)]
    [SerializeField] private float zoomIn;
    [SerializeField] private float zoomOut;

    private float targetOrthoSize;

    private void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineCamera>();
        }

        targetOrthoSize = virtualCamera.Lens.OrthographicSize;
    }

    private void Update()
    {
        float currentOrthoSize = virtualCamera.Lens.OrthographicSize;
        virtualCamera.Lens.OrthographicSize = Mathf.Lerp(currentOrthoSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
    }

    public void ZoomIn(float amount)
    {
        targetOrthoSize = Mathf.Max(virtualCamera.Lens.OrthographicSize - amount, minOrthoSize);
    }

    public void ZoomOut(float amount)
    {
        targetOrthoSize = Mathf.Min(virtualCamera.Lens.OrthographicSize + amount, maxOrthoSize);
    }

    public void SetZoom(float orthoSize)
    {
        targetOrthoSize = Mathf.Clamp(orthoSize, minOrthoSize, maxOrthoSize);
    }


    public void ZoomInEvent(Component sender, object data)
    {
        ZoomIn(zoomIn);
    }

    public void ZoomOutEvent(Component sender, object data)
    {
        Debug.Log("Zooming out");
        ZoomOut(zoomOut);
    }
}