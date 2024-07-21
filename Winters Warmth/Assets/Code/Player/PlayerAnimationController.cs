using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Breathing Animation")]
    [SerializeField] private float breathingSpeed = 1f;
    [SerializeField] private float breathingAmount = 0.05f;

    private const string IS_WALKING = "IsWalking";
    private Vector3 originalScale;
    private Coroutine breathingCoroutine;

    void Start()
    {
        originalScale = transform.localScale;
        StartBreathingAnimation();
    }

    void Update()
    {
        UpdateAnimationState();
    }

    void UpdateAnimationState()
    {
        if (playerMovement != null)
        {
            bool isWalking = playerMovement.IsMoving();
            animator.SetBool(IS_WALKING, isWalking);

            PlayerMovement.Direction currentDirection = playerMovement.GetCurrentDirection();

            if (currentDirection == PlayerMovement.Direction.UpLeft || currentDirection == PlayerMovement.Direction.DownLeft)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            if (isWalking)
            {
                StopBreathingAnimation();
            }
            else
            {
                StartBreathingAnimation();
            }
        }
    }

    void StartBreathingAnimation()
    {
        if (breathingCoroutine == null)
        {
            breathingCoroutine = StartCoroutine(BreathingAnimation());
        }
    }

    void StopBreathingAnimation()
    {
        if (breathingCoroutine != null)
        {
            StopCoroutine(breathingCoroutine);
            breathingCoroutine = null;
            transform.localScale = originalScale;
        }
    }

    IEnumerator BreathingAnimation()
    {
        while (true)
        {
            yield return ScaleOverTime(originalScale, originalScale * (1 + breathingAmount), breathingSpeed / 2);
            yield return ScaleOverTime(originalScale * (1 + breathingAmount), originalScale, breathingSpeed / 2);
        }
    }

    IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
    }
}