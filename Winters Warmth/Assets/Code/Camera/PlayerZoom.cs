using UnityEngine;

public class PlayerZoom : MonoBehaviour
{
    [SerializeField] private GameEvent zoomIn;
    [SerializeField] private GameEvent zoomOut;
    [SerializeField] private bool isZoomInTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isZoomInTrigger)
            {
                zoomIn.Raise();
            }
            else
            {
                zoomOut.Raise();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isZoomInTrigger)
            {
                zoomOut.Raise();
            }
            else
            {
                zoomIn.Raise();
            }
        }
    }
}