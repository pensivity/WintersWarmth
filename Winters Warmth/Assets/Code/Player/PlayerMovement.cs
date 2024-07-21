using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 input;
    private Vector2 lastMovementDirection;

    [SerializeField] private PlayerController player;

    public enum Direction
    {
        UpRight,
        DownRight,
        DownLeft,
        UpLeft
    }

    private void Awake()
    {
        player = gameObject.GetComponent<PlayerController>();
        lastMovementDirection = Vector2.up + Vector2.right; // Default to UpRight
    }

    private void Update()
    {
        GetPlayerInput();
        PlayerMove();
    }

    void GetPlayerInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        input = new Vector2(horizontal, vertical).normalized;

        // Update last movement direction if there's input
        if (input.magnitude > 0.1f)
        {
            lastMovementDirection = input;
        }

        input *= player.speed;
    }

    void PlayerMove()
    {
        Vector3 movement = new Vector3(input.x, input.y, 0) * Time.deltaTime;
        player.transform.position += movement;
    }

    public bool IsMoving()
    {
        return input.magnitude > 0.1f;

    }

    public Direction GetCurrentDirection()
    {
        if (lastMovementDirection.y > 0)
        {
            return lastMovementDirection.x >= 0 ? Direction.UpRight : Direction.UpLeft;
        }
        else
        {
            return lastMovementDirection.x >= 0 ? Direction.DownRight : Direction.DownLeft;
        }
    }
}