using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private const string IS_WALKING = "IsWalking";

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
        }
    }
}