using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public float interactionRadius = 2f;
    public GameObject interactionPromptPrefab;
    public KeyCode interactionKey = KeyCode.E;
    public KeyCode advanceConversationKey = KeyCode.Space;
    public float appearanceHeight = 0.5f;
    public float animationDuration = 0.5f;

    [SerializeField] private Transform player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private string conversationID;
    [SerializeField] private Vector3 newPosition;
    [SerializeField] private bool shouldTeleport = false;
    [SerializeField] private bool shouldDelete = false;

    private bool canInteract = false;
    private bool isInConversation = false;
    private GameObject instantiatedPrompt;
    private Coroutine shakeCoroutine;
    private Vector3 promptStartPosition;

    // Static reference to the currently talking NPC
    private static Interactable currentlyTalkingNPC;

    void Start()
    {
        StartCoroutine(InitializeInteractable());
    }

    private IEnumerator InitializeInteractable()
    {
        while (player == null || playerController == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            playerController = GameObject.Find("PlayerController")?.GetComponent<PlayerController>();
            yield return null;
        }

        while (ConversationSystemManager.Instance == null)
        {
            yield return null;
        }

        ConversationSystemManager.Instance.OnConversationEnded += OnConversationEnded;
    }

    void OnDestroy()
    {
        if (ConversationSystemManager.Instance != null)
        {
            ConversationSystemManager.Instance.OnConversationEnded -= OnConversationEnded;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (isInConversation && this == currentlyTalkingNPC)
        {
            if (Input.GetKeyDown(advanceConversationKey))
            {
                AdvanceConversation();
            }
        }
        else if (Vector2.Distance(transform.position, player.position) <= interactionRadius)
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

    void Interact()
    {
        switch (gameObject.tag)
        {
            case "Fuel":
                if (playerController != null && playerController.carryCapacity < playerController.maxCapacity)
                {
                    playerController.carryCapacity++;
                    SafeDestroyPrompt();
                    if (shouldDelete)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Debug.Log("Carrying too much!");
                    // TODO: Implement UI popup
                }
                break;
            case "NPC":
                Debug.Log("Interacting with NPC");
                if (ConversationSystemManager.Instance != null)
                {
                    ConversationSystemManager.Instance.StartConversation(conversationID);
                    isInConversation = true;
                    currentlyTalkingNPC = this;
                    HideInteractionPrompt();
                }
                break;
            default:
                Debug.Log("Interacting with " + gameObject.name);
                break;
        }
    }

    void AdvanceConversation()
    {
        if (ConversationSystemManager.Instance != null)
        {
            ConversationSystemManager.Instance.AdvanceConversation();
        }
    }

    void OnConversationEnded(string endedConversationID)
    {
        if (isInConversation && this == currentlyTalkingNPC)
        {
            isInConversation = false;
            currentlyTalkingNPC = null;
            HandlePostConversation();
        }
    }

    void HandlePostConversation()
    {
        if (shouldTeleport)
        {
            transform.position = newPosition;
        }

        if (shouldDelete)
        {
            Destroy(gameObject);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}