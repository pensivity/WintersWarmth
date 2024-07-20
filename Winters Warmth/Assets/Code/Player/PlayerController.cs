using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public int carryCapacity;
    [SerializeField] public int playerWarmth;


    private void Awake()
    {
        speed = 5f;
        carryCapacity = 5;
        playerWarmth = 100;
    }


}
