using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public float interactionRadius = 2f;
    public GameObject interactionPromptPrefab;
    public KeyCode interactionKey = KeyCode.E;
    public float appearanceHeight = 0.5f;
    public float animationDuration = 0.5f;

    // Other scripts
    private Transform player;
    private PlayerController playerController;
    private MenuManager menus;


    private bool canInteract = false;
    private GameObject instantiatedPrompt;
    private Coroutine shakeCoroutine;
    private Vector3 promptStartPosition;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= interactionRadius)
        {
            if (!canInteract)
            {
                canInteract = true;
                ShowInteractionPrompt();
            }

            if (Input.GetKeyDown(interactionKey))
            {
                Interact();
            }
        }
        else if (canInteract)
        {
            canInteract = false;
            HideInteractionPrompt();
        }
    }

    void ShowInteractionPrompt()
    {
        if (interactionPromptPrefab != null && instantiatedPrompt == null)
        {
            promptStartPosition = transform.position + Vector3.up;
            instantiatedPrompt = Instantiate(interactionPromptPrefab, promptStartPosition, Quaternion.identity);
            instantiatedPrompt.transform.SetParent(transform);
            instantiatedPrompt.transform.localScale = Vector3.zero;
            StartCoroutine(StartAnimation(instantiatedPrompt.transform));
        }
    }

    void HideInteractionPrompt()
    {
        if (instantiatedPrompt != null)
        {
            StartCoroutine(DisappearAnimation(instantiatedPrompt.transform));
        }
    }

    void Interact()
    {
        switch(gameObject.tag)
        {
            case "Fuel":
                if (playerController.carryCapacity < playerController.maxCapacity)
                {
                    // add to the player's carried fuel
                    playerController.carryCapacity++;
                    menus.RefreshHUD();

                    // destroy this object
                    SafeDestroyPrompt();
                    Destroy(gameObject);
                } else
                {
                    Debug.Log("Carrying too much!");
                    // TODO: Implement UI popup
                }
                
                break;

            default:
                Debug.Log("Interacting with " + gameObject.name);
                break;
        }
    }

    IEnumerator StartAnimation(Transform objectToAnimate)
    {
        if (objectToAnimate == null) yield break;

        Vector3 endPosition = promptStartPosition + Vector3.up * appearanceHeight;
        Vector3 originalScale = interactionPromptPrefab.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration && objectToAnimate != null)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / animationDuration;
            float smoothPercentComplete = Mathf.SmoothStep(0, 1, percentComplete);

            objectToAnimate.position = Vector3.Lerp(promptStartPosition, endPosition, smoothPercentComplete);
            objectToAnimate.localScale = Vector3.Lerp(Vector3.zero, originalScale, smoothPercentComplete);

            yield return null;
        }

        if (objectToAnimate != null)
        {
            shakeCoroutine = StartCoroutine(SmoothShakeAnimation(objectToAnimate));
        }
    }

    IEnumerator DisappearAnimation(Transform objectToAnimate)
    {
        if (objectToAnimate == null) yield break;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        Vector3 startPosition = objectToAnimate.position;
        Vector3 originalScale = objectToAnimate.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration && objectToAnimate != null)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / animationDuration;
            float smoothPercentComplete = Mathf.SmoothStep(0, 1, percentComplete);

            objectToAnimate.position = Vector3.Lerp(startPosition, promptStartPosition, smoothPercentComplete);
            objectToAnimate.localScale = Vector3.Lerp(originalScale, Vector3.zero, smoothPercentComplete);

            yield return null;
        }

        SafeDestroyPrompt();
    }

    IEnumerator SmoothShakeAnimation(Transform objectToShake)
    {
        if (objectToShake == null) yield break;

        Vector3 startPosition = objectToShake.localPosition;
        float shakeAmount = 0.05f;
        float shakePeriod = 0.5f;

        while (objectToShake != null)
        {
            float timeSinceStart = Time.time;

            float x = Mathf.Sin(timeSinceStart * Mathf.PI * 2 / shakePeriod) * shakeAmount;
            float y = Mathf.Sin(timeSinceStart * Mathf.PI * 2 / shakePeriod + Mathf.PI / 2) * shakeAmount;

            objectToShake.localPosition = new Vector3(startPosition.x + x, startPosition.y + y, startPosition.z);
            yield return null;
        }
    }

    void SafeDestroyPrompt()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        if (instantiatedPrompt != null)
        {
            Destroy(instantiatedPrompt);
            instantiatedPrompt = null;
        }
    }

    void OnDisable()
    {
        SafeDestroyPrompt();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}