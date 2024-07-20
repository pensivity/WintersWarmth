using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 input;

    [SerializeField] private PlayerController player;


    private void Awake()
    {
        // Get the character controller script
        player = gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Call it in update so timing doesn't mess with input
        GetPlayerInput();

        // Move the character
        PlayerMove();
    }

    void GetPlayerInput()
    {
        // Get (normalised) player direction
        input = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical")).normalized;

        // increase by speed
        input *= player.speed;
    }

    void PlayerMove()
    {        
        // move the character
        player.transform.position += input * Time.deltaTime;
    }
}
